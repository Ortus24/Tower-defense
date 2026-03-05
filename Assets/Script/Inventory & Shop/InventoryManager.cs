using Assets.Script.Inventory___Shop;
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
        // Tạo một biến lưu số lượng vật phẩm đang cần thêm vào túi
        int amountToAdd = quantity;

        // 2. Tìm các ô ĐÃ CÓ sẵn vật phẩm này và CÒN CHỖ TRỐNG
        foreach (InventorySlot slot in itemSlots)
        {
            // Nếu vật phẩm trong ô giống với vật phẩm đang nhặt VÀ số lượng hiện tại chưa đạt mức Stack tối đa
            if (slot.item == newItem && slot.quantity < newItem.stackSize)
            {
                // Tính toán xem ô này còn chứa thêm được bao nhiêu cái nữa
                int spaceLeft = newItem.stackSize - slot.quantity;

                // Lấy số lượng nhỏ nhất giữa "số lượng cần thêm" và "chỗ trống còn lại"
                int amountToAddNow = Mathf.Min(amountToAdd, spaceLeft);

                // Cộng thêm vào slot và trừ đi số lượng còn phải thêm
                slot.quantity += amountToAddNow;
                amountToAdd -= amountToAddNow;

                // Cập nhật lại UI của slot đó
                slot.UpdateUI();

                // Nếu đã xếp xong toàn bộ số lượng nhặt được thì thoát hàm
                if (amountToAdd <= 0)
                {
                    return;
                }
            }
        }

        // 3. Nếu các ô chứa vật phẩm đó đã ĐẦY STACK, ta tìm một Ô TRỐNG hoàn toàn
        foreach (InventorySlot slot in itemSlots)
        {
            if (slot.item == null)
            {
                // Tính xem có thể nhét bao nhiêu vào ô trống (tối đa bằng stackSize của vật phẩm)
                int amountToAddNow = Mathf.Min(newItem.stackSize, amountToAdd);

                // Gán dữ liệu cho ô trống
                slot.item = newItem;
                slot.quantity = amountToAddNow;
                amountToAdd -= amountToAddNow;

                // Cập nhật lại UI
                slot.UpdateUI();

                // Thoát nếu đã xếp hết đồ
                if (amountToAdd <= 0)
                {
                    return;
                }
            }
        }

        // 4. TÚI ĐỒ ĐÃ ĐẦY HOÀN TOÀN
        if (amountToAdd > 0)
        {
            // Lúc này túi đồ không còn chỗ, lượng vật phẩm thừa (amountToAdd) sẽ rơi ngược ra ngoài map
            // DropLoot(newItem, amountToAdd); 
            Debug.Log("Túi đồ đã đầy! Số lượng dư: " + amountToAdd);
        }
    }
    public void AddMonsterDrop(int amount)
    {
        monsterDropTotal += amount; // Đây là dòng cộng vào biến số thực tế
        UpdateResourceUI();        // Đây là dòng cập nhật con số lên màn hình
    }
}
