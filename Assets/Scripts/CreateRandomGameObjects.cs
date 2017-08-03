using UnityEngine;

public class CreateRandomGameObjects : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject Parent;
    public int NumberOfObjects = 10;
    public float XMin = 25f;
    public float XMax = 150f;
    public float YMin = 8.5f;
    public float YMax = 1.5f;

    public void Start()
    {
        for (int i = 0; i < NumberOfObjects; i++)
        {
            var position = new Vector3(
                Random.Range(XMin, XMax),
                Random.Range(YMin, YMax),
                0);

            var enemy = Instantiate(Prefab, position, Quaternion.identity);
            enemy.transform.parent = Parent.transform;
        }
    }
}