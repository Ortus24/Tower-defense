using UnityEngine;

public class WaveTestHelper : MonoBehaviour
{
    public WaveManager waveManager;
    
    [Header("Test Controls")]
    [Tooltip("Press this key to skip to next wave immediately")]
    public KeyCode skipWaveKey = KeyCode.N;
    
    [Tooltip("Press this key to jump to Wave 5")]
    public KeyCode jumpToWave5 = KeyCode.Alpha5;
    
    [Tooltip("Press this key to jump to Wave 10 (Boss)")]
    public KeyCode jumpToWave10 = KeyCode.Alpha0;
    
    [Tooltip("Press this key to kill all enemies")]
    public KeyCode killAllKey = KeyCode.K;

    void Update()
    {
        if (waveManager == null) return;

        // Skip to next wave
        if (Input.GetKeyDown(skipWaveKey))
        {
            KillAllEnemies();
            ForceNextWave();
            Debug.Log($"[TEST] Skipping to Wave {waveManager.currentWave}");
        }

        // Jump to Wave 5
        if (Input.GetKeyDown(jumpToWave5))
        {
            KillAllEnemies();
            waveManager.currentWave = 4; // Will become 5 after we call ForceNextWave
            ForceNextWave();
            Debug.Log($"[TEST] Jumping to Wave {waveManager.currentWave}!");
        }

        // Jump to Wave 10 (Boss Wave)
        if (Input.GetKeyDown(jumpToWave10))
        {
            KillAllEnemies();
            waveManager.currentWave = 9; // Will become 10
            ForceNextWave();
            Debug.Log($"[TEST] Jumping to Wave {waveManager.currentWave} (Boss Wave)!");
        }

        // Kill all enemies
        if (Input.GetKeyDown(killAllKey))
        {
            int killed = KillAllEnemies();
            Debug.Log($"[TEST] Killed {killed} enemies");
        }
    }

    void ForceNextWave()
    {
        // Cancel any pending StartNextWave invoke
        waveManager.CancelInvoke("StartNextWave");
        
        // Immediately trigger next wave
        waveManager.Invoke("StartNextWave", 0.1f);
    }

    int KillAllEnemies()
    {
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
        int count = enemies.Length;
        
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        // Also kill bosses
        BossEnemy[] bosses = FindObjectsOfType<BossEnemy>();
        count += bosses.Length;
        
        foreach (var boss in bosses)
        {
            Destroy(boss.gameObject);
        }

        return count;
    }

    void OnGUI()
    {
        // Display test controls on screen
        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = Color.yellow;
        
        GUI.Label(new Rect(10, 10, 400, 200), 
            $"=== WAVE TEST HELPER ===\n" +
            $"Current Wave: {waveManager.currentWave}\n" +
            $"Press [{skipWaveKey}] - Skip Wave\n" +
            $"Press [{jumpToWave5}] - Jump to Wave 5\n" +
            $"Press [{jumpToWave10}] - Jump to Wave 10 (Boss)\n" +
            $"Press [{killAllKey}] - Kill All Enemies", 
            style);
    }
}
