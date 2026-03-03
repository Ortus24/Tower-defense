using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;


namespace Assets.Script.TowerBuilding.EconomyTower
{
    public class DamagePopup : MonoBehaviour
    {
        private float disappearTimer = 1f; // Thời gian tồn tại
        private TextMeshPro textMesh;
        private Color textColor;
        private Vector3 moveVector;

        private void Awake()
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        // --- NÂNG CẤP: Thêm tham số isDamage (mặc định false để không lỗi code cũ của Mỏ Vàng) ---
        public void Setup(int amount, bool isDamage = false)
        {
            if (textMesh == null) textMesh = GetComponent<TextMeshPro>();

            if (isDamage)
            {
                // Nếu là Sát thương -> Không có dấu +, màu Trắng (hoặc Đỏ tùy bạn chỉnh)
                textMesh.text = amount.ToString();
                textMesh.color = Color.red;
            }
            else
            {
                // Nếu là Thu hoạch mỏ -> Có dấu +, màu Vàng
                textMesh.text = "+" + amount.ToString();
                textMesh.color = Color.yellow;
            }

            textColor = textMesh.color;

            // Bay lên với tốc độ x: random một chút để các số không đè chặt lên nhau, y: bay lên
            moveVector = new Vector3(Random.Range(-1f, 1f), 2f, 0);

            // Tự hủy sau 1.5 giây để dọn dẹp bộ nhớ
            Destroy(gameObject, 1.5f);
        }

        private void Update()
        {
            // Di chuyển lên trên
            transform.position += moveVector * Time.deltaTime;
            moveVector -= moveVector * 2f * Time.deltaTime; // Giảm tốc dần

            // Hiệu ứng mờ dần (Optional - nếu muốn đẹp hơn)
            if (disappearTimer > 0)
            {
                disappearTimer -= Time.deltaTime;
                if (disappearTimer < 0)
                {
                    float disappearSpeed = 3f;
                    textColor.a -= disappearSpeed * Time.deltaTime;
                    textMesh.color = textColor;
                }
            }
        }
    }
}
