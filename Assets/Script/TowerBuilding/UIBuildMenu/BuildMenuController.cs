using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.TowerBuilding.UIBuildMenu
{
    public class BuildMenuController : MonoBehaviour
    {
        [Header("Kéo cái Panel chứa các nút tháp vào đây")]
        public GameObject menuPanel;

        [Header("Kéo nút Cái Búa vào đây")]
        public Button hammerButton;

        [Header("Có muốn chọn tháp xong tự đóng menu không?")]
        public bool closeOnSelect = true;

        private void Start()
        {
            // 1. Đảm bảo lúc đầu game menu bị ẩn
            if (menuPanel != null)
                menuPanel.SetActive(false);

            // 2. Tự động gán sự kiện cho nút Búa
            if (hammerButton != null)
            {
                hammerButton.onClick.AddListener(ToggleMenu);
            }
        }

        // Hàm này dùng để Bật/Tắt menu
        public void ToggleMenu()
        {
            if (menuPanel != null)
            {
                bool isActive = menuPanel.activeSelf;
                menuPanel.SetActive(!isActive); // Nếu đang bật thì tắt, đang tắt thì bật
            }
        }

        // Hàm này để các nút con gọi khi được bấm (để đóng menu lại)
        public void CloseMenu()
        {
            if (menuPanel != null && closeOnSelect)
            {
                menuPanel.SetActive(false);
            }
        }
    }
}
