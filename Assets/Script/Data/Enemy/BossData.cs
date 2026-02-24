using UnityEngine;
[CreateAssetMenu(fileName = "BossData", menuName = "Enemy/Create Boss Data")]
public class BossData : ScriptableObject
{
    public string bossName;
    public float baseHP = 500f;
    public float hpScalePerWave = 50f; // Mỗi 10 wave tăng 50 HP
    public float moveSpeed = 1f;
    public float damage = 30f;

    [Header("Special Abilities")]
    public bool canSummonFog = true;
    public bool canSpawnMinions = true;
    public float abilityInterval = 10f; // Cooldown giữa các abilities

    [Header("Minion Settings")]
    public EnemyData minionType;
    public int minionsPerSpawn = 3;

    [Header("Rewards")]
    public int goldReward = 100;
    public int goldMultiplier = 2;

    public GameObject bossPrefab;
    public float GetHPForWave(int wave)
    {
        int bossIndex = wave / 10;
        return baseHP + (hpScalePerWave * bossIndex);
    }
}