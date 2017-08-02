using UnityEngine;

public class SortParticleSystem : MonoBehaviour
{
    public string SortingLayerName = "Particles";
    public int SortingOrder = 0;

    public void Start()
    {
        var renderer = GetComponent<ParticleSystem>().GetComponent<Renderer>();
        renderer.sortingLayerName = SortingLayerName;
        renderer.sortingOrder = SortingOrder;
    }
}