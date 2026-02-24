using UnityEngine;

public class SlashDamage : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}