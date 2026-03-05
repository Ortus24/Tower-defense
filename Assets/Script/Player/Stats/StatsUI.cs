using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public GameObject[] statsSlots;

    public void Start()
    {
        UpdateDamage();
        UpdateHealth();
        UpdateMana();
        UpdateSpeed();
    }
    
    public void UpdateHealth()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Health: " + StatesManager.Instance.maxHp;
    }

    public void UpdateMana()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Mana: " + StatesManager.Instance.maxMana ;
    }

    public void UpdateDamage()
    {
        statsSlots[2].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatesManager.Instance.damage;
    }

    

    public void UpdateSpeed()
    {
        statsSlots[3].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatesManager.Instance.speed ;
    }


}
