using UnityEngine;

public class LinkLifetimeToCamera : MonoBehaviour
{
    private new Renderer renderer;
    private bool hasAppeared;

    public void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public void Start()
    {
        hasAppeared = false;
    }

    public void Update()
    {
        if (!hasAppeared)
        {
            if (renderer.IsVisibleFrom(Camera.main))
            {
                hasAppeared = true;
            }
        }
        else
        {
            if (!renderer.IsVisibleFrom(Camera.main))
            {
                Destroy(gameObject);
            }
        }
    }
}