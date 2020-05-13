using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private Health playerHealth;

    private void Start()
    {
        var player = FindObjectOfType<PlayerController>();
        playerHealth = player.GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            gameObject.transform.localScale = Vector3.zero; // make invisible. We could use gameObject.SetActive(false), but that would not play the sound then.
            playerHealth.RecoverHealth(2);
            SoundEffectsHelper.Instance.PlaySound(SoundType.RestoreHealth);
            Destroy(gameObject);
        }
    }
}