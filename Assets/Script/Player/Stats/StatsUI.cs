using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public GameObject[] statsSlots;

    public void Start()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Máu: " + StatesManager.Instance.maxHp;
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Hồi máu : " + StatesManager.Instance.hoiMau;
        statsSlots[2].GetComponentInChildren<TMP_Text>().text = "Năng lượng: " + StatesManager.Instance.maxMana;
        statsSlots[3].GetComponentInChildren<TMP_Text>().text = "Hồi mana: " + StatesManager.Instance.hoiMana;
        statsSlots[4].GetComponentInChildren<TMP_Text>().text = "Sát thương: " + StatesManager.Instance.damage;
        statsSlots[5].GetComponentInChildren<TMP_Text>().text = "Tốc đánh: " + StatesManager.Instance.attackSpeed;
        statsSlots[6].GetComponentInChildren<TMP_Text>().text = "Tốc chạy: " + StatesManager.Instance.speed;    
        statsSlots[7].GetComponentInChildren<TMP_Text>().text = "Né: " + StatesManager.Instance.ne;
        statsSlots[8].GetComponentInChildren<TMP_Text>().text = "Chí mạng: " + StatesManager.Instance.criticalChance;
        statsSlots[9].GetComponentInChildren<TMP_Text>().text = "???????????";
    }
    
    public void UpdateHealth()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Health: " + StatesManager.Instance.maxHp;
    }

    public void HealthHp(int newHp)
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Health: " + newHp;
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
