using UnityEngine;

public class KnightAI : MonoBehaviour
{
    [Header("Cài đặt chung")]
    public BarrackTower parentBarrack;
    public float moveSpeed = 2f;
    public float attackRange = 0.2f; 

    [Header("Cài đặt chiến đấu")]
    public float damage = 10f;
    public float attackRate = 1f;
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

        // Xử lý hiển thị (đè lên nhau dựa theo chiều cao Y)
        if (sr != null)
            sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }

    void MoveAndAttack()
    {
        // 1. Tính khoảng cách thực tế (Đường chéo)
        float dist = Vector2.Distance(transform.position, target.position);

        // 2. Tính độ lệch theo trục Y (Xem đang đứng lệch bao nhiêu)
        float yDifference = Mathf.Abs(transform.position.y - target.position.y);

        // 3. Tính hướng để gửi vào Animator
        Vector2 direction = (target.position - transform.position).normalized;
        UpdateAnimationDirection(direction);

        // --- LOGIC MỚI: CƠ CHẾ "LANE MAGNET" (NAM CHÂM HÚT VÀO LÀN) ---

        // Điều kiện đánh khắt khe hơn:
        // - Phải đủ gần (dist <= attackRange)
        // - VÀ Phải thẳng hàng (yDifference <= 0.1f)
        bool isAlignedY = yDifference <= 0.1f;

        if (dist > attackRange || !isAlignedY)
        {
            // Nếu chưa thẳng hàng, ta sẽ "ép" lính đi về phía Y của quái trước
            Vector3 moveTarget = target.position;

            // Nếu đã đến khá gần (theo trục X) nhưng vẫn bị lệch Y -> Chỉ di chuyển Y
            if (Mathf.Abs(transform.position.x - target.position.x) <= attackRange)
            {
                // Giữ nguyên X hiện tại, chỉ thay đổi Y để trượt lên/xuống cho thẳng
                moveTarget = new Vector3(transform.position.x, target.position.y, transform.position.z);
            }

            transform.position = Vector2.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);

            anim.SetBool("isMoving", true);
            anim.SetBool("isAttacking", false);
        }
        else
        {
            // Đã thẳng hàng & Đủ gần -> TẤN CÔNG
            anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", true);

            if (attackCountdown <= 0f)
            {
                DealDamage();
                attackCountdown = 1f / attackRate;
            }
        }
    }

    void DealDamage()
    {
        if (target != null)
        {
            // Gửi sát thương sang script máu của Enemy
            target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    void ReturnToSpawnPoint()
    {
        float distToSpawn = Vector2.Distance(transform.position, spawnPosition);

        // Nếu chưa về đến nhà -> Đi tiếp
        if (distToSpawn > 0.1f)
        {
            // Tính hướng về nhà để quay mặt cho đúng
            Vector2 direction = (spawnPosition - transform.position).normalized;
            UpdateAnimationDirection(direction);

            transform.position = Vector2.MoveTowards(transform.position, spawnPosition, moveSpeed * Time.deltaTime);
            anim.SetBool("isMoving", true);
            anim.SetBool("isAttacking", false);
        }
        // Đã về đến nhà -> Đứng im (Idle)
        else
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", false);
        }
    }

    void FindTargetInBarrackRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            // QUAN TRỌNG: Dùng GetTowerCenter() để lấy tâm chuẩn (đã chỉnh Offset)
            float distToBarrack = Vector2.Distance(parentBarrack.GetTowerCenter(), enemy.transform.position);

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

    // Hàm này thay thế hoàn toàn hàm Flip cũ
    void UpdateAnimationDirection(Vector2 dir)
    {
        // Chỉ cập nhật nếu có hướng di chuyển rõ ràng
        if (dir.magnitude > 0.1f)
        {
            // Gửi thông số vào Blend Tree
            // InputX dùng Abs vì ta tái sử dụng Animation bên Phải cho bên Trái
            anim.SetFloat("InputX", Mathf.Abs(dir.x));
            anim.SetFloat("InputY", dir.y);

            // Xử lý lật mặt thủ công (Scale)
            if (dir.x < 0) transform.localScale = new Vector3(-1, 1, 1); // Quay trái
            else transform.localScale = new Vector3(1, 1, 1);  // Quay phải
        }
    }

    // Vẽ Gizmos để debug
    private void OnDrawGizmosSelected()
    {
        // Vòng tròn đỏ thể hiện tầm kiếm
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Đường nối về vị trí gác
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, spawnPosition);
    }
}
