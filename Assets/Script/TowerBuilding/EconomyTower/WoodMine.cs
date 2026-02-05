using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.TowerBuilding.EconomyTower
{
    public class WoodMine : MonoBehaviour
    {
        [Header("Dữ liệu")]
        public TowerData data; // Kéo file SO_Wood vào đây

        [Header("Hiển thị")]
        public GameObject woodIcon;      // Kéo icon Khúc Gỗ vào đây
        public GameObject popupPrefab;   // Kéo Prefab "TextPopup" vào đây

        private int currentStoredWood = 0;
        private float timer = 0f;

        private void Start()
        {
            if (woodIcon != null) woodIcon.SetActive(false);
        }

        private void Update()
        {
            if (data == null) return;

            // Kiểm tra kho chứa
            if (currentStoredWood < data.maxWoodCapacity)
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    timer = 0f;
                    ProduceWood();
                }
            }
        }

        void ProduceWood()
        {
            // Cộng gỗ (Lấy chỉ số woodPerSecond)
            currentStoredWood += data.woodPerSecond;

            if (currentStoredWood > data.maxWoodCapacity)
                currentStoredWood = data.maxWoodCapacity;

            UpdateVisual();
        }

        void UpdateVisual()
        {
            if (woodIcon != null)
            {
                // Hiện icon Khúc Gỗ khi có tài nguyên
                woodIcon.SetActive(currentStoredWood > 0);
            }
        }

        private void OnMouseDown()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            if (currentStoredWood > 0)
            {
                CollectWood();
            }
        }

        void CollectWood()
        {
            // 1. Gửi về ResourceManager (Số thứ 2 là Gỗ)
            if (ResourceManager.main != null)
            {
                ResourceManager.main.AddResources(0, currentStoredWood);
                Debug.Log($"Thu hoạch được {currentStoredWood} gỗ!");
            }

            // 2. Tạo Popup bay lên
            if (popupPrefab != null)
            {
                GameObject popup = Instantiate(popupPrefab, transform.position, Quaternion.identity);
                DamagePopup damagePopup = popup.GetComponent<DamagePopup>();

                if (damagePopup != null)
                {
                    // Setup số lượng và màu sắc (Ví dụ màu Nâu cho gỗ)
                    damagePopup.Setup(currentStoredWood);

                    // (Nâng cao) Nếu bạn muốn đổi màu chữ thành màu Nâu cho khác màu Vàng:
                    // damagePopup.SetColor(new Color(0.6f, 0.4f, 0.2f)); 
                }
            }

            // 3. Reset
            currentStoredWood = 0;
            timer = 0f;
            UpdateVisual();
        }
    }
}
