using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TheKeep"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject); 
        }
        else if (other.GetComponent<BaseTower>() != null)
        {
            BaseTower tower = other.GetComponent<BaseTower>();
            tower.TakeDamage(damage); // Gọi hàm trừ máu của tháp
            Destroy(gameObject);
        }
    }
}