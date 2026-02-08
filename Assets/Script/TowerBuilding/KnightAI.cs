using UnityEngine;

public class KnightAI : MonoBehaviour
{
    [Header("Cài đặt chung")]
    public float moveSpeed = 2f;
    public float attackRange = 0.5f; // Tầm đánh cận chiến
    public float attackRate = 1f;    // Tốc độ đánh (giây/phát)

    // Các biến này sẽ được ghi đè bởi Data từ Tháp
    private float damage;
    private float maxHP;
    private float currentHP;

    private BarrackTower parentBarrack;
    private Vector3 rallyPoint; // Điểm gác
    private Transform target;

    private float attackCountdown = 0f;
    private Animator anim;
    private SpriteRenderer sr;

    // --- HÀM KHỞI TẠO (GỌI TỪ BARRACK TOWER) ---
    public void SetupSoldier(BarrackTower barrack, float dmg, float hp, Vector3 spawnPos)
    {
        this.parentBarrack = barrack;
        this.damage = dmg;
        this.maxHP = hp;
        this.currentHP = hp;
        this.rallyPoint = spawnPos;

        // Đặt vị trí ban đầu
        transform.position = spawnPos;
    }
    // -------------------------------------------

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (attackCountdown > 0) attackCountdown -= Time.deltaTime;

        if (parentBarrack == null) return; // Nếu nhà lính bị phá, lính dừng hoạt động

        // 1. Tìm mục tiêu
        FindTarget();

        // 2. Hành động
        if (target != null)
        {
            // Có địch -> Chiến đấu
            MoveAndAttack(target.position);
        }
        else
        {
            // Hết địch -> Về nhà gác
            ReturnToRallyPoint();
        }

        // 3. Xử lý hiển thị (Sorting Order theo chiều cao Y)
        if (sr != null) sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }

    void FindTarget()
    {
        // Nếu đang đánh 1 con mà con đó chạy ra khỏi vùng bảo vệ của tháp -> Bỏ qua, quay về
        if (target != null)
        {
            if (!target.gameObject.activeSelf) target = null; // Quái chết
            else
            {
                // Kiểm tra khoảng cách từ QUÁI đến THÁP
                float distEnemyToTower = Vector2.Distance(target.position, parentBarrack.transform.position);
                if (distEnemyToTower > parentBarrack.data.range) target = null; // Quái chạy xa quá thì bỏ
            }
        }

        // Nếu chưa có mục tiêu, tìm con mới
        if (target == null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float shortestDist = Mathf.Infinity;
            GameObject nearest = null;

            foreach (GameObject enemy in enemies)
            {
                // Chỉ quan tâm quái nằm trong tầm bảo vệ của THÁP
                float distToTower = Vector2.Distance(enemy.transform.position, parentBarrack.transform.position);
                if (distToTower <= parentBarrack.data.range)
                {
                    // Chọn con gần LÍNH nhất trong số đó
                    float distToKnight = Vector2.Distance(transform.position, enemy.transform.position);
                    if (distToKnight < shortestDist)
                    {
                        shortestDist = distToKnight;
                        nearest = enemy;
                    }
                }
            }
            target = (nearest != null) ? nearest.transform : null;
        }
    }

    void MoveAndAttack(Vector3 desPos)
    {
        float dist = Vector2.Distance(transform.position, desPos);
        float yDiff = Mathf.Abs(transform.position.y - desPos.y);

        // Điều kiện đánh: Đủ gần VÀ Thẳng hàng Y (Lane Magnet)
        bool isAlignedY = yDiff <= 0.2f;

        if (dist > attackRange || !isAlignedY)
        {
            // DI CHUYỂN
            Vector3 moveTarget = desPos;

            // Kỹ thuật Lane Magnet: Nếu X đã gần, chỉ di chuyển Y để trượt vào hàng
            if (Mathf.Abs(transform.position.x - desPos.x) <= attackRange)
            {
                moveTarget = new Vector3(transform.position.x, desPos.y, transform.position.z);
            }

            transform.position = Vector2.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
            SetAnim(true, false, (moveTarget - transform.position).normalized);
        }
        else
        {
            // TẤN CÔNG
            SetAnim(false, true, (desPos - transform.position).normalized);

            if (attackCountdown <= 0f)
            {
                // Gửi sát thương
                if (target != null) target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                attackCountdown = 1f / attackRate;
            }
        }
    }

    void ReturnToRallyPoint()
    {
        float dist = Vector2.Distance(transform.position, rallyPoint);

        if (dist > 0.1f)
        {
            // Đi về điểm gác
            transform.position = Vector2.MoveTowards(transform.position, rallyPoint, moveSpeed * Time.deltaTime);
            SetAnim(true, false, (rallyPoint - transform.position).normalized);
        }
        else
        {
            // Đứng yên (Idle)
            SetAnim(false, false, Vector2.zero);
        }
    }

    void SetAnim(bool isMoving, bool isAttacking, Vector2 dir)
    {
        if (anim == null) return;

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isAttacking", isAttacking);

        if (dir.magnitude > 0.1f)
        {
            anim.SetFloat("InputX", Mathf.Abs(dir.x));
            anim.SetFloat("InputY", dir.y);

            // Lật mặt Sprite
            if (dir.x < 0) transform.localScale = new Vector3(-1, 1, 1);
            else transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Hàm nhận sát thương (nếu quái đánh lại lính)
    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Có thể thêm hiệu ứng nổ hoặc animation chết ở đây
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
