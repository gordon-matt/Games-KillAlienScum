using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage = 1;
    public int SecondsToLive = 20;
    public bool IsEnemyProjectile = false;

    public void Start()
    {
        Destroy(gameObject, SecondsToLive);
    }
}