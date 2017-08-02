using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 Speed = new Vector2(10, 10);

    public void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        var movement = new Vector3(
            Speed.x * inputX,
            Speed.y * inputY,
            0);

        movement *= Time.deltaTime;

        transform.Translate(movement);

        bool shoot =
            Input.GetButtonDown("Fire1") |
            Input.GetButtonDown("Fire2");

        if (shoot)
        {
            var weapon = GetComponent<Weapon>();

            if (weapon != null)
            {
                weapon.Attack(false);
            }
        }

        float distance = (transform.position - Camera.main.transform.position).z;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
            Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
            transform.position.z);
    }
}