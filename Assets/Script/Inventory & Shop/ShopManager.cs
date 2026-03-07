using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.TowerBuilding;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Inventory___Shop
{
    public enum PaymentType { PayWithGold, PayWithWood, PayWithMonsterDrop }
    public enum RewardType { ReceiveItem, ReceiveGold, ReceiveWood }
    public class ShopManager : MonoBehaviour
    {
        [Header("Cài đặt các Tab của Shop")]
        // Thay vì 1 List, giờ chúng ta có 1 mảng chứa các Tab (Mỗi tab chứa 1 List hàng riêng)
        [SerializeField] private List<ShopTab> shopTabs;

        [Header("Danh sách các ô UI")]
        [SerializeField] private ShopSlot[] shopSlots;

        // Biến nhớ xem người chơi đang mở tab số mấy (Mặc định là 0 - Tab đầu tiên)
        private int currentTabIndex = 0;

        [Header("Liên kết hệ thống")]
        // THÊM MỚI: Cần tham chiếu đến InventoryManager để kiểm tra tiền và thêm đồ
        [SerializeField] private InventoryManager inventoryManager;

        [Header("Icons Tiền Tệ (Hiển thị trên nút giá)")]
        public Sprite goldIcon;
        public Sprite woodIcon;
        public Sprite monsterDropIcon;

        public static ShopManager Instance;
        private void Awake()
        {
            // Đảm bảo Instance được gán ngay khi game chạy
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            // Mở Tab đầu tiên khi game bắt đầu
            OpenTab(0);
        }

        // Hàm này sẽ được gán vào các Nút bấm (Button)
        public void OpenTab(int tabIndex)
        {
            // Kiểm tra xem số thứ tự tab có hợp lệ không
            if (tabIndex >= 0 && tabIndex < shopTabs.Count)
            {
                currentTabIndex = tabIndex;
                PopulateShopItems(); // Cập nhật lại UI
            }
        }

        public void PopulateShopItems()
        {
            // Lấy danh sách mặt hàng của Tab hiện tại đang được chọn
            List<ShopItem> currentItems = shopTabs[currentTabIndex].items;

            // 1. Đổ dữ liệu vào các ô Slot
            for (int i = 0; i < currentItems.Count; i++)
            {
                if (i >= shopSlots.Length) break;

                ShopItem currentItem = currentItems[i];
                shopSlots[i].gameObject.SetActive(true);

                // Nạp dữ liệu vào UI
                shopSlots[i].Initialize(currentItem);
            }

            // 2. Tắt các ô Slot dư thừa (Nếu tab này có ít đồ hơn tab khác)
            for (int i = currentItems.Count; i < shopSlots.Length; i++)
            {
                shopSlots[i].gameObject.SetActive(false);
            }
        }

        public void PopulateShopItems(List<ShopItem> itemsToDisplay)
        {
            // Sử dụng danh sách itemsToDisplay được truyền vào thay vì lấy từ shopTabs
            for (int i = 0; i < itemsToDisplay.Count; i++)
            {
                if (i >= shopSlots.Length) break;

                ShopItem currentItem = itemsToDisplay[i];
                shopSlots[i].gameObject.SetActive(true);
                shopSlots[i].Initialize(currentItem);
            }

            // Tắt các ô trống dư thừa
            for (int i = itemsToDisplay.Count; i < shopSlots.Length; i++)
            {
                shopSlots[i].gameObject.SetActive(false);
            }
        }

        public void TryBuyItem(ShopItem shopItem)
        {
            if (shopItem.paymentType == PaymentType.PayWithMonsterDrop)
            {
                Debug.Log($"[CHECK SHOP] Đang mua bằng Đầu Lâu. Trong túi có: {inventoryManager.monsterDropTotal} | Giá món đồ: {shopItem.price}");
            }
            // ============================================
            // 1. KIỂM TRA XEM CÓ ĐỦ TÀI NGUYÊN ĐỂ TRẢ KHÔNG?
            // ============================================
            bool canAfford = false;

            // Kiểm tra Vàng, Gỗ qua ResourceManager
            if (shopItem.paymentType == PaymentType.PayWithGold && ResourceManager.main.CurrentGold >= shopItem.price)
            {
                canAfford = true;
            }
            else if (shopItem.paymentType == PaymentType.PayWithWood && ResourceManager.main.CurrentWood >= shopItem.price)
            {
                canAfford = true;
            }
            // Kiểm tra Đồ quái rớt qua InventoryManager
            else if (shopItem.paymentType == PaymentType.PayWithMonsterDrop && inventoryManager.monsterDropTotal >= shopItem.price)
            {
                canAfford = true;
            }

            if (!canAfford)
            {
                Debug.Log("Không đủ tài nguyên để giao dịch món này!");
                return;
            }

            // ============================================
            // 2. KIỂM TRA TÚI ĐỒ (Chỉ kiểm tra nếu phần thưởng là Item trang bị)
            // ============================================
            if (shopItem.rewardType == RewardType.ReceiveItem && !HasSpaceForItem(shopItem.itemSO))
            {
                Debug.Log("Túi đồ đã đầy, không thể mua thêm Vật phẩm!");
                return;
            }

            // ============================================
            // 3. TIẾN HÀNH THANH TOÁN (TRỪ TÀI NGUYÊN)
            // ============================================
            if (shopItem.paymentType == PaymentType.PayWithGold)
            {
                ResourceManager.main.SpendResources(shopItem.price, 0); // Trừ Vàng
            }
            else if (shopItem.paymentType == PaymentType.PayWithWood)
            {
                ResourceManager.main.SpendResources(0, shopItem.price); // Trừ Gỗ
            }
            else if (shopItem.paymentType == PaymentType.PayWithMonsterDrop)
            {
                inventoryManager.monsterDropTotal -= shopItem.price; // Trừ Đồ quái rớt
                inventoryManager.UpdateResourceUI(); // Cập nhật UI của túi đồ
            }

            // ============================================
            // 4. TRẢ THƯỞNG (CỘNG TÀI NGUYÊN / THÊM ĐỒ VÀO TÚI)
            // ============================================
            if (shopItem.rewardType == RewardType.ReceiveItem)
            {
                inventoryManager.AddItem(shopItem.itemSO, shopItem.quantity);
                Debug.Log($"Đã mua thành công {shopItem.quantity} {shopItem.itemSO.itemName}");
            }
            else if (shopItem.rewardType == RewardType.ReceiveGold)
            {
                ResourceManager.main.AddResources(shopItem.quantity, 0); // Cộng Vàng thẳng vào ResourceManager
                Debug.Log($"Quy đổi thành công: Nhận {shopItem.quantity} Vàng");
            }
            else if (shopItem.rewardType == RewardType.ReceiveWood)
            {
                ResourceManager.main.AddResources(0, shopItem.quantity);
                Debug.Log($"Quy đổi thành công: Nhận {shopItem.quantity} Gỗ");
            }

        }

        // Hàm kiểm tra túi đồ còn chỗ không
        private bool HasSpaceForItem(ItemSO item)
        {
            foreach (InventorySlot slot in inventoryManager.itemSlots)
            {
                if (slot.item == item && slot.quantity < item.stackSize) return true;
                if (slot.item == null) return true;
            }
            return false;
        }
    }

    // --- CÁC CLASS DATA ---

    [System.Serializable]
    public class ShopTab
    {
        public string tabName; // Tên tab (VD: "Vật phẩm", "Vũ khí") để bạn dễ phân biệt trên Inspector
        public List<ShopItem> items; // Danh sách hàng hóa dành riêng cho Tab này
    }

    [System.Serializable]
    public class ShopItem
    {
        [Header("Loại giao dịch")]
        public RewardType rewardType = RewardType.ReceiveItem; // Sẽ nhận được gì?
        public PaymentType paymentType = PaymentType.PayWithGold; // Phải trả bằng gì?

        [Header("Thông tin hiển thị")]
        public ItemSO itemSO;     // Kéo SO vào đây để lấy Icon và Tên hiển thị lên UI

        [Header("Giá & Số lượng")]
        public int price;         // Tốn bao nhiêu tiền/gỗ?
        public int quantity = 1;  // Nhận được bao nhiêu món/vàng/gỗ?
    }
}
