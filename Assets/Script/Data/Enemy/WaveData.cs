using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveEnemyConfig
{
    public EnemyData enemyData;
    public int count;
}
[CreateAssetMenu(fileName = "WaveData", menuName = "Enemy/Create Wave Data")]
public class WaveData : ScriptableObject
{
    public int waveNumber;
    public List<WaveEnemyConfig> enemies = new List<WaveEnemyConfig>();
    public bool isBossWave;
    public float timeBetweenSpawns = 1f;
}