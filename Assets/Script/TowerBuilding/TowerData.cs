using UnityEngine;

public enum ResourceType
{
    None,       // Tháp thường (Xây đất trống)
    GoldMine,   // Mỏ vàng (Phải xây đè lên cục vàng)
    Tree        // Nhà gỗ (Phải xây đè lên cái cây)
}

[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/Tower Data")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public int woodCost;
    public int goldCost;
    public float maxHP;
    public float damage;
    public float range;
    public float attackSpeed; // Phát bắn mỗi giây
    public float buildTime; // 3-5s
    public bool isAoE;
    public float splashRadius;
    public Vector2Int towerSize; // Kích thước chiếm dụng trên lưới


    // ========================================================================
    // HỆ THỐNG Xây DỰNG ECONOMY TOWER
    // ========================================================================
    [Header("Yêu cầu xây dựng")]
    public ResourceType resourceType = ResourceType.None; // Mặc định là None

    [Header("Kinh tế Gold")]
    public int goldPerSecond = 5;     // Mỗi giây sinh ra bao nhiêu
    public int maxGoldCapacity = 50;  // Sức chứa tối đa (đầy thì ngừng sinh)

    [Header("Kinh tế Wood")]
    public int woodPerSecond = 5;      // Tốc độ sản xuất gỗ
    public int maxWoodCapacity = 50;   // Sức chứa gỗ tối đa

    // ========================================================================
    // HỆ THỐNG NÂNG CẤP
    // ========================================================================
    [Header("Chỉ số hiển thị")]
    [TextArea(3, 5)]
    public string description;

    [Header("Hệ thống Nâng Cấp")]
    // 1. Dữ liệu của cấp tiếp theo (Để biết tốn bao nhiêu tiền, mạnh thế nào)
    public TowerData nextLevelData;

    // 2. Hình ảnh/Prefab của cấp tiếp theo (Để thay thế cái nhà cũ)
    public GameObject nextLevelPrefab;

    [Header("Cost Nâng cấp This Level")]
    public int goldCostUpgrade; // <--- Số vàng cần để xây hoặc nâng lên cấp này
    public int woodCostUpgrade; // <--- Số gỗ cần để xây hoặc nâng lên cấp này
}
