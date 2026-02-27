using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

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

        public void Setup(int amount)
        {
            textMesh.text = "+" + amount.ToString();
            textColor = textMesh.color;
            moveVector = new Vector3(0, 1f, 0) * 2f; // Bay lên với tốc độ 2

            // Tự hủy sau 2 giây
            Destroy(gameObject, 2f);
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
