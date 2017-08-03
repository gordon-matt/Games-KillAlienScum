using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Max time a player can go while still receiving a bonus
    /// </summary>
    public int BonusCutoffSeconds;

    /// <summary>
    /// How manys seconds left over * how many points you should receive
    /// </summary>
    public int BonusSecondMultiplier;

    public Checkpoint DebugSpawn;

    private List<Checkpoint> checkpoints;
    private int currentCheckpointIndex;
    private int savedPoints;
    private DateTime started;

    public static LevelManager Instance { get; private set; }

    public int CurrentTimeBonus
    {
        get
        {
            int secondsDifference = (int)(BonusCutoffSeconds - RunningTime.TotalSeconds);
            return Mathf.Max(0, secondsDifference) * BonusSecondMultiplier;
        }
    }

    public PlayerController Player { get; private set; }

    public TimeSpan RunningTime
    {
        get { return DateTime.UtcNow - started; }
    }

    public void Awake()
    {
        savedPoints = GameManager.Instance.Points;
        Instance = this;
    }

    public void KillPlayer()
    {
        StartCoroutine(KillPlayerCo());
    }

    public void LoadNextLevel(string levelName)
    {
        StartCoroutine(LoadNextLevelCo(levelName));
    }

    public void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>()
            .OrderBy(x => x.transform.position.x)
            .ToList();

        currentCheckpointIndex = checkpoints.Any() ? 0 : -1;

        Player = FindObjectOfType<PlayerController>();

        started = DateTime.UtcNow;

        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();

        foreach (var listener in listeners)
        {
            for (int i = checkpoints.Count - 1; i >= 0; i--)
            {
                // Get distance value between checkpoint and the object we're looking at.
                float distance = ((MonoBehaviour)listener).transform.position.x - checkpoints[i].transform.position.x;

                if (distance < 0)
                {
                    continue;
                }

                checkpoints[i].AssignObjectToCheckpoint(listener);
                break;
            }
        }

#if UNITY_EDITOR
        if (DebugSpawn != null)
        {
            DebugSpawn.SpawnPlayer(Player);
        }
        else if (currentCheckpointIndex != -1)
        {
            checkpoints[currentCheckpointIndex].SpawnPlayer(Player);
        }
#else
        if (currentCheckpointIndex != -1)
        {
            checkpoints[currentCheckpointIndex].SpawnPlayer(Player);
        }
#endif
    }

    public void Update()
    {
        int nextCheckpointIndex = currentCheckpointIndex + 1;
        bool isAtLastCheckpoint = nextCheckpointIndex >= checkpoints.Count;

        if (isAtLastCheckpoint)
        {
            return;
        }

        float distanceToNextCheckpoint = checkpoints[nextCheckpointIndex].transform.position.x - Player.transform.position.x;

        // If we haven't hit the next checkpoint yet.
        if (distanceToNextCheckpoint >= 0)
        {
            return;
        }

        // We've hit a checkpoint, so:
        checkpoints[currentCheckpointIndex].PlayerLeftCheckpoint();
        currentCheckpointIndex++;
        checkpoints[currentCheckpointIndex].PlayerHitCheckpoint();

        GameManager.Instance.AddPoints(CurrentTimeBonus);
        savedPoints = GameManager.Instance.Points;
        started = DateTime.UtcNow;
    }

    private IEnumerator KillPlayerCo()
    {
        Player.GetComponent<Health>().Kill();
        yield return new WaitForSeconds(2f);

        if (currentCheckpointIndex != -1)
        {
            checkpoints[currentCheckpointIndex].SpawnPlayer(Player);
        }

        started = DateTime.UtcNow;
        GameManager.Instance.ResetPoints(savedPoints);
    }

    private IEnumerator LoadNextLevelCo(string levelName)
    {
        Player.FinishLevel();
        GameManager.Instance.AddPoints(CurrentTimeBonus);

        FloatingText.Show(
            "Level Complete!",
            "CheckpointText",
            new CenteredTextPositioner(.2f));

        yield return new WaitForSeconds(1f);

        FloatingText.Show(
            string.Format("{0} points!", CurrentTimeBonus),
            "CheckpointText",
            new CenteredTextPositioner(.1f));

        yield return new WaitForSeconds(5f);

        if (string.IsNullOrEmpty(levelName))
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            SceneManager.LoadScene(levelName);
        }
    }
}