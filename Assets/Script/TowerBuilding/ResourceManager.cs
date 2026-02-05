using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.TowerBuilding
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager main;

        [Header("Tài nguyên khởi đầu")]
        public int startGold = 100;
        public int startWood = 100;

        [Header("UI References")]
        public TextMeshProUGUI goldText; // Kéo UI Text hiển thị vàng vào đây
        public TextMeshProUGUI woodText; // Kéo UI Text hiển thị gỗ vào đây
                              // Nếu dùng TextMeshPro thì sửa thành: public TextMeshProUGUI goldText;

        // Biến lưu trữ tiền hiện tại (Private để bảo mật, chỉ chỉnh sửa qua hàm)
        private int currentGold;
        private int currentWood;

        private void Awake()
        {
            if (main == null) main = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            // Khởi tạo tiền và cập nhật UI ngay đầu game
            currentGold = startGold;
            currentWood = startWood;
            UpdateUI();
        }

        // 1. Hàm kiểm tra xem có đủ tiền không
        public bool HasEnoughResources(int goldRequired, int woodRequired)
        {
            return currentGold >= goldRequired && currentWood >= woodRequired;
        }

        // 2. Hàm trừ tiền
        public void SpendResources(int goldAmount, int woodAmount)
        {
            if (HasEnoughResources(goldAmount, woodAmount))
            {
                currentGold -= goldAmount;
                currentWood -= woodAmount;
                UpdateUI();
            }
        }

        // 3. Hàm cộng tiền (Dùng cho sau này khi giết quái hoặc đào mỏ)
        public void AddResources(int goldAmount, int woodAmount)
        {
            currentGold += goldAmount;
            currentWood += woodAmount;
            UpdateUI();
        }

        // Cập nhật giao diện
        private void UpdateUI()
        {
            if (goldText != null) goldText.text = currentGold.ToString();
            if (woodText != null) woodText.text = currentWood.ToString();
        }
    }
}
