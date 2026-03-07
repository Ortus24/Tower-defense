using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Inventory___Shop
{
    public class ShopSlot : MonoBehaviour
    {
        private ShopItem currentShopItem;

        [HideInInspector] public ItemSO itemSO; // Ẩn đi cho Inspector đỡ rối
        private int price;
        private int quantity;



        [Header("UI References (Kéo từ Hierarchy vào)")]
        public Image itemImage;           // Kéo ImageIcon vào đây
        public TextMeshProUGUI quantityText; // Kéo Text (TMP) con của ImageIcon vào đây
        public TextMeshProUGUI itemNameText; // Kéo Text (TMP) con của Banner vào đây
        public TextMeshProUGUI priceText;    // Kéo Text (TMP) con của PricePanel vào đây
        public Image priceIcon; // Kéo cục ImagePrice (nằm cạnh Text giá tiền) vào đây

        [Header("System References")]
        public ShopManager shopManager; // THÊM MỚI: Liên kết tới bộ não của Shop

        // Hàm nhận dữ liệu từ Manager để nạp vào UI
        // Hàm này nhận trực tiếp gói hàng ShopItem thay vì từng biến lẻ
        public void Initialize(ShopItem shopItem)
        {
            currentShopItem = shopItem; // Lưu lại để dùng khi bấm nút Buy

            // Cập nhật UI dựa trên data trong itemSO
            if (shopItem.itemSO != null)
            {
                itemImage.sprite = shopItem.itemSO.itemIcon;
                itemNameText.text = shopItem.itemSO.itemName;
            }

            priceText.text = shopItem.price.ToString();

            // Nếu là quy đổi Vàng/Gỗ hoặc mua nhiều Item thì hiện x10, x50
            if (shopItem.quantity >= 1)
            {
                quantityText.text = "x" + shopItem.quantity.ToString();
                quantityText.gameObject.SetActive(true);
            }
            else
            {
                quantityText.gameObject.SetActive(false);
            }

            // --- THÊM ĐOẠN NÀY ĐỂ TỰ ĐỘNG ĐỔI ICON TIỀN TỆ ---
            if (priceIcon != null && shopManager != null)
            {
                if (shopItem.paymentType == PaymentType.PayWithGold)
                    priceIcon.sprite = shopManager.goldIcon;
                else if (shopItem.paymentType == PaymentType.PayWithWood)
                    priceIcon.sprite = shopManager.woodIcon;
                else if (shopItem.paymentType == PaymentType.PayWithMonsterDrop)
                    priceIcon.sprite = shopManager.monsterDropIcon;
            }
        }

        // --- THÊM MỚI: HÀM NÀY SẼ ĐƯỢC GỌI KHI NGƯỜI CHƠI BẤM VÀO Ô ĐỒ ---
        public void OnBuyButtonClicked()
        {
            if (shopManager != null && currentShopItem != null)
            {
                shopManager.TryBuyItem(currentShopItem);
            }
        }
    }
}
