using UnityEngine;

public class KnightAI : MonoBehaviour
{
    [Header("Cài đặt chung")]
    public BarrackTower parentBarrack;
    public float moveSpeed = 2f;
    public float attackRange = 0.5f; // Tầm đánh cận chiến nên nhỏ (0.5 - 0.8)

    [Header("Cài đặt chiến đấu")]
    public float damage = 10f;       // Sát thương mỗi nhát chém
    public float attackRate = 1f;    // Tốc độ đánh (1 lần/giây)
    private float attackCountdown = 0f;

    private Transform target;
    private Vector3 spawnPosition;
    private Animator anim;
    private SpriteRenderer sr;

    void Start()
    {
        spawnPosition = transform.position;
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Giảm thời gian hồi chiêu
        if (attackCountdown > 0)
            attackCountdown -= Time.deltaTime;

        if (parentBarrack == null) return;

        FindTargetInBarrackRange();

        if (target != null)
        {
            // --- CÓ ĐỊCH: TẤN CÔNG ---
            MoveAndAttack();
        }
        else
        {
            // --- HẾT ĐỊCH: VỀ NHÀ ---
            ReturnToSpawnPoint();
        }

        // Xử lý hiển thị (đè lên nhau)
        if (sr != null)
            sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }

    void MoveAndAttack()
    {
        float dist = Vector2.Distance(transform.position, target.position);

        // Nếu chưa tới tầm đánh -> Di chuyển lại gần
        if (dist > attackRange)
        {
            MoveTowards(target.position);
            anim.SetBool("isAttacking", false);
        }
        // Nếu đã trong tầm đánh -> Đứng lại và Chém
        else
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", true);
            Flip(target.position.x);

            // Logic gây sát thương (Giống ArrowTower bắn tên)
            if (attackCountdown <= 0f)
            {
                DealDamage();
                attackCountdown = 1f / attackRate;
            }
        }
    }

    void DealDamage()
    {
        // Kiểm tra lại nếu target vẫn còn tồn tại
        if (target != null)
        {
            Debug.Log($"Knight chém {target.name} - {damage} sát thương!");

            // Gửi sát thương sang script máu của Enemy (Ví dụ: EnemyHealth)
            // Cách 1: Dùng SendMessage (Dễ nhưng chậm)
            target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);

            // Cách 2: Gọi trực tiếp (Khuyên dùng nếu bạn có script Enemy)
            /*
            var enemyHealth = target.GetComponent<Enemy>();
            if (enemyHealth != null) enemyHealth.TakeDamage(damage);
            */
        }
    }

    void ReturnToSpawnPoint()
    {
        float distToSpawn = Vector2.Distance(transform.position, spawnPosition);

        // Nếu chưa về đến nhà -> Đi tiếp
        if (distToSpawn > 0.1f)
        {
            MoveTowards(spawnPosition);
            anim.SetBool("isAttacking", false);
        }
        // Đã về đến nhà -> Đứng im (Idle)
        else
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", false);
        }
    }

    void MoveTowards(Vector3 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        anim.SetBool("isMoving", true);
        Flip(destination.x);
    }

    void Flip(float targetX)
    {
        if (targetX > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else if (targetX < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void FindTargetInBarrackRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            // Kiểm tra khoảng cách từ ĐỊCH đến NHÀ LÍNH (Barrack)
            float distToBarrack = Vector2.Distance(parentBarrack.transform.position, enemy.transform.position);

            // Chỉ đánh nếu địch nằm trong vùng bảo vệ của nhà lính
            if (distToBarrack <= parentBarrack.data.range)
            {
                float distToKnight = Vector2.Distance(transform.position, enemy.transform.position);
                if (distToKnight < shortestDistance)
                {
                    shortestDistance = distToKnight;
                    nearestEnemy = enemy;
                }
            }
        }
        target = (nearestEnemy != null) ? nearestEnemy.transform : null;
    }
}
