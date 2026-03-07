using UnityEngine;

public class StatesManager : MonoBehaviour
{
    public static StatesManager Instance;

    [Header("Hp")]
    public int maxHp;

    [Header("Hồi máu")]
    public int hoiMau;

    [Header("Mana")]
    public int maxMana;

    [Header("Hồi mana")]
    public int hoiMana;

    [Header("Damage")]
    public int damage;

    [Header("Tốc đánh")]
    public float attackSpeed;

    [Header("Speed")]
    public float speed;

    [Header("Né")]
    public float ne;

    [Header("Chí mạng")]
    public float criticalChance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Hàm này sẽ được gọi khi bạn ấn nút
    public void IncreaseHP(int add)
    {
        maxHp += add;

        // Cập nhật lại hiển thị máu trên UI
        StatsUI statsUI = FindObjectOfType<StatsUI>();
        if (statsUI != null)
        {
            statsUI.UpdateHealth();
        }
    }

    public void IncreaseHPHealth(int add)
    {
        hoiMau += add;
        // Cập nhật lại hiển thị hồi máu trên UI
        StatsUI statsUI = FindObjectOfType<StatsUI>();
        if (statsUI != null)
        {
            statsUI.HealthHp();
        }
    }

    public void IncreaseMana(int add)
    {
        maxMana += add;
        // Cập nhật lại hiển thị mana trên UI
        StatsUI statsUI = FindObjectOfType<StatsUI>();
        if (statsUI != null)
        {
            statsUI.UpdateMana();
        }
    }

    public void IncreaseHoiMana(int add)
    {
        hoiMana += add;
        // Cập nhật lại hiển thị hồi mana trên UI
        StatsUI statsUI = FindObjectOfType<StatsUI>();
        if (statsUI != null)
        {
            statsUI.UpdateHoiMana();
        }
    }
     public void IncreaseDamage(int add)
    {
        damage += add;
        // Cập nhật lại hiển thị sát thương trên UI
        StatsUI statsUI = FindObjectOfType<StatsUI>();
        if (statsUI != null)
        {
            statsUI.UpdateDamage();
        }
    }
     public void IncreaseAttackSpeed(float add)
    {
        attackSpeed += add;
        // Cập nhật lại hiển thị tốc đánh trên UI
        StatsUI statsUI = FindObjectOfType<StatsUI>();
        if (statsUI != null)
        {
            statsUI.UpdateAttackSpeed();
        }
    }
     public void IncreaseSpeed(float add)
    {
        speed += add;
        // Cập nhật lại hiển thị tốc chạy trên UI
        StatsUI statsUI = FindObjectOfType<StatsUI>();
        if (statsUI != null)
        {
            statsUI.UpdateSpeed();
        }
    }
     public void IncreaseNe(float add)
    {
        ne += add;
        // Cập nhật lại hiển thị né trên UI
        StatsUI statsUI = FindObjectOfType<StatsUI>();
        if (statsUI != null)
        {
            statsUI.UpdateNe();
        }
    }
     public void IncreaseCriticalChance(float add)
    {
        criticalChance += add;
        // Cập nhật lại hiển thị chí mạng trên UI
        StatsUI statsUI = FindObjectOfType<StatsUI>();
        if (statsUI != null)
        {
            statsUI.UpdateCriticalChance();
        }
    }
}

