using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Script.TowerBuilding.EconomyTower
{
    public class GoldMine : MonoBehaviour
    {
        [Header("Dữ liệu")]
        public TowerData data;

        [Header("Cài đặt Hiển thị")]
        public GameObject coinIcon;      // Chỉ cần Icon túi tiền
        public GameObject popupPrefab;   // Kéo Prefab "pfDamagePopup" vừa tạo vào đây

        private int currentStoredGold = 0;
        private float timer = 0f;

        private void Start()
        {
            if (coinIcon != null) coinIcon.SetActive(false);
        }

        private void Update()
        {
            if (data == null) return;

            if (currentStoredGold < data.maxGoldCapacity)
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    timer = 0f;
                    ProduceGold();
                }
            }
        }

        void ProduceGold()
        {
            currentStoredGold += data.goldPerSecond;
            if (currentStoredGold > data.maxGoldCapacity)
                currentStoredGold = data.maxGoldCapacity;

            // Chỉ cập nhật trạng thái hiển thị của Icon túi tiền
            UpdateVisual();
        }

        void UpdateVisual()
        {
            // Có tiền thì hiện túi, không thì ẩn
            if (coinIcon != null)
            {
                coinIcon.SetActive(currentStoredGold > 0);
            }
        }

        private void OnMouseDown()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            if (currentStoredGold > 0)
            {
                CollectGold();
            }
        }

        void CollectGold()
        {
            // 1. Cộng tiền
            if (ResourceManager.main != null)
            {
                ResourceManager.main.AddResources(currentStoredGold, 0);
            }

            // 2. TẠO POPUP HIỂN THỊ SỐ TIỀN
            if (popupPrefab != null)
            {
                // Tạo popup tại vị trí mỏ vàng
                GameObject popup = Instantiate(popupPrefab, transform.position, Quaternion.identity);

                // Gọi hàm Setup để set số tiền và tự hủy
                DamagePopup damagePopup = popup.GetComponent<DamagePopup>();
                if (damagePopup != null)
                {
                    damagePopup.Setup(currentStoredGold);
                }
            }

            // 3. Reset
            currentStoredGold = 0;
            timer = 0f;
            UpdateVisual();
        }
    }
}
