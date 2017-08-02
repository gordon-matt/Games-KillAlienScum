using UnityEngine;

public class Health : MonoBehaviour
{
    public int HitPoints = 2;
    public bool IsEnemy = true;

    public void OnTriggerEnter2D(Collider2D other)
    {
        var projectile = other.gameObject.GetComponent<Projectile>();

        if (projectile == null)
        {
            return;
        }

        if (projectile.IsEnemyProjectile != IsEnemy)
        {
            HitPoints -= projectile.Damage;
            Destroy(projectile.gameObject);

            if (HitPoints <= 0)
            {
                SpecialEffectsHelper.Instance.Explosion(transform.position);
                SoundEffectsHelper.Instance.MakeExplosionSound();
                Destroy(gameObject);
            }
        }
    }
}