using UnityEngine;

public class CreateRandomGameObjects : MonoBehaviour
{
    public GameObject[] Prefabs;
    public GameObject Parent;
    public int NumberOfObjects = 10;
    public float XMin = 25f;
    public float XMax = 150f;
    public float YMin = 8.5f;
    public float YMax = 1.5f;
    public bool RandomizeSize;
    public float MinScale = 1;
    public float MaxScale = 2;
    public bool RandomizeInitialRotation;

    public void Awake()
    {
        var random = new System.Random();
        for (int i = 0; i < NumberOfObjects; i++)
        {
            var position = new Vector3(
                Random.Range(XMin, XMax),
                Random.Range(YMin, YMax),
                0);

            var prefab = random.NextFrom(Prefabs);
            var obj = Instantiate(prefab, position, Quaternion.identity);

            if (RandomizeSize)
            {
                float scale = Random.Range(MinScale, MaxScale);
                obj.transform.localScale = new Vector3(scale, scale, 0);
            }

            if (RandomizeInitialRotation)
            {
                //int rotation = Random.Range(0, 359);
                //obj.transform.Rotate(0, 0, rotation);

                // We only rotate the object with the Sprite Renderer
                // Try to get Sprite Renderer on the main object.
                GameObject gameObjectToRotate = null;

                if (obj.GetComponent<SpriteRenderer>() != null)
                {
                    gameObjectToRotate = obj;
                }
                else
                {
                    // If not found, get it from a child object
                    var spriteRenderer = obj.GetComponentInChildren<SpriteRenderer>();

                    if (spriteRenderer != null)
                    {
                        gameObjectToRotate = spriteRenderer.gameObject;
                    }
                }

                if (gameObjectToRotate != null)
                {
                    int rotation = Random.Range(0, 359);
                    gameObjectToRotate.transform.Rotate(0, 0, rotation);
                }
            }

            obj.transform.parent = Parent.transform;
        }
    }
}