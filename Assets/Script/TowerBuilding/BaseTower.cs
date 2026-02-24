using Assets.Script.TowerBuilding;
using Assets.Script.TowerBuilding.EconomyTower;
using Assets.Script.TowerBuilding.UIBuildMenu;
using System.Collections;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [Header("Dữ liệu tháp")]
    public TowerData data;

    [Header("--- HỆ THỐNG MÁU (MỚI) ---")]
    public TowerHealthBarUI healthBarScript; // Kéo Prefab thanh máu đã kéo vào tháp vào đây
    protected float currentHP; // Máu hiện tại của tháp

    [Header("Căn chỉnh tâm")]
    // Biến này để chỉnh tâm vòng tròn cho khớp với hình ảnh tòa nhà
    [SerializeField] protected Vector2 centerOffset;

    // --- THÊM DÒNG NÀY ĐỂ NHỚ CÁI MỎ ---
    [HideInInspector] public ResourceSpot occupiedSpot;

    protected bool isBuilt = true;
    protected Transform target;

    // Hàm lấy tâm đã cộng thêm Offset (Dùng cho cả việc bắn và vẽ Gizmos)
    public Vector3 GetTowerCenter()
    {
        return transform.position + (Vector3)centerOffset;
    }

    protected virtual void Start()
    {
        if (data != null)
        {
            currentHP = data.maxHP;
            // 1. Theo dõi xem lúc mới sinh ra tháp có nhận được 100 máu không?
            Debug.Log($"[Khởi tạo] Tháp {gameObject.name} đã nhận data. Max HP = {currentHP}");
        }
        else
        {
            // Cảnh báo màu vàng nếu bị lỗi mất kết nối Data
            Debug.LogWarning($"[LỖI] Tháp {gameObject.name} KHÔNG CÓ DATA! Máu mặc định = 0");
        }

        if (healthBarScript != null && data != null)
        {
            healthBarScript.Setup(currentHP, data.maxHP);
        }
    }

    protected void FindNearestTarget()
    {
        // Lấy tất cả kẻ địch
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies == null || enemies.Length == 0)
        {
            target = null;
            return;
        }

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        // Lấy tâm tháp chuẩn (đã chỉnh offset)
        Vector3 towerCenter = GetTowerCenter();

        foreach (GameObject enemy in enemies)
        {
            // --- LOGIC HÌNH TRÒN ---
            // Tính khoảng cách đường thẳng từ Tâm Tháp đến Kẻ Địch
            float distance = Vector2.Distance(towerCenter, enemy.transform.position);

            // So sánh khoảng cách với Range trong Data
            if (distance <= data.range && distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        // Cập nhật mục tiêu
        target = (nearestEnemy != null) ? nearestEnemy.transform : null;
    }

    // --- [MỚI] HÀM NHẬN SÁT THƯƠNG ---
    public void TakeDamage(float amount)
    {
        if (data == null) return;

        // 2. Theo dõi con số thực tế đang diễn ra khi bạn bấm nút
        Debug.Log($"[Bị đánh] Tháp {gameObject.name} đang có {currentHP} máu. Bị trừ {amount} máu!");

        currentHP -= amount;

        if (healthBarScript != null)
        {
            healthBarScript.UpdateHealthUI(currentHP, data.maxHP);
        }

        if (currentHP <= 0)
        {
            Debug.Log($"[Chết] Máu còn {currentHP} (<=0) nên gọi hàm Die()!");
            Die();
        }
    }

    // --- [MỚI] HÀM CHẾT (Bị phá hủy) ---
    protected virtual void Die()
    {
        // TODO: Gọi hiệu ứng khói lửa / âm thanh tháp sập ở đây
        Debug.Log(gameObject.name + " đã bị phá hủy!");


        // TODO: (Tùy chọn) Gọi GridManager để giải phóng ô đất này cho phép xây lại
        // 1. GIẢI PHÓNG Ô ĐẤT
        if (GridManager.main != null && GridManager.main.GetLevelGrid() != null && data != null)
        {
            int gridX, gridY;
            float cellSize = GridManager.main.cellSize;

            // 1. Tính toán độ lệch từ Tâm tháp về ô Góc dưới cùng bên trái
            // Ví dụ: Size 2x2 -> Lùi X đi 0.5 ô, lùi Y đi 0.5 ô
            float offsetX = (data.towerSize.x - 1) * 0.5f * cellSize;
            float offsetY = (data.towerSize.y - 1) * 0.5f * cellSize;

            Vector3 bottomLeftWorldPos = transform.position - new Vector3(offsetX, offsetY, 0);

            // 2. Lấy tọa độ lưới (X, Y) dựa trên điểm chuẩn vừa tìm được
            GridManager.main.GetLevelGrid().GetXY(bottomLeftWorldPos, out gridX, out gridY);

            // 3. Tiến hành giải phóng đúng kích thước tháp
            GridManager.main.FreeArea(new Vector2Int(gridX, gridY), data.towerSize);

            Debug.Log($"Đã giải phóng đất {data.towerSize.x}x{data.towerSize.y} tại ({gridX}, {gridY})");
        }

        if (occupiedSpot != null)
        {
            occupiedSpot.Restore();
            Debug.Log("Đã trả lại mỏ tài nguyên!");
        }

        Destroy(gameObject);
    }

    // Vẽ Gizmos hỗ trợ căn chỉnh trong Editor
    private void OnDrawGizmosSelected()
    {
        if (data != null)
        {
            // 1. Vẽ vòng tròn Range (Màu xanh)
            Gizmos.color = Color.cyan;
            Vector3 center = GetTowerCenter();
            Gizmos.DrawWireSphere(center, data.range);

            // 2. Vẽ tâm (Màu đỏ) để dễ căn chỉnh Offset
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(center, 0.2f);
        }
    }
}
