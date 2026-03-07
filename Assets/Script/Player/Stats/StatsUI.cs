using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public GameObject[] statsSlots;

    public void Start()
    {
        UpdateHealth();
        HealthHp();
        UpdateMana();
        UpdateHoiMana();
        UpdateDamage();
        UpdateAttackSpeed();
        UpdateSpeed();
        UpdateNe();
        UpdateCriticalChance();
        statsSlots[9].GetComponentInChildren<TMP_Text>().text = "???????????";
    }
    
    public void UpdateHealth()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Máu: " + StatesManager.Instance.maxHp;
    }

    public void HealthHp()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Hồi máu : " + StatesManager.Instance.hoiMau;
    }

    public void UpdateMana()
    {
        statsSlots[2].GetComponentInChildren<TMP_Text>().text = "Năng lượng: " + StatesManager.Instance.maxMana;
    }

    public void UpdateHoiMana()
    {
        statsSlots[3].GetComponentInChildren<TMP_Text>().text = "Hồi mana: " + StatesManager.Instance.hoiMana;
    }
     public void UpdateDamage()
    {
        statsSlots[4].GetComponentInChildren<TMP_Text>().text = "Sát thương: " + StatesManager.Instance.damage;
    }
     public void UpdateAttackSpeed()
    {
        statsSlots[5].GetComponentInChildren<TMP_Text>().text = "Tốc đánh: " + StatesManager.Instance.attackSpeed;
    }
     public void UpdateSpeed()
    {
        statsSlots[6].GetComponentInChildren<TMP_Text>().text = "Tốc chạy: " + StatesManager.Instance.speed;    
    }
     public void UpdateNe()
    {
        statsSlots[7].GetComponentInChildren<TMP_Text>().text = "Né: " + StatesManager.Instance.ne;
    }
     public void UpdateCriticalChance()
    {
        statsSlots[8].GetComponentInChildren<TMP_Text>().text = "Chí mạng: " + StatesManager.Instance.criticalChance;
    }
}
