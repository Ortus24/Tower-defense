using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

namespace Assets.Script.TowerBuilding.EconomyTower
{
    public class WoodMine : BaseTower
    {
        //[Header("Dữ liệu")]
        //public TowerData data;

        [Header("Hiển thị")]
        public GameObject woodIcon;      // Icon Khúc Gỗ (Thay cho CoinIcon)
        public GameObject popupPrefab;

        [Header("UI Context (Banner)")]
        public BuildingBanner bannerScript;

        [Header("Cài đặt Timer")]
        public float productionInterval = 5f; // Thời gian chờ sản xuất (Giống GoldMine)

        private int currentStoredWood = 0;
        private float timer = 0f;

        protected override void Start()
        {
            base.Start();
            if (woodIcon != null) woodIcon.SetActive(false);

            if (bannerScript != null)
            {
                bannerScript.Setup(gameObject, data);
                bannerScript.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (data == null) return;

            // Nếu kho chưa đầy -> Bắt đầu đếm giờ
            if (currentStoredWood < data.maxWoodCapacity)
            {
                timer += Time.deltaTime;

                if (timer >= productionInterval)
                {
                    timer = 0f;
                    ProduceWood();
                }
            }
        }

        void ProduceWood()
        {
            // Tính lượng gỗ sinh ra sau khoảng thời gian interval
            // Ví dụ: 5 gỗ/s * 5s = 25 gỗ
            int woodToAdd = data.woodPerSecond * (int)productionInterval;

            currentStoredWood += woodToAdd;

            if (currentStoredWood > data.maxWoodCapacity)
                currentStoredWood = data.maxWoodCapacity;

            UpdateVisual();
        }

        void UpdateVisual()
        {
            if (woodIcon != null)
            {
                woodIcon.SetActive(currentStoredWood > 0);
            }
        }

        // --- XỬ LÝ CLICK CHUỘT ---
        private void OnMouseDown()
        {
            // Chặn click xuyên qua UI
            if (EventSystem.current.IsPointerOverGameObject()) return;

            bool newState = true;
            // 1. Ưu tiên Thu hoạch
            if (currentStoredWood > 0)
            {
                CollectWood();
                if (bannerScript != null) bannerScript.gameObject.SetActive(false);
                return;
            }

            // 2. Nếu rỗng -> Bật/Tắt Banner nâng cấp
            if (bannerScript != null)
            {
                bool isActive = bannerScript.gameObject.activeSelf;
                bannerScript.gameObject.SetActive(!isActive);
            }

            // 3. ĐỒNG BỘ THANH MÁU
            if (healthBarScript != null)
            {
                healthBarScript.Toggle(newState);
                if (newState == true)
                {
                    healthBarScript.UpdateHealthUI(currentHP, data.maxHP);
                }
            }

            //Test
            TakeDamage(10);
        }

        void CollectWood()
        {
            // CỘNG TÀI NGUYÊN
            // Lưu ý: AddResources(Gold, Wood) -> Nên để 0 ở vị trí Gold, và amount ở vị trí Wood
            if (ResourceManager.main != null)
            {
                ResourceManager.main.AddResources(0, currentStoredWood);
            }

            // HIỆN POPUP
            if (popupPrefab != null)
            {
                GameObject popup = Instantiate(popupPrefab, transform.position, Quaternion.identity);
                DamagePopup damagePopup = popup.GetComponent<DamagePopup>();

                if (damagePopup != null)
                {
                    damagePopup.Setup(currentStoredWood);

                    // (Tùy chọn) Nếu bạn muốn popup gỗ có màu khác (ví dụ màu nâu)
                    // damagePopup.SetColor(new Color(0.6f, 0.4f, 0.2f)); 
                }
            }

            // RESET
            currentStoredWood = 0;
            timer = 0f;
            UpdateVisual();
        }
    }
}
