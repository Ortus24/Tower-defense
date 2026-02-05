using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class BossEnemy : MonoBehaviour
{
    [Header("Boss Data")]
    public BossData bossData;
    public int waveNumber; // Wave number mà boss spawn
    private float currentHP;
    private Transform target;
    private float nextAbilityTime;
    void Start()
    {
        currentHP = bossData.GetHPForWave(waveNumber);
        target = GameObject.FindWithTag("TheKeep")?.transform;
        nextAbilityTime = Time.time + bossData.abilityInterval;

        Debug.Log($"Boss spawned with {currentHP} HP (Wave {waveNumber})");
    }
    void Update()
    {
        if (target == null) return;
        MoveToTarget();
        CheckAbilities();
    }
    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            bossData.moveSpeed * Time.deltaTime
        );
    }
    void CheckAbilities()
    {
        if (Time.time >= nextAbilityTime)
        {
            UseRandomAbility();
            nextAbilityTime = Time.time + bossData.abilityInterval;
        }
    }
    void UseRandomAbility()
    {
        float random = Random.value;
        if (random < 0.5f && bossData.canSummonFog)
        {
            SummonFog();
        }
        else if (bossData.canSpawnMinions)
        {
            SpawnMinions();
        }
    }
    void SummonFog()
    {
        Debug.Log($"{bossData.bossName} summons fog!");
        // TODO: Implement fog effect
        // - Tạo fog particle effect
        // - Slow player movement
        // - Reduce visibility
    }
    void SpawnMinions()
    {
        Debug.Log($"{bossData.bossName} spawns {bossData.minionsPerSpawn} minions!");

        for (int i = 0; i < bossData.minionsPerSpawn; i++)
        {
            if (bossData.minionType?.enemyPrefab != null)
            {
                Vector3 spawnPos = transform.position +
                    (Vector3)(Random.insideUnitCircle * 2f);

                Instantiate(
                    bossData.minionType.enemyPrefab,
                    spawnPos,
                    Quaternion.identity
                );
            }
        }
    }
    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        Debug.Log($"Boss HP: {currentHP}/{bossData.GetHPForWave(waveNumber)}");

        if (currentHP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log($"Boss defeated! Reward: {bossData.goldReward * bossData.goldMultiplier} gold");

        // TODO: Implement reward system
        // GoldManager.Instance?.AddGold(bossData.goldReward * bossData.goldMultiplier);

        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Boss attack logic
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("TheKeep"))
        {
            // Deal damage to target
            Debug.Log($"Boss deals {bossData.damage} damage!");
        }
    }
}