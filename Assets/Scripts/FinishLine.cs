using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public string NextLevelName;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() == null)
        {
            return;
        }

        LevelManager.Instance.LoadNextLevel(NextLevelName);
    }
}