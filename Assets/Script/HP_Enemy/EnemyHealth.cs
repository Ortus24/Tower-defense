using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public EnemyData enemyData;

    public float maxHP { get; private set; }
    public float CurrentHP { get; private set; }

    public GameObject itemDie;

    public event Action<float> OnHealthPercentChanged;
    public event Action OnDead;

    [SerializeField] private Animator animator;
    [SerializeField] private float destroyDelay = 0f;
    [Header("Hiệu ứng UI")]
    public GameObject damagePopupPrefab;

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

        // --- GỌI HIỆN SỐ SÁT THƯƠNG ---
        if (damagePopupPrefab != null)
        {
            // Cho vị trí xuất hiện cao lên một chút so với chân quái vật
            Vector3 spawnPos = transform.position + new Vector3(0, 0.5f, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

            // Gọi hàm Setup với isDamage = true
            popup.GetComponent<Assets.Script.TowerBuilding.EconomyTower.DamagePopup>().Setup((int)dmg, true);
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;


        if (StatesManager.Instance != null)
        {
            StatesManager.Instance.AddExp(1);
        }
        else
        {
            Debug.LogError("EnemyHealth: StatesManager.Instance is NULL!");
        }

        Destroy(gameObject, destroyDelay);
        animator?.SetTrigger("Die");
        OnDead?.Invoke();

        if (damagePopupPrefab != null)
        {
            // Cho vị trí xuất hiện cao lên một chút so với chân quái vật
            Vector3 spawnPos = transform.position + new Vector3(0, 0, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

            // Gọi hàm Setup với isDamage = true
            popup.GetComponent<Assets.Script.TowerBuilding.EconomyTower.DamagePopup>().SetUpExp((int)1, false);
        }

        if (itemDie != null)
        {
            Instantiate(itemDie, transform.position, Quaternion.identity);
        }
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
