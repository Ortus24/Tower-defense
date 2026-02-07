using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.TowerBuilding.EconomyTower
{
    public class BuildingInfoUI : MonoBehaviour
    {
        public static BuildingInfoUI main;

        [Header("1. Panel cha")]
        public GameObject uiPanel;

        [Header("2. Text hiển thị")]
        public TextMeshProUGUI nameText;       // Tên công trình
        public TextMeshProUGUI statsText;      // Nội dung chỉ số (Description)
        public Image towerImage;               // (Tùy chọn) Hình ảnh công trình

        [Header("3. Button")]
        public Button closeButton;             // Nút đóng bảng

        private GameObject _ownerBuilding;
        private TowerData _data;

        private void Awake()
        {
            // Singleton Setup
            if (main == null) main = this;
            else Destroy(gameObject);

            Hide(); // Ẩn lúc đầu

            if (closeButton) closeButton.onClick.AddListener(Hide);
        }

        // --- HÀM HIỂN THỊ ---
        public void Show(TowerData data)
        {
            if (data == null) return;

            // 1. Cập nhật Tên
            if (nameText != null) nameText.text = data.towerName;

            // 2. Cập nhật Chỉ số
            if (statsText != null)
            {
                statsText.text = GetStatsContent(data);
            }

            // 3. CẬP NHẬT HÌNH ẢNH (Thêm đoạn này)
            if (towerImage != null)
            {
                if (data.towerIcon != null)
                {
                    towerImage.sprite = data.towerIcon; // Gán ảnh từ Data vào UI
                    towerImage.gameObject.SetActive(true); // Bật lên
                    
                    // (Mẹo) Giữ tỉ lệ ảnh gốc cho đẹp, không bị méo
                    towerImage.preserveAspect = true; 
                }
                else
                {
                    // Nếu Data không có ảnh thì ẩn cái khung ảnh đi cho đỡ xấu
                    towerImage.gameObject.SetActive(false); 
                }
            }

            // 4. Hiện bảng
            if (uiPanel != null) uiPanel.SetActive(true);
        }

        public void Hide()
        {
            if (uiPanel != null) uiPanel.SetActive(false);
        }

        // --- HÀM TẠO TEXT CHỈ SỐ (Copy từ Upgrade UI để đồng bộ) ---
        string GetStatsContent(TowerData data)
        {
            if (data == null) return "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // --- Chỉ số Chiến đấu ---
            if (data.maxHP > 0) sb.AppendLine($"<color=#FF5555>HP:</color> {data.maxHP}");
            if (data.damage > 0) sb.AppendLine($"DMG: {data.damage}");
            if (data.range > 0) sb.AppendLine($"Range: {data.range}");
            if (data.attackSpeed > 0) sb.AppendLine($"Spd: {data.attackSpeed}/s");

            // --- Chỉ số Kinh tế (Vàng) ---
            if (data.goldPerSecond > 0)
            {
                sb.AppendLine($"<color=#FFD700>Gold:</color> {data.goldPerSecond}/s");
                sb.AppendLine($"<color=#FFD700>Cap:</color> {data.maxGoldCapacity}");
            }

            // --- Chỉ số Kinh tế (Gỗ) ---
            if (data.woodPerSecond > 0)
            {
                sb.AppendLine($"<color=#8B4513>Wood:</color> {data.woodPerSecond}/s");
                sb.AppendLine($"<color=#8B4513>Cap:</color> {data.maxWoodCapacity}");
            }

            // --- Mô tả thêm (nếu có trong Data) ---
            if (!string.IsNullOrEmpty(data.description))
            {
                sb.AppendLine("----------------");
                sb.AppendLine($"<i>{data.description}</i>");
            }

            return sb.ToString();
        }
    }
}
