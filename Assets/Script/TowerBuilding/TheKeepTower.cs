using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace Assets.Script.TowerBuilding
{

    public class TheKeep : BaseTower
    {
        [Header("--- CÀI ĐẶT THE KEEP ---")]
        [SerializeField] private string mainSceneName = "MainScrene"; // Tên scene để quay về
        [SerializeField] private float delayBeforeLoad = 5f;       // Thời gian chờ 5 giây

        protected override void Start()
        {
            base.Start(); // Đảm bảo lấy máu khởi tạo từ BaseTower

            Canvas healthCanvas = GetComponentInChildren<Canvas>(true);
            if (healthCanvas != null)
            {
                // Bật toàn bộ Canvas chứa thanh máu lên
                healthCanvas.gameObject.SetActive(true);

                // Tìm đối tượng BaseBar theo tên và bật nó lên
                Transform baseBar = healthCanvas.transform.Find("BaseBar");
                if (baseBar != null)
                {
                    baseBar.gameObject.SetActive(true);
                }
            }
        }

        // Ghi đè hàm Die để thực hiện logic kết thúc game của riêng Nhà chính
        protected override void Die()
        {
            // 1. Thông báo Console (Dành cho việc Debug)
            Debug.LogWarning("THE KEEP ĐÃ BỊ PHÁ HỦY!");

            // 2. Hiển thị UI Game Over từ GameManager của bạn
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ShowGameOver();
            }

            // 3. Tạm dừng thời gian trong game
            Time.timeScale = 0f;

            // 4. Chạy Coroutine để đợi 5 giây rồi chuyển cảnh
            // Lưu ý: Phải dùng StartCoroutine vì Time.timeScale đang là 0
            StartCoroutine(WaitAndReturnToMenu());
        }

        private IEnumerator WaitAndReturnToMenu()
        {
            // Sử dụng WaitForSecondsRealtime vì Time.timeScale = 0 (thời gian game dừng)
            yield return new WaitForSecondsRealtime(delayBeforeLoad);

            // Trả lại thời gian bình thường trước khi load cảnh mới
            Time.timeScale = 1f;

            // Chuyển về màn hình chính
            SceneManager.LoadScene(mainSceneName);
        }

        // Ghi đè TakeDamage để thêm cảnh báo và xử lý riêng biệt
        public override void TakeDamage(float amount)
        {
            // Chỉ gọi base.TakeDamage (Đã bao gồm logic trừ máu và gọi Die nếu currentHP <= 0)
            base.TakeDamage(amount);

            // Hiển thị log cảnh báo nếu máu dưới 20% và nhà chính chưa sập
            if (currentHP > 0 && currentHP < data.maxHP * 0.2f)
            {
                Debug.LogError("CẢNH BÁO: Nhà chính sắp sập!");
            }
        }
    }
}
