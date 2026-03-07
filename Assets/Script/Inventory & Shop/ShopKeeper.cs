using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


namespace Assets.Script.Inventory___Shop
{
    public class Shopkeeper : MonoBehaviour
    {
        [Header("Cấu hình UI")]
        [SerializeField] private GameObject shopPanel; 

        private bool isShopOpen = false;

        private void Start()
        {

            if (shopPanel != null) shopPanel.SetActive(false);
        }

        private void Update()
        {
            // Đóng bằng phím Escape cho tiện
            if (isShopOpen && Input.GetButtonDown("Cancel"))
            {
                ToggleShop(false);
            }
        }

        public void ToggleShopFromButton()
        {
            ToggleShop(!isShopOpen);
        }

        private void ToggleShop(bool open)
        {
            if (shopPanel == null) return;

            isShopOpen = open;

            // Bật hoặc Tắt GameObject trực tiếp
            shopPanel.SetActive(open);

            // Dừng game hoặc chạy tiếp
            Time.timeScale = open ? 0 : 1;
        }


    }
}
