using System.Collections;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    public TowerData data;
    protected float currentHP;
    protected bool isBuilt = true;
    protected Transform target;

    void Start()
    {
        currentHP = data.maxHP;
        StartCoroutine(BuildRoutine());
    }

    IEnumerator BuildRoutine()
    {
        // Chỗ này có thể thêm hiệu ứng bụi bay từ Tiny Swords
        yield return new WaitForSeconds(data.buildTime);
        isBuilt = true;
        OnBuildComplete();
    }

    protected abstract void OnBuildComplete();
    protected void FindNearestTarget()
    {
        GameObject[] enemies;
        try
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
        }
        catch (UnityException)
        {
            return;
        }

        if (enemies == null || enemies.Length == 0)
        {
            target = null;
            return;
        }

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            // Kiểm tra xem quái vật có trong tầm bắn không
            if (distance <= data.range && distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        // Quan trọng: Nếu không có ai trong tầm bắn, nearestEnemy sẽ là null
        // và target sẽ được gán về null, giúp Animator chuyển về Idle
        target = (nearestEnemy != null) ? nearestEnemy.transform : null;
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0) Destroy(gameObject);
    }
}
