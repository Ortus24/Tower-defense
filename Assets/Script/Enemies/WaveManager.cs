using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WaveManager : MonoBehaviour
{
    [Header("Enemy Data References")]
    public EnemyData tntGoblin;
    public EnemyData torchGoblin;
    public EnemyData heavyOrc;
    public EnemyData shadowAssassin;
    public EnemyData skeleton;
    [Header("Wave Settings")]
    public int currentWave = 1;
    public float timeBetweenWaves = 30f;
    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public float spawnDelay = 1.5f;
    [Header("Events")]
    public UnityEvent<int> OnWaveStart;
    public UnityEvent<int> OnWaveComplete;
    private bool waveInProgress = false;
    private int enemiesAlive = 0;
    
    // --- Biến đếm ngược ---
    public float waveCountdown = 0f;
    public bool isWaitingForNextWave = false;

    [Header("Boss Settings")]
    public BossData bossData;
    public int bossWaveInterval = 10; // Spawn boss mỗi 10 waves
    void Start()
    {
        // Khởi động chờ 10s cho wave đầu tiên
        waveCountdown = timeBetweenWaves;
        isWaitingForNextWave = true;
    }

    void Update()
    {
        // Nếu đang trong thời gian đếm ngược
        if (isWaitingForNextWave)
        {
            waveCountdown -= Time.deltaTime;

            // In số giây chẵn còn lại ra Console
            // Debug.Log($"Wave mới sẽ xuất hiện sau: {(int)waveCountdown} giây");

            if (waveCountdown <= 0f)
            {
                isWaitingForNextWave = false;
                StartNextWave();
            }
        }
        // Kiểm tra xem wave đã hoàn thành chưa
        else if (waveInProgress && enemiesAlive <= 0)
        {
            CompleteWave();
        }
    }
    void StartNextWave()
    {
        waveInProgress = true;
        OnWaveStart?.Invoke(currentWave);

        Debug.Log($"Starting Wave {currentWave}");
        StartCoroutine(SpawnWave());
    }
    IEnumerator SpawnWave()
    {
        if (IsBossWave(currentWave))
        {
            SpawnBoss();
            yield break; // Chỉ spawn boss, không spawn enemies thường
        }

        List<EnemyData> enemiesToSpawn = GetEnemiesForWave(currentWave);
        int enemyCount = GetEnemyCountForWave(currentWave);
        enemiesAlive += enemyCount; // Cần cộng dồn để an toàn nếu wave chạy đè
        for (int i = 0; i < enemyCount; i++)
        {
            // Chọn random enemy từ list available
            EnemyData randomEnemy = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)];
            SpawnEnemy(randomEnemy);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
    List<EnemyData> GetEnemiesForWave(int wave)
    {
        List<EnemyData> availableEnemies = new List<EnemyData>();

        // ===== FOR TESTING: ALL ENEMIES FROM WAVE 1 =====
        // Comment this out to restore original wave progression
        availableEnemies.Add(tntGoblin);
        // availableEnemies.Add(torchGoblin);
        // availableEnemies.Add(skeleton);
        // availableEnemies.Add(heavyOrc);
        // availableEnemies.Add(shadowAssassin);
        return availableEnemies;

        /* ===== ORIGINAL WAVE SCALING (COMMENTED FOR TESTING) =====
        // Wave 1-5: Chỉ TNT Goblin
        if (wave <= 5)
        {
            availableEnemies.Add(tntGoblin);
        }
        // Wave 5-10: + Torch Goblin, Skeleton
        else if (wave <= 10)
        {
            availableEnemies.Add(tntGoblin);
            availableEnemies.Add(torchGoblin);
            availableEnemies.Add(skeleton);
        }
        // Wave 10-15: + Heavy Orc, Shadow Assassin
        else if (wave <= 15)
        {
            availableEnemies.Add(tntGoblin);
            availableEnemies.Add(torchGoblin);
            availableEnemies.Add(skeleton);
            availableEnemies.Add(heavyOrc);
            availableEnemies.Add(shadowAssassin);
        }
        // Wave 15+: All types
        else
        {
            availableEnemies.Add(tntGoblin);
            availableEnemies.Add(torchGoblin);
            availableEnemies.Add(skeleton);
            availableEnemies.Add(heavyOrc);
            availableEnemies.Add(shadowAssassin);
        }
        return availableEnemies;
        */
    }
    int GetEnemyCountForWave(int wave)
    {
        // Wave 1-5: 5-15 units
        if (wave <= 5)
        {
            return Mathf.RoundToInt(Mathf.Lerp(5, 15, (wave - 1) / 4f));
        }
        // Wave 5-10: 15-30 units
        else if (wave <= 10)
        {
            return Mathf.RoundToInt(Mathf.Lerp(15, 30, (wave - 5) / 5f));
        }
        // Wave 10-15: 30-50 units
        else if (wave <= 15)
        {
            return Mathf.RoundToInt(Mathf.Lerp(30, 50, (wave - 10) / 5f));
        }
        // Wave 15+: 50+ units (scale theo wave)
        else
        {
            return 50 + ((wave - 15) * 5);
        }
    }
    void SpawnEnemy(EnemyData data)
    {
        if (data == null || data.enemyPrefab == null)
        {
            Debug.LogError($"Enemy data or prefab is NULL!");
            enemiesAlive--; // Giảm count để tránh stuck
            return;
        }
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No Spawn Points assigned!");
            enemiesAlive--;
            return;
        }
        // Random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(
            data.enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );
        // Subscribe to enemy death
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDead += OnEnemyDied;
        }
    }
    void OnEnemyDied()
    {
        enemiesAlive--;
    }
    void CompleteWave()
    {
        waveInProgress = false;
        OnWaveComplete?.Invoke(currentWave);

        Debug.Log($"Wave {currentWave} Complete!");

        currentWave++;

        // Bắt đầu đếm ngược 10s cho wave tiếp theo
        waveCountdown = timeBetweenWaves;
        isWaitingForNextWave = true;
    }
    bool IsBossWave(int wave)
    {
        return wave % bossWaveInterval == 0;
    }
    void SpawnBoss()
    {
        if (bossData == null || bossData.bossPrefab == null)
        {
            Debug.LogError("Boss Data or Prefab is NULL!");
            return;
        }
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject bossObj = Instantiate(
            bossData.bossPrefab,
            spawnPoint.position,
            Quaternion.identity
        );
        BossEnemy boss = bossObj.GetComponent<BossEnemy>();
        if (boss != null)
        {
            boss.bossData = bossData;
            boss.waveNumber = currentWave;
        }
        enemiesAlive++; // Boss cũng count như enemy

        Debug.Log($"Boss spawned for Wave {currentWave}!");
    }
}