using UnityEngine;

public class StatesManager : MonoBehaviour
{
    public static StatesManager Instance;

    [Header("Hp")]
    public int maxHp;

    [Header("Mana")]
    public int maxMana;

    [Header("Damage")]
    public int damage;

    [Header("Speed")]
    public float speed;

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

