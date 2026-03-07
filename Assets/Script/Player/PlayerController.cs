using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action<int, int> OnHPChanged;

    private int maxHp;
    private int currentHp;
    [Header("Hiệu ứng UI")]
    public GameObject damagePopupPrefab;
    public static PlayerController Instance;
    private void Start()
    {

        maxHp = StatesManager.Instance.maxHp;
        currentHp = maxHp;
        OnHPChanged?.Invoke(currentHp, maxHp);
    }

    private void Awake()
    {
        // Đảm bảo Instance được gán ngay khi game chạy
        if (Instance == null) Instance = this;
    }

    public void UpdateMaxHp(int newMaxHp)
    {
        int difference = newMaxHp - maxHp;
        maxHp = newMaxHp;
        currentHp += difference; 
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        OnHPChanged?.Invoke(currentHp, maxHp);
    }

    public void TakeDamage(int damage)
    {

        Debug.Log($"Player takes {damage} damage in HP = {currentHp}" );

        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        OnHPChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Die();
        }
        // --- GỌI HIỆN SỐ SÁT THƯƠNG ---
        if (damagePopupPrefab != null)
        {
            // Cho vị trí xuất hiện cao lên một chút so với chân quái vật
            Vector3 spawnPos = transform.position + new Vector3(0, 0.5f, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

            // Gọi hàm Setup với isDamage = true
            popup.GetComponent<Assets.Script.TowerBuilding.EconomyTower.DamagePopup>().Setup((int)damage, true);
        }
    }

    public void Heal(int amount)
    {
        if (currentHp >= maxHp) return;

        currentHp += amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        Debug.Log($"Player healed {amount}. Current HP: {currentHp}");
        OnHPChanged?.Invoke(currentHp, maxHp);

        // Hiển thị Popup màu xanh lá cho hồi máu (tùy chọn)
        if (damagePopupPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, 0.5f, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);
            // Giả sử setup có tham số màu sắc hoặc isDamage
            popup.GetComponent<Assets.Script.TowerBuilding.EconomyTower.DamagePopup>().Setup(amount, false);
        }
    }
    private void Die()
    {
        Time.timeScale = 0f; // Đóng băng game
        GameManager.Instance.ShowGameOver();
    }

    public int GetCurrentHp() => currentHp;
    public int GetMaxHp() => maxHp;
}