using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 0 - The speed of the ship
    /// </summary>
    public Vector2 Speed = new Vector2(25, 25);

    // 1 - Store the movement
    private Vector2 movement;

    private Rigidbody2D rigidBody;

    private Health playerHealth;

    public void Awake()
    {
        playerHealth = GetComponent<Health>();
    }

    public void Update()
    {
        // 2 - Retrieve axis information
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // 3 - Movement per direction
        movement = new Vector2(Speed.x * inputX, Speed.y * inputY);

        // 5 - Shooting
        bool shoot =
            Input.GetButtonDown("Fire1") |
            Input.GetButtonDown("Fire2"); // For Mac users, ctrl + arrow is a bad idea

        if (shoot)
        {
            var weapon = GetComponent<Weapon>();
            if (weapon != null && weapon.CanAttack)
            {
                weapon.Attack(false);
                SoundEffectsHelper.Instance.PlaySound(SoundType.PlayerProjectile);
            }
        }

        // 6 - Make sure we are not outside the camera bounds
        float distance = (transform.position - Camera.main.transform.position).z;
        float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
        float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;
        float topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y;
        float bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
            Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
            transform.position.z);
    }

    public void FixedUpdate()
    {
        // 4 - Move the game object
        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }
        rigidBody.velocity = movement;
    }

    public void OnDestroy()
    {
        // Check that the player is dead, as we is also callled when closing Unity
        if (playerHealth != null && playerHealth.HitPointsLeft <= 0)
        {
            // Game Over.
            var gameOver = FindObjectOfType<GameOver>();
            gameOver.ShowCanvas();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        bool damagePlayer = false;

        // Collision with enemy
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Kill the enemy
            var enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(enemyHealth.HitPointsLeft);
            }

            damagePlayer = true;
        }

        // Collision with the boss
        var boss = collision.gameObject.GetComponent<Boss>();
        if (boss != null)
        {
            // Boss lose some hp too
            var bossHealth = boss.GetComponent<Health>();
            if (bossHealth != null)
            {
                bossHealth.Damage(5);
            }

            damagePlayer = true;
        }

        // Damage the player
        if (damagePlayer)
        {
            if (playerHealth != null)
            {
                playerHealth.Damage(1);
            }
        }
    }

    public void FinishLevel()
    {
        enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    public void RespawnAt(Transform spawnPoint)
    {
        GetComponent<Collider2D>().enabled = true;
        playerHealth.ResetHitPoints();
        transform.position = spawnPoint.position;
        //Camera.main.transform.position = spawnPoint.position;
    }
}