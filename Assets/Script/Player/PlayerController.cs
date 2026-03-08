using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Action<int, int> OnManaChanged;
    public Action<int, int> OnHPChanged;

    private int maxHp;
    private int currentHp;

    private int manaMax;
    private int currentMana;

    [Header("Hiệu ứng UI")]
    public GameObject damagePopupPrefab;
    public static PlayerController Instance;
    private void Start()
    {
        manaMax = StatesManager.Instance.maxMana;
        currentMana = manaMax;
        maxHp = StatesManager.Instance.maxHp;
        currentHp = maxHp;
        OnHPChanged?.Invoke(currentHp, maxHp);
    }

    private void Awake()
    {
        // Đảm bảo Instance được gán ngay khi game chạy
        if (Instance == null) Instance = this;
    }
    public void UpdateManaMax(int newManaMax)
    {
        int difference = newManaMax - manaMax;
        manaMax = newManaMax;
        currentMana += difference;
        currentMana = Mathf.Clamp(currentMana, 0, manaMax);
        OnManaChanged?.Invoke(currentMana, manaMax);
    }

    public void UpdateMaxHp(int newMaxHp)
    {
        int difference = newMaxHp - maxHp;
        maxHp = newMaxHp;
        currentHp += difference; 
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        OnHPChanged?.Invoke(currentHp, maxHp);
    }

    public void TakeMana(int amount)
    {
        currentMana -= amount;
        currentMana = Mathf.Clamp(currentMana, 0, manaMax);
        Debug.Log($"Player uses {amount} mana. Current Mana: {currentMana}");
        OnManaChanged?.Invoke(currentMana, manaMax);
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

    public void RestoreMana(int amount)
    {
        // Kiểm tra nếu Mana đã đầy thì không thực hiện
        if (currentMana >= manaMax) return;

        currentMana += amount;
        // Đảm bảo mana không vượt quá giới hạn tối đa
        currentMana = Mathf.Clamp(currentMana, 0, manaMax);

        Debug.Log($"Player restored {amount} mana. Current Mana: {currentMana}");

        // Gọi Action để các thanh Mana Bar trên UI cập nhật theo
        OnManaChanged?.Invoke(currentMana, manaMax);

        // Hiển thị Popup màu xanh dương cho Mana (tương tự như Heal)
        if (damagePopupPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, 0.5f, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

            // Giả sử setup có logic phân biệt màu sắc, bạn có thể truyền màu xanh dương
            popup.GetComponent<Assets.Script.TowerBuilding.EconomyTower.DamagePopup>().Setup(amount, false);
        }
    }
    private void Die()
    {
        GameManager.Instance.ShowGameOver();
        Time.timeScale = 0f; // Tạm dừng game để
        // Gọi hàm LoadMainScene sau 5 giây

        SceneManager.LoadScene("MainScrene");
        Time.timeScale = 1f;
    }

    void LoadMainScene()
    {

    }
    public int GetCurrentHp() => currentHp;
    public int GetMaxHp() => maxHp;

    public int GetCurrentMana() => currentMana;
    public int GetMaxMana() => manaMax;
}