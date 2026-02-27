using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.TowerBuilding.UIBuildMenu
{
    public class TowerHealthBarUI : MonoBehaviour
    {
        [Header("Kéo cái ảnh FillBar (màu đỏ) vào đây")]
        public Image fillImage;

        [Header("Tốc độ tụt máu (Càng to càng nhanh)")]
        public float lerpSpeed = 5f;

        // Biến lưu trữ % máu mục tiêu cần chạy tới
        private float targetFillAmount = 1f;

        // Hàm khởi tạo lúc mới xây tháp xong
        public void Setup(float currentHP, float maxHP)
        {
            if (maxHP > 0)
            {
                targetFillAmount = currentHP / maxHP;

                // Set độ dài đầy ngay lập tức lúc mới xây
                if (fillImage != null) fillImage.fillAmount = targetFillAmount;
            }
            gameObject.SetActive(false);
        }

        // Hàm này giờ chỉ cập nhật "MỤC TIÊU" cần tụt tới
        public void UpdateHealthUI(float currentHP, float maxHP)
        {
            if (maxHP > 0)
            {
                targetFillAmount = currentHP / maxHP;
            }
        }

        // --- CODE MỚI: Xử lý hiệu ứng tụt từ từ mỗi khung hình ---
        private void Update()
        {
            if (fillImage != null)
            {
                // Nếu độ dài hiện tại chưa bằng mục tiêu -> Cho nó chạy từ từ về mục tiêu
                if (fillImage.fillAmount != targetFillAmount)
                {
                    fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFillAmount, Time.deltaTime * lerpSpeed);
                }
            }
        }

        // Hàm Bật/Tắt thanh máu
        public void Toggle(bool show)
        {
            gameObject.SetActive(show);
        }
    }
}
