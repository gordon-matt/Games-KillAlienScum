using UnityEngine;

public class Health : MonoBehaviour
{
    public int HitPoints = 2;

    public bool IsEnemy = true;

    public bool IsProjectile = false;

    public int HitPointsLeft { get; private set; }

    private LevelMaker levelMaker;

    public void Awake()
    {
        HitPointsLeft = HitPoints;
    }

    public void Start()
    {
        levelMaker = GameObject.FindWithTag("Level Maker").GetComponent<LevelMaker>();
    }

    public void Damage(int damageCount)
    {
        HitPointsLeft -= damageCount;

        if (HitPointsLeft <= 0)
        {
            DoKill();
        }
        else if (!IsEnemy && !IsProjectile)
        {
            float healthPercent = ((float)HitPointsLeft / HitPoints) * 100;

            if (healthPercent <= 25)
            {
                SoundEffectsHelper.Instance.PlaySound(SoundType.HolyShit);
            }
            else if (healthPercent <= 50)
            {
                SoundEffectsHelper.Instance.PlaySound(SoundType.OhShit3);
            }
            else if (healthPercent <= 75)
            {
                SoundEffectsHelper.Instance.PlaySound(SoundType.OhShit2);
            }
            else
            {
                SoundEffectsHelper.Instance.PlaySound(SoundType.OhShit1);
            }
        }
    }

    public void RecoverHealth(int hp)
    {
        // Ensure we don't go past the max hitpoints
        HitPointsLeft = System.Math.Min(HitPointsLeft + hp, HitPoints);
    }

    public void Kill()
    {
        HitPointsLeft = 0;
        DoKill();
    }

    public void ResetHitPoints()
    {
        HitPointsLeft = HitPoints;
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

    private void DoKill()
    {
        // Explosion!
        SpecialEffectsHelper.Instance.Explosion(transform.position);
        SoundEffectsHelper.Instance.PlaySound(SoundType.Explosion);

        if (IsEnemy)
        {
            GameManager.Instance.AddPoints(1);

            // 1-in-15 chance to drop health pack.
            if (Random.Range(1, 15) == 1)
            {
                if (levelMaker.HealthPack != null)
                {
                    Instantiate(levelMaker.HealthPack, gameObject.transform.position, Quaternion.identity);
                }
            }
            // 1-in-20 chance to drop health pack.
            else if (Random.Range(1, 20) == 1)
            {
                if (levelMaker.ZapAttack != null)
                {
                    Instantiate(levelMaker.ZapAttack, gameObject.transform.position, Quaternion.identity);
                }
            }
        }

        // Dead!
        Destroy(gameObject);
    }
}