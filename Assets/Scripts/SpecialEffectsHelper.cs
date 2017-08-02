using UnityEngine;

/// <summary>
/// Creating instance of particles from code with no effort
/// </summary>
public class SpecialEffectsHelper : MonoBehaviour
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static SpecialEffectsHelper Instance;

    public ParticleSystem ExplosionEffect;

    private void Awake()
    {
        // Register the singleton
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SpecialEffectsHelper!");
        }
        Instance = this;
    }

    /// <summary>
    /// Create an explosion at the given location
    /// </summary>
    /// <param name="position"></param>
    public void Explosion(Vector3 position)
    {
        if (ExplosionEffect != null)
        {
            InstantiateParticleSystem(ExplosionEffect, position);
        }
    }

    /// <summary>
    /// Instantiate a Particle system from prefab
    /// </summary>
    /// <param name="particleSystem"></param>
    /// <returns></returns>
    private ParticleSystem InstantiateParticleSystem(ParticleSystem particleSystem, Vector3 position)
    {
        var newParticleSystem = Instantiate(particleSystem, position, Quaternion.identity);

        // Make sure it will be destroyed
        Destroy(newParticleSystem.gameObject, newParticleSystem.startLifetime);

        return newParticleSystem;
    }
}