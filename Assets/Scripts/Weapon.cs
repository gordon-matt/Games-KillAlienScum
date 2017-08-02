using UnityEngine;

/// <summary>
/// Launch projectile
/// </summary>
public class Weapon : MonoBehaviour
{
    private float cooldownTime;

    public Transform ProjectilePrefab;
    public float FireRate = 0.25f;

    public void Start()
    {
        cooldownTime = 0f;
    }

    public void Update()
    {
        if (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
        }
    }

    public void Attack(bool isEnemy)
    {
        if (CanAttack)
        {
            cooldownTime = FireRate;

            var projectileTransform = Instantiate(ProjectilePrefab);
            projectileTransform.position = transform.position;

            // The is enemy property
            var projectile = projectileTransform.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.IsEnemyProjectile = isEnemy;
            }

            // Make the weapon shot always towards it
            var moveController = projectileTransform.gameObject.GetComponent<MoveController>();
            if (moveController != null)
            {
                moveController.Direction = transform.right; // towards in 2D space is the right of the sprite
            }

            if (isEnemy)
            {
                SoundEffectsHelper.Instance.MakeEnemyShotSound();
            }
            else
            {
                SoundEffectsHelper.Instance.MakePlayerShotSound();
            }
        }
    }

    public bool CanAttack
    {
        get { return cooldownTime <= 0f; }
    }
}