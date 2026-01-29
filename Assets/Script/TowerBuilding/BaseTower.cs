using System.Collections;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    public TowerData data;
    protected float currentHP;
    protected bool isBuilt = false;
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= data.range)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        target = nearestEnemy != null ? nearestEnemy.transform : null;
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0) Destroy(gameObject);
    }
}
