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
}

