using UnityEngine;

public class SoundEffectsHelper : MonoBehaviour
{
    public static SoundEffectsHelper Instance;

    public AudioClip ExplosionSound;
    public AudioClip PlayerProjectileSound;
    public AudioClip EnemyProjectileSound;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SoundEffectsHelper!");
        }
        Instance = this;
    }

    public void PlaySound(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Explosion: PlaySound(ExplosionSound); break;
            case SoundType.PlayerProjectile: PlaySound(PlayerProjectileSound); break;
            case SoundType.EnemyProjectile: PlaySound(EnemyProjectileSound); break;
        }
    }

    private void PlaySound(AudioClip originalClip)
    {
        // We use the camera's position to ensure 2D audio
        AudioSource.PlayClipAtPoint(originalClip, Camera.main.transform.position);
    }
}

public enum SoundType : byte
{
    Explosion,
    PlayerProjectile,
    EnemyProjectile
}