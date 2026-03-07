using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Inventory___Shop
{
    public class InventoryController : MonoBehaviour
    {
        [Header("Cấu hình UI")]
        [SerializeField] private GameObject inventoryPanel; // Kéo GameObject túi đồ vào đây

        private bool isInventoryOpen = false;

        private void Start()
        {
            // Đảm bảo túi đồ đóng khi mới bắt đầu game
            if (inventoryPanel != null) inventoryPanel.SetActive(false);
        }

        private void Update()
        {
            // Nhấn phím 'I' để bật/tắt túi đồ (phổ biến trong game)
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventoryFromButton();
            }

            // Cho phép đóng nhanh bằng phím Escape nếu túi đang mở
            if (isInventoryOpen && Input.GetButtonDown("Cancel"))
            {
                ToggleInventory(false);
            }
        }

        // Hàm này gán vào sự kiện OnClick() của nút "Balo/Túi đồ" trên màn hình
        public void ToggleInventoryFromButton()
        {
            ToggleInventory(!isInventoryOpen);
        }

        private void ToggleInventory(bool open)
        {
            if (inventoryPanel == null) return;

            isInventoryOpen = open;

            // Bật hoặc Tắt trực tiếp GameObject túi đồ
            inventoryPanel.SetActive(open);

            // Tùy chọn: Dừng game khi xem túi đồ (TimeScale = 0) hoặc để chạy tiếp (TimeScale = 1)
             Time.timeScale = open ? 0 : 1; 
        }
    }
}
