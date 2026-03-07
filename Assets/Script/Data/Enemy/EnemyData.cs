using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyData",
    menuName = "Enemy/Create Enemy Data"
)]
public class EnemyData : ScriptableObject
{
    public string enemyName;

    public float maxHP;
    public float moveSpeed;
    public float damage;
    public float experienceReward;

    public EnemyTargetType targetType;

    public GameObject enemyPrefab;
}