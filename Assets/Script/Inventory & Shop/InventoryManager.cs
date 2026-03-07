using Assets.Script.Inventory___Shop;
using System;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] itemSlots;

    [Header("Quản lý Đồ Quái Rớt (Huy hiệu/Pha lê)")]
    public int monsterDropTotal;
    public TMP_Text monsterDropText;
    public static InventoryManager instance; // Thêm dòng này

    private void Awake()
    {
        // Khởi tạo Singleton
        if (instance == null) instance = this;
    }
    public void UpdateResourceUI()
    {
        if (monsterDropText != null) monsterDropText.text = monsterDropTotal.ToString();
    }

    private void OnEnable()
    {
        Loot.OnItemLooted += AddItem;
    }

    private void OnDisable()
    {
        Loot.OnItemLooted -= AddItem;
    }

    private void Start()
    {
        foreach (InventorySlot slot in itemSlots)
        {
            slot.UpdateUI();
        }
    }

    public void AddItem(ItemSO newItem, int quantity)
    {
        // 1. CỘNG TIỀN TRƯỚC (Để không bị lệnh return phía dưới chặn mất)
        if (newItem != null && newItem.itemName.Equals("MonsterDrop", StringComparison.OrdinalIgnoreCase))
        {
            monsterDropTotal += quantity;
            UpdateResourceUI();
            Debug.Log("Đã cộng Monster Drop: " + quantity);
        }

        // Tạo một biến lưu số lượng vật phẩm đang cần thêm vào túi
        int amountToAdd = quantity;

        // 2. Tìm các ô ĐÃ CÓ sẵn vật phẩm này và CÒN CHỖ TRỐNG
        if (newItem.stackSize > 1)
        {
            foreach (InventorySlot slot in itemSlots)
            {
                if (slot.item == newItem && slot.quantity < newItem.stackSize)
                {
                    int spaceLeft = newItem.stackSize - slot.quantity;
                    int amountToAddNow = Mathf.Min(amountToAdd, spaceLeft);

                    slot.quantity += amountToAddNow;
                    amountToAdd -= amountToAddNow;
                    slot.UpdateUI();

                    if (amountToAdd <= 0) return;
                }
            }
        }

        // 3. Tìm ô TRỐNG hoàn toàn (Dùng cho món mới hoặc món không cho cộng dồn)
        foreach (InventorySlot slot in itemSlots)
        {
            if (slot.item == null)
            {
                int amountToAddNow = Mathf.Min(newItem.stackSize, amountToAdd);

                slot.item = newItem;
                slot.quantity = amountToAddNow;
                amountToAdd -= amountToAddNow;
                slot.UpdateUI();

                if (amountToAdd <= 0) return;
            }
        }

        if (amountToAdd > 0)
        {
            Debug.Log("Túi đồ đã đầy! Số lượng dư: " + amountToAdd);
        }
    }
    public void AddMonsterDrop(int amount)
    {
        monsterDropTotal += amount; // Đây là dòng cộng vào biến số thực tế
        UpdateResourceUI();        // Đây là dòng cập nhật con số lên màn hình
    }

    public void UseItem(InventorySlot slot)
    {
        if (slot.item == null) return;

        ItemSO itemToUse = slot.item;
        bool used = false;

        // 1. Logic hồi HP
        if (itemToUse.healAmount > 0)
        {
            // Kiểm tra nếu máu chưa đầy mới cho dùng
            if (PlayerController.Instance.GetCurrentHp() < PlayerController.Instance.GetMaxHp())
            {
                PlayerController.Instance.Heal(itemToUse.healAmount);
                used = true;
            }
            else
            {
                Debug.Log("Máu đã đầy, không cần sử dụng!");
                return; // Thoát hàm, không trừ vật phẩm
            }
        }

        // 2. Logic hồi MP (Nếu bạn có hệ thống Mana tương tự)
        if (itemToUse.manaAmount > 0)
        {
            // PlayerController.Instance.RestoreMana(itemToUse.manaAmount);
            used = true;
        }

        // 3. Trừ số lượng nếu đã sử dụng thành công
        if (used)
        {
            slot.quantity--;
            if (slot.quantity <= 0)
            {
                slot.item = null;
            }
            slot.UpdateUI();
        }
    }


}
