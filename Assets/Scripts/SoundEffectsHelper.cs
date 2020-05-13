using UnityEngine;

public class SoundEffectsHelper : MonoBehaviour
{
    public static SoundEffectsHelper Instance;

    public AudioClip ExplosionSound;
    public AudioClip PlayerProjectileSound;
    public AudioClip EnemyProjectileSound;
    public AudioClip RestoreHealthSound;
    public AudioClip ZapAttackSound;

    public AudioClip[] Expletives;
    public AudioClip[] TriumphantShouts;

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
            case SoundType.RestoreHealth: PlaySound(RestoreHealthSound); break;
            case SoundType.ZapAttack: PlaySound(ZapAttackSound); break;
            case SoundType.OhShit1: PlaySound(Expletives[0]); break;
            case SoundType.OhShit2: PlaySound(Expletives[1]); break;
            case SoundType.OhShit3: PlaySound(Expletives[2]); break;
            case SoundType.HolyShit: PlaySound(Expletives[3]); break;
            case SoundType.YesShout1: PlaySound(TriumphantShouts[0]); break;
            case SoundType.YesShout2: PlaySound(TriumphantShouts[1]); break;
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
    EnemyProjectile,
    RestoreHealth,
    ZapAttack,
    OhShit1,
    OhShit2,
    OhShit3,
    HolyShit,
    YesShout1,
    YesShout2
}