using UnityEngine;

public class SlashDamage : MonoBehaviour
{
    private float damage;
    private LayerMask enemyLayer;

    public void Setup(float damage, LayerMask layer)
    {
        this.damage = damage;
        this.enemyLayer = layer;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((enemyLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}