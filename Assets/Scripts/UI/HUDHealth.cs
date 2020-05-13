using UnityEngine;

public class HUDHealth : MonoBehaviour
{
    public PlayerController Player;

    private Healthbar healthbar;
    private Health playerHealth;

    private void Start()
    {
        healthbar = GetComponent<Healthbar>();
        playerHealth = Player.GetComponent<Health>();
    }

    private void Update()
    {
        float health = ((float)playerHealth.HitPointsLeft / playerHealth.HitPoints) * 100;
        healthbar.SetHealth(health);
    }
}