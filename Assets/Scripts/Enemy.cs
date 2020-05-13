using UnityEngine;

public class Enemy : MonoBehaviour
{
    private new Renderer renderer;
    private Weapon[] weapons;
    private bool hasSpawned;

    public void Awake()
    {
        weapons = GetComponentsInChildren<Weapon>();
        renderer = GetComponent<Renderer>();
    }

    public void Start()
    {
        hasSpawned = false;

        foreach (var weapon in weapons)
        {
            weapon.enabled = false;
        }
    }

    public void Update()
    {
        if (!hasSpawned)
        {
            if (renderer.IsVisibleFrom(Camera.main))
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
                    SoundEffectsHelper.Instance.PlaySound(SoundType.EnemyProjectile);
                }
            }

            // Out of camera?
            if (!renderer.IsVisibleFrom(Camera.main))
            {
                Destroy(gameObject);
            }
        }
    }

    private void Spawn()
    {
        hasSpawned = true;

        foreach (var weapon in weapons)
        {
            weapon.enabled = true;
        }
    }
}