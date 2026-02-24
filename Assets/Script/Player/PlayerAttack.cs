using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Effect")]
    [SerializeField] private GameObject slashEffectPrefab;
    [SerializeField] private float effectOffset = 0.5f;

    private float lastAttackTime;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        Attack();
    }

    void Attack()
    {
        float facingDir = transform.localScale.x > 0 ? 1f : -1f;

        Vector2 attackPos = (Vector2)transform.position
                            + Vector2.right * facingDir * effectOffset;

        if (slashEffectPrefab != null)
        {
            GameObject effect = Instantiate(
                slashEffectPrefab,
                attackPos,
                Quaternion.identity
            );

            Vector3 scale = effect.transform.localScale;
            scale.x *= facingDir;
            effect.transform.localScale = scale;

            Destroy(effect, 0.5f);
        }
    }

    void OnDrawGizmosSelected()
    {
        float facingDir = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 pos = (Vector2)transform.position
                      + Vector2.right * facingDir * effectOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, attackRange);
    }
}