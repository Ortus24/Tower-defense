using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Inventory___Shop
{
    public class Shopkeeper : MonoBehaviour
    {
        [SerializeField] private Animator anim; // Animator của icon nhảy trên đầu
        [SerializeField] private CanvasGroup shopCanvasGroup; // Nhóm UI của Shop
        [SerializeField] private ShopManager shopManager; // Trình quản lý Shop

        private bool playerInRange; // Kiểm tra người chơi có đang ở gần không
        private bool isShopOpen; // Trạng thái đóng/mở của Shop

        // Lưu trữ danh sách hàng hóa riêng cho từng người bán
        public List<ShopItem> shopItems;
        public List<ShopItem> shopWeapons;
        public List<ShopItem> shopArmor;

        // Sự kiện tĩnh để thông báo khi Shop mở/đóng
        public static Action<bool, ShopManager> OnShopStateChanged;
        public static Shopkeeper currentShopkeeper; // Lưu người bán hiện tại đang tương tác

        private void Update()
        {
            // Nhấn phím 'K' (Interact) để mở, hoặc 'Escape' (Cancel) để đóng
            if (playerInRange && Input.GetButtonDown("Interact") && !isShopOpen)
            {
                ToggleShop(true);
            }
            else if (Input.GetButtonDown("Cancel") && isShopOpen)
            {
                ToggleShop(false);
            }
        }

        private void ToggleShop(bool open)
        {
            isShopOpen = open;
            shopCanvasGroup.alpha = open ? 1 : 0; // Hiện/Ẩn UI
            shopCanvasGroup.blocksRaycasts = open;
            shopCanvasGroup.interactable = open;

            Time.timeScale = open ? 0 : 1; // Tạm dừng game khi mở Shop
            currentShopkeeper = open ? this : null; // Gán người bán hiện tại

            OnShopStateChanged?.Invoke(open, shopManager);

            if (open) OpenItemShop(); // Mặc định mở tab vật phẩm khi bắt đầu
        }

        // Các hàm chuyển đổi Tab hàng hóa
        public void OpenItemShop() => shopManager.PopulateShopItems(shopItems);
        public void OpenWeaponShop() => shopManager.PopulateShopItems(shopWeapons);
        public void OpenArmorShop() => shopManager.PopulateShopItems(shopArmor);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                anim.SetBool("playerInRange", true); // Kích hoạt icon nhảy
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                anim.SetBool("playerInRange", false);
                if (isShopOpen) ToggleShop(false); // Tự động đóng nếu đi quá xa
            }
        }
    }
}
