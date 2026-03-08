using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour
{
    [Header("Data")]
    public EnemyData data;
    public System.Action OnDeath;

    [Header("Movement")]
    [SerializeField] private float stopDistanceUp = 4.5f; // Quái từ dưới đi lên
    [SerializeField] private float stopDistanceDown = 3.0f; // Quái từ trên đi xuống
    [SerializeField] private float stopDistanceSide = 4.0f; // Quái đi từ 2 bên
    [SerializeField] private float stopBuffer = 0.2f;

    [Header("Separation")]
    [SerializeField] private float separationRadius = 0.6f;
    [SerializeField] private float separationForce = 2f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Attack")]
    [SerializeField] private float waitBeforeAttack = 2f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("TNT")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 6f;

    [Header("Hiệu ứng UI")]
    public GameObject damagePopupPrefab;

    private Transform target;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private EnemyHealth health;

    private bool isMoving;

    private float waitTimer = 0f;
    private float attackTimer = 0f;
    private bool hasStartedAttack = false;

    private EnemyState currentState = EnemyState.Moving;
    [SerializeField] private float stunDuration = 0.4f;

    private enum EnemyState
    {
        Moving,
        PreparingAttack,
        Attacking,
        Stunned
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        health = GetComponent<EnemyHealth>();
    }

    void Start()
    {
        FindTarget();
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            SetMoving(false);
            return;
        }

        Vector2 targetPos = GetTargetPosition();
        Vector2 toTarget = targetPos - rb.position;
        float distance = toTarget.magnitude;

        FaceTarget(toTarget);

        float currentStopDistance = GetDirectionalStopDistance(toTarget);

        if (distance <= currentStopDistance + stopBuffer)
        {
            SetMoving(false);

            // Chưa đủ 2 giây → đếm thời gian chờ
            if (!hasStartedAttack)
            {
                waitTimer += Time.fixedDeltaTime;

                if (waitTimer >= waitBeforeAttack)
                {
                    hasStartedAttack = true;
                    attackTimer = 0f; // để đánh ngay sau khi chờ xong
                }
            }
            else
            {
                attackTimer -= Time.fixedDeltaTime;

                if (attackTimer <= 0f)
                {
                    AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

                    if (!state.IsName("Attack"))
                    {
                        animator.SetTrigger("Attack");
                        attackTimer = attackCooldown; // 🔥 QUAN TRỌNG
                    }
                }
            }

            return;
        }
        else
        {
            // Nếu ra khỏi vùng → reset lại
            waitTimer = 0f;
            hasStartedAttack = false;
        }

        Vector2 moveDir = toTarget.normalized;

        // Kiểm tra bị chặn phía trước
        bool blocked = IsBlocked(moveDir);

        // Lấy lực tách nếu quá gần
        Vector2 separation = GetSeparationForce();

        Vector2 finalDir = Vector2.zero;

        if (!blocked)
            finalDir += moveDir;

        finalDir += separation;

        if (finalDir.sqrMagnitude < 0.001f)
        {
            SetMoving(false);
            return;
        }

        finalDir.Normalize();

        SetMoving(true);

        Vector2 newPos = rb.position + finalDir * data.moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    bool IsBlocked(Vector2 moveDir)
    {
        RaycastHit2D hit = Physics2D.CircleCast(
            rb.position,
            0.25f,              // bán kính kiểm tra phía trước
            moveDir,
            0.4f,               // khoảng kiểm tra phía trước
            enemyLayer
        );

        if (hit.collider != null && hit.rigidbody != rb)
            return true;

        return false;
    }

    Vector2 GetSeparationForce()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            separationRadius,
            enemyLayer
        );

        Vector2 force = Vector2.zero;
        int count = 0;

        foreach (var hit in hits)
        {
            if (hit.attachedRigidbody == rb) continue;

            Vector2 diff = (Vector2)transform.position - (Vector2)hit.transform.position;
            float dist = diff.magnitude;

            if (dist > 0.01f)
            {
                force += diff.normalized / dist;
                count++;
            }
        }

        if (count > 0)
            force /= count;

        return force * separationForce;
    }

    void SetMoving(bool value)
    {
        if (isMoving == value) return;

        isMoving = value;
        animator.SetBool("isMoving", value);
    }

    Vector2 GetTargetPosition()
    {
        if (target == null) return rb.position;

        TagetPoint player = target.GetComponent<TagetPoint>();
        if (player != null && player.targetPoint != null)
            return player.targetPoint.position;

        return target.position;
    }

    void FaceTarget(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > 0.05f)
            sprite.flipX = direction.x < 0;
    }

    void FindTarget()
    {
        switch (data.targetType)
        {
            case EnemyTargetType.TheKeep:
                target = GameObject.FindWithTag("TheKeep")?.transform;
                break;

            case EnemyTargetType.Mines:
                target = FindClosestWithTag("Mine")
                         ?? GameObject.FindWithTag("TheKeep")?.transform;
                break;

            case EnemyTargetType.Towers:
                target = FindClosestWithTag("Tower")
                         ?? GameObject.FindWithTag("Player")?.transform;
                break;

            case EnemyTargetType.Hero:
                target = GameObject.FindWithTag("Player")?.transform;
                break;

            case EnemyTargetType.Sweep:
                target = FindClosestWithTag("EnemyTarget")
                         ?? GameObject.FindWithTag("Player")?.transform;
                break;
        }
    }

    Transform FindClosestWithTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        float minDist = Mathf.Infinity;
        Transform closest = null;

        foreach (var obj in objs)
        {
            float dist = Vector2.Distance(rb.position, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = obj.transform;
            }
        }

        return closest;
    }

    public void TakeDamage(float amount)
    {
        ResetAttackState();
        health?.TakeDamage(amount);
    }

    // Debug vẽ bán kính separation
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }

    private void ResetAttackState()
    {
        waitTimer = 0f;
        hasStartedAttack = false;
        attackTimer = 0f;
    }

    private float GetDirectionalStopDistance(Vector2 toTarget)
    {
        // Xét xem mục tiêu có nằm ngang hơn là dọc không
        if (Mathf.Abs(toTarget.x) > Mathf.Abs(toTarget.y))
            return stopDistanceSide; 
        
        // Mục tiêu nằm ở trên (y dương) → Quái vật đang đi từ dưới lên
        if (toTarget.y > 0)
            return stopDistanceUp; 
        
        // Mục tiêu nằm ở dưới (y âm) → Quái vật đang đi từ trên xuống
        return stopDistanceDown; 
    }

    public void SpawnProjectile()
    {
        if (target == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Throw_Taget projectile = proj.GetComponent<Throw_Taget>();
        projectile.Init(target, projectileSpeed);
    }
}