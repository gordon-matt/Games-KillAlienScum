using System.Linq;
using UnityEngine;

public class SpecialEffectsHelper : MonoBehaviour
{
    public static SpecialEffectsHelper Instance;

    public ParticleSystem ExplosionEffect;

    public ParticleSystem ZapAttackEffect;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SoundEffectsHelper!");
        }
        Instance = this;
    }

    public void Explosion(Vector3 position)
    {
        if (ExplosionEffect != null)
        {
            InstantiateParticleSystem(ExplosionEffect, position);
        }
    }

    public void ZapAttack(Vector3 position)
    {
        if (ZapAttackEffect != null)
        {
            InstantiateParticleSystem(ZapAttackEffect, position);
        }
    }

    private ParticleSystem InstantiateParticleSystem(ParticleSystem particleSystem, Vector3 position)
    {
        var particleSystemInstance = Instantiate(particleSystem, position, Quaternion.identity);

        float lifetime = 0f;

        var minMaxCurve = particleSystemInstance.main.startLifetime;
        switch (minMaxCurve.mode)
        {
            case ParticleSystemCurveMode.Constant: lifetime = minMaxCurve.constant; break;
            case ParticleSystemCurveMode.Curve: lifetime = minMaxCurve.curve.keys.Select(x => x.time).Sum(); break;
            case ParticleSystemCurveMode.TwoConstants: lifetime = minMaxCurve.constantMax; break;
            case ParticleSystemCurveMode.TwoCurves: lifetime = minMaxCurve.curveMax.keys.Select(x => x.time).Sum(); break;
        }

        // Ensure particle is destroyed
        Destroy(particleSystemInstance.gameObject, lifetime);

        return particleSystemInstance;
    }
}