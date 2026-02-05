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

    [Header("Yêu cầu xây dựng")]
    public ResourceType resourceType = ResourceType.None; // Mặc định là None
}
