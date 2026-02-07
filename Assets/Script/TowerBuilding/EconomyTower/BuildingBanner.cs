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
        [Header("Kéo nút Nâng cấp (Mũi tên) vào đây")]
        public Button btnUpgrade;

        // (Tùy chọn) Nút Info, Nút Phá hủy... thêm vào đây nếu muốn

        private GameObject _ownerBuilding; // Nhà của tôi
        private TowerData _data;           // Dữ liệu của tôi

        void Start()
        {
            // Gán sự kiện cho nút bấm
            if (btnUpgrade != null)
            {
                btnUpgrade.onClick.AddListener(OnUpgradeClicked);
            }
        }

        // Hàm này được GoldMine gọi ngay khi game bắt đầu để nạp dữ liệu
        public void Setup(GameObject building, TowerData data)
        {
            _ownerBuilding = building;
            _data = data;

            if (btnUpgrade != null)
            {
                btnUpgrade.gameObject.SetActive(true);
            }
        }

        void OnUpgradeClicked()
        {
            // 1. Gọi UI Manager để hiện bảng to
            if (BuildingUpgradeUI.main != null)
            {
                BuildingUpgradeUI.main.Show(_ownerBuilding, _data);
            }

            // 2. Ẩn chính cái Banner này đi cho đỡ vướng
            gameObject.SetActive(false);
        }
    }
}
