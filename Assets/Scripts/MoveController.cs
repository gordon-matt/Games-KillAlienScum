using UnityEngine;

public class MoveController : MonoBehaviour
{
    public Vector2 Speed = new Vector2(7, 7);
    public Vector2 Direction = Vector2.left;

    public void Update()
    {
        var movement = new Vector3(
            Speed.x * Direction.x,
            Speed.y * Direction.y,
            0);

        movement *= Time.deltaTime;

        transform.Translate(movement);
    }
}