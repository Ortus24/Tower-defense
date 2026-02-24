using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public EnemyData enemyData;

    public float maxHP { get; private set; }
    public float CurrentHP { get; private set; }

    public event Action<float> OnHealthPercentChanged;
    public event Action OnDead;

    [SerializeField] private Animator animator;
    [SerializeField] private float destroyDelay = 0f;

    private bool isDead;

    void Start()
    {
        maxHP = enemyData.maxHP;
        CurrentHP = maxHP;
    }

    void Awake()
    {
        CurrentHP = maxHP;
        if (!animator)
            animator = GetComponentInChildren<Animator>();

        Notify();
    }

    public void TakeDamage(float dmg)
    {
        if (isDead) return;
        animator?.SetTrigger("Hit");

        Debug.Log($"Enemy took {dmg} damage. Current HP: {CurrentHP - dmg}/{maxHP}");

        CurrentHP = Mathf.Clamp(CurrentHP - dmg, 0, maxHP);


        Notify();

        if (CurrentHP <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        Destroy(gameObject, destroyDelay);
        animator?.SetTrigger("Die");
        OnDead?.Invoke();
    }

    public void OnHitFrame()
    {
        Notify();
    }

    void Notify()
    {
        OnHealthPercentChanged?.Invoke(CurrentHP / maxHP);
    }
}
