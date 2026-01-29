using UnityEngine;

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
}
