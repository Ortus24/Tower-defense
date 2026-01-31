using System.Collections;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [Header("Dữ liệu tháp")]
    public TowerData data;

    [Header("Căn chỉnh tâm")]
    // Biến này để chỉnh tâm vòng tròn cho khớp với hình ảnh tòa nhà
    [SerializeField] protected Vector2 centerOffset;

    protected float currentHP;
    protected bool isBuilt = true;
    protected Transform target;

    // Hàm lấy tâm đã cộng thêm Offset (Dùng cho cả việc bắn và vẽ Gizmos)
    public Vector3 GetTowerCenter()
    {
        return transform.position + (Vector3)centerOffset;
    }

    void Start()
    {
        if (data != null) currentHP = data.maxHP;
        StartCoroutine(BuildRoutine());
    }

    IEnumerator BuildRoutine()
    {
        // Chỗ này có thể thêm hiệu ứng bụi bay
        if (data != null) yield return new WaitForSeconds(data.buildTime);
        isBuilt = true;
        OnBuildComplete();
    }

    protected abstract void OnBuildComplete();

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

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0) Destroy(gameObject);
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
