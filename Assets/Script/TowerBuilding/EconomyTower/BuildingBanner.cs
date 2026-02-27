using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.TowerBuilding.EconomyTower
{
    public class BuildingBanner : MonoBehaviour
    {
        [Header("Nút Nâng cấp (Mũi tên)")]
        public Button btnUpgrade;

        // --- THÊM MỚI: Nút Info ---
        [Header("Nút Thông tin (Chữ i)")]
        public Button btnInfo;
        // --------------------------

        private GameObject _ownerBuilding;
        private TowerData _data;

        void Start()
        {
            // 1. Gán sự kiện cho nút Nâng cấp
            if (btnUpgrade != null)
            {
                // Xóa listener cũ (nếu có) để tránh lỗi duplicate khi pooling
                btnUpgrade.onClick.RemoveAllListeners();
                btnUpgrade.onClick.AddListener(OnUpgradeClicked);
            }

            // 2. Gán sự kiện cho nút Info (MỚI)
            if (btnInfo != null)
            {
                btnInfo.onClick.RemoveAllListeners();
                btnInfo.onClick.AddListener(OnInfoClicked);
            }
        }

        public void Setup(GameObject building, TowerData data)
        {
            _ownerBuilding = building;
            _data = data;

            // Nút Upgrade: Luôn hiện (theo logic bài trước của bạn)
            if (btnUpgrade != null)
            {
                btnUpgrade.gameObject.SetActive(true);
            }

            // Nút Info: Luôn hiện
            if (btnInfo != null)
            {
                btnInfo.gameObject.SetActive(true);
            }
        }

        // --- HÀM XỬ LÝ KHI BẤM NÚT INFO ---
        void OnInfoClicked()
        {
            if (BuildingInfoUI.main != null)
            {
                // Chỉ cần truyền Data để hiển thị, không cần GameObject nhà
                BuildingInfoUI.main.Show(_data);
            }

            // Ẩn banner đi cho đỡ vướng
            gameObject.SetActive(false);
        }

        // --- HÀM XỬ LÝ KHI BẤM NÚT UPGRADE ---
        void OnUpgradeClicked()
        {
            if (BuildingUpgradeUI.main != null)
            {
                BuildingUpgradeUI.main.Show(_ownerBuilding, _data);
            }
            // Ẩn banner đi
            gameObject.SetActive(false);
        }
    }
}
