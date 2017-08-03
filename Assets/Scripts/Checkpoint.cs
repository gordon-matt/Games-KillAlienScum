using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private List<IPlayerRespawnListener> listeners;

    public void Awake()
    {
        listeners = new List<IPlayerRespawnListener>();
    }

    public void PlayerHitCheckpoint()
    {
        StartCoroutine(PlayerHitCheckpointCo(LevelManager.Instance.CurrentTimeBonus));
    }

    private IEnumerator PlayerHitCheckpointCo(int bonus)
    {
        FloatingText.Show(
            "Checkpoint!",
            "CheckpointText",
            new CenteredTextPositioner(.5f));

        yield return new WaitForSeconds(.5f);

        FloatingText.Show(
            string.Format("+{0} time bonus!", bonus),
            "CheckpointText",
            new CenteredTextPositioner(.5f));
    }

    public void PlayerLeftCheckpoint()
    {
    }

    public void SpawnPlayer(PlayerController player)
    {
        player.RespawnAt(transform);

        foreach (var listener in listeners)
        {
            listener.OnPlayerRespawnInThisCheckpoint(this, player);
        }
    }

    public void AssignObjectToCheckpoint(IPlayerRespawnListener listener)
    {
        listeners.Add(listener);
    }
}