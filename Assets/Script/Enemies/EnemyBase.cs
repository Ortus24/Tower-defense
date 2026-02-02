using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyBase : MonoBehaviour
{
    [Header("Data")]
    public EnemyData data;
    public System.Action OnDeath;
    private float currentHP;
    private Transform target;

    void Start()
    {
        currentHP = data.maxHP;
        FindTarget();
        
        // Debug logging
        if (target != null)
        {
            Debug.Log($"{data.enemyName} spawned! Target: {target.name} at {target.position}");
        }
        else
        {
            Debug.LogError($"{data.enemyName} spawned but NO TARGET FOUND for type: {data.targetType}!");
        }
    }

    void Update()
    {
        if (target == null)
        {
            // Try to find target again if lost
            FindTarget();
            return;
        }
        MoveToTarget();
    }

    void FindTarget()
    {
        switch (data.targetType)
        {
            case EnemyTargetType.TheKeep:
                target = GameObject.FindWithTag("TheKeep")?.transform;
                if (target == null) Debug.LogWarning($"{data.enemyName}: No GameObject with tag 'TheKeep' found!");
                break;

            case EnemyTargetType.Mines:
                target = FindClosestWithTag("Mine");
                if (target == null)
                {
                    Debug.LogWarning($"{data.enemyName}: No Mine found, targeting TheKeep instead!");
                    target = GameObject.FindWithTag("TheKeep")?.transform;
                }
                break;

            case EnemyTargetType.Towers:
                target = FindClosestWithTag("Tower");
                if (target == null)
                {
                    Debug.LogWarning($"{data.enemyName}: No Tower found, targeting Player instead!");
                    target = GameObject.FindWithTag("Player")?.transform;
                }
                break;

            case EnemyTargetType.Hero:
                target = GameObject.FindWithTag("Player")?.transform;
                if (target == null) Debug.LogWarning($"{data.enemyName}: No GameObject with tag 'Player' found!");
                break;

            case EnemyTargetType.Sweep:
                target = FindClosestWithTag("EnemyTarget");
                if (target == null)
                {
                    Debug.LogWarning($"{data.enemyName}: No EnemyTarget found, targeting Player instead!");
                    target = GameObject.FindWithTag("Player")?.transform;
                }
                break;
        }
    }

    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            data.moveSpeed * Time.deltaTime
        );
    }

    Transform FindClosestWithTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        float minDist = Mathf.Infinity;
        Transform closest = null;

        foreach (var obj in objs)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = obj.transform;
            }
        }
        return closest;
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{data.enemyName} died!");
        OnDeath?.Invoke();  // Trigger event
        Destroy(gameObject);
    }
}
