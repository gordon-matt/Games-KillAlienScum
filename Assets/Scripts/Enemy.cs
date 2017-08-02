using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool hasSpawn;
    private MoveController moveController;
    private Weapon[] weapons;

    private void Awake()
    {
        // Retrieve the weapon only once
        weapons = GetComponentsInChildren<Weapon>();

        // Retrieve scripts to disable when not spawn
        moveController = GetComponent<MoveController>();
    }

    private void Start()
    {
        hasSpawn = false;

        // Disable everything
        // -- collider
        GetComponent<Collider2D>().enabled = false;
        // -- Moving
        moveController.enabled = false;
        // -- Shooting
        foreach (Weapon weapon in weapons)
        {
            weapon.enabled = false;
        }
    }

    private void Update()
    {
        // Check if the enemy has spawned
        if (hasSpawn == false)
        {
            if (GetComponent<Renderer>().IsVisibleFrom(Camera.main))
            {
                Spawn();
            }
        }
        else
        {
            // Auto-fire
            foreach (var weapon in weapons)
            {
                if (weapon != null && weapon.enabled && weapon.CanAttack)
                {
                    weapon.Attack(true);
                    SoundEffectsHelper.Instance.MakeEnemyShotSound();
                }
            }

            // Out of camera?
            if (GetComponent<Renderer>().IsVisibleFrom(Camera.main) == false)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Spawn()
    {
        hasSpawn = true;

        // Enable everything
        // -- Collider
        GetComponent<Collider2D>().enabled = true;
        // -- Moving
        moveController.enabled = true;
        // -- Shooting
        foreach (Weapon weapon in weapons)
        {
            weapon.enabled = true;
        }
    }
}