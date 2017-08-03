using UnityEngine;

public class Boss : MonoBehaviour
{
    public float MaxAttackCooldown = 2f;
    public float MinAttackCooldown = 0.5f;

    private float aiCooldown;
    private Animator animator;
    private bool hasSpawned;
    private bool isAttacking;

    private MoveController moveController;
    private Vector2 positionTarget;
    private SpriteRenderer[] renderers;
    private Weapon[] weapons;

    private void Awake()
    {
        weapons = GetComponentsInChildren<Weapon>();
        moveController = GetComponent<MoveController>();
        animator = GetComponent<Animator>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnDrawGizmos()
    {
        if (hasSpawned && !isAttacking)
        {
            Gizmos.DrawSphere(positionTarget, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider2D)
    {
        var projectile = otherCollider2D.gameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            if (!projectile.IsEnemyProjectile)
            {
                aiCooldown = Random.Range(MinAttackCooldown, MaxAttackCooldown);
                isAttacking = false;
                animator.SetTrigger("Hit");
            }
        }
    }

    private void Spawn()
    {
        hasSpawned = true;

        GetComponent<Collider2D>().enabled = true;
        moveController.enabled = true;
        foreach (var weapon in weapons)
        {
            weapon.enabled = true;
        }

        foreach (var scrolling in FindObjectsOfType<Scrolling>())
        {
            if (scrolling.IsLinkedToCamera)
            {
                scrolling.Speed = Vector2.zero;
            }
        }
    }

    private void Start()
    {
        hasSpawned = false;
        GetComponent<Collider2D>().enabled = false;
        moveController.enabled = false;

        foreach (var weapon in weapons)
        {
            weapon.enabled = false;
        }

        isAttacking = false;
        aiCooldown = MaxAttackCooldown;
    }

    private void Update()
    {
        if (!hasSpawned)
        {
            if (renderers[0].IsVisibleFrom(Camera.main))
            {
                Spawn();
            }
        }
        else
        {
            aiCooldown -= Time.deltaTime;

            if (aiCooldown <= 0f)
            {
                isAttacking = !isAttacking;
                aiCooldown = Random.Range(MinAttackCooldown, MaxAttackCooldown);
                positionTarget = Vector2.zero;

                animator.SetBool("Attack", isAttacking);
            }

            if (isAttacking)
            {
                moveController.Direction = Vector2.zero;

                foreach (var weapon in weapons)
                {
                    if (weapon != null && weapon.enabled && weapon.CanAttack)
                    {
                        weapon.Attack(true);
                        SoundEffectsHelper.Instance.PlaySound(SoundType.EnemyProjectile);
                    }
                }
            }
            else
            {
                if (positionTarget == Vector2.zero)
                {
                    var randomPoint = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
                    positionTarget = Camera.main.ViewportToWorldPoint(randomPoint);
                }

                if (GetComponent<Collider2D>().OverlapPoint(positionTarget))
                {
                    positionTarget = Vector2.zero;
                }

                var direction = ((Vector3)positionTarget - transform.position);

                moveController.Direction = Vector3.Normalize(direction);
            }
        }
    }
}