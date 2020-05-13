using System.Linq;
using UnityEngine;

public class ZapAttack : MonoBehaviour
{
    private LevelMaker levelMaker;

    public void Start()
    {
        levelMaker = GameObject.FindWithTag("Level Maker").GetComponent<LevelMaker>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            gameObject.transform.localScale = Vector3.zero; // make invisible. We could use gameObject.SetActive(false), but that would not play the sound then.
            
            var enemies = FindObjectsOfType<Enemy>();
            int enemyCount = enemies.Count();

            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Health>().Kill();
            }

            if (enemyCount >= 2) // this shout is stronger
            {
                SoundEffectsHelper.Instance.PlaySound(SoundType.YesShout2);
            }
            else if (enemyCount == 1)
            {
                SoundEffectsHelper.Instance.PlaySound(SoundType.YesShout1);
            }

            SpecialEffectsHelper.Instance.ZapAttack(other.transform.position);
            SoundEffectsHelper.Instance.PlaySound(SoundType.ZapAttack);

            Destroy(gameObject);
        }
    }
}