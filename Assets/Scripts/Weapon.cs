// Gained this script from doing this tutorial: https://pixelnest.io/tutorials/2d-game-unity/

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

            // Ensure the weapon always shoots towards the correct target
            var moveController = projectileTransform.gameObject.GetComponent<MoveController>();
            if (moveController != null)
            {
                // In 2D space, transform.right is forward for the sprite.
                moveController.Direction = transform.right;
            }
        }
    }

    public bool CanAttack
    {
        get { return cooldownTime <= 0f; }
    }
}