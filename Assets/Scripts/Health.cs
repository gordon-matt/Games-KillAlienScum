using UnityEngine;

public class Health : MonoBehaviour
{
    public int HitPoints = 2;

    public bool IsEnemy = true;

    public void Damage(int damageCount)
    {
        HitPoints -= damageCount;

        if (HitPoints <= 0)
        {
            // Explosion!
            SpecialEffectsHelper.Instance.Explosion(transform.position);
            SoundEffectsHelper.Instance.PlaySound(SoundType.Explosion);

            // Dead!
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        var shot = otherCollider.gameObject.GetComponent<Projectile>();
        if (shot == null)
        {
            return;
        }
        // Avoid friendly fire
        if (shot.IsEnemyProjectile != IsEnemy)
        {
            Damage(shot.Damage);

            // Destroy the shot
            Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
        }
    }
}