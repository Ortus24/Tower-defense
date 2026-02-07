using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.AppUI.Redux;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Script.TowerBuilding.EconomyTower
{
    public class GoldMine : MonoBehaviour
    {
        [Header("Dữ liệu")]
        public TowerData data;

        [Header("Hiển thị")]
        public GameObject coinIcon;
        public GameObject popupPrefab;

        [Header("UI Context (Banner)")]
        public BuildingBanner bannerScript;

        // --- THÊM PHẦN NÀY ---
        [Header("Cài đặt Timer")]
        public float productionInterval = 5f; // Thời gian chờ để sinh vàng (Mặc định 10s)
        // ---------------------

        private int currentStoredGold = 0;
        private float timer = 0f;

        private void Start()
        {
            if (coinIcon != null) coinIcon.SetActive(false);

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
            if (currentStoredGold < data.maxGoldCapacity)
            {
                timer += Time.deltaTime;

                // SỬA LOGIC Ở ĐÂY: So sánh với biến productionInterval (10s)
                if (timer >= productionInterval)
                {
                    timer = 0f; // Reset đồng hồ
                    ProduceGold();
                }
            }
        }

        void ProduceGold()
        {
            // LƯU Ý QUAN TRỌNG: 
            // Vì 10s mới sinh 1 lần, bạn có muốn nhân số lượng vàng lên không?
            // Cách 1: Cộng đúng số goldPerSecond trong Data (Ví dụ: 10s được 5 vàng -> Rất ít).
            // Cách 2: Cộng dồn theo thời gian (Ví dụ: 5 vàng/giây * 10s = 50 vàng).

            // Ở đây mình dùng Cách 2 để đảm bảo kinh tế đúng với thông số "Per Second"
            int goldToAdd = data.goldPerSecond * (int)productionInterval;

            currentStoredGold += goldToAdd;

            if (currentStoredGold > data.maxGoldCapacity)
                currentStoredGold = data.maxGoldCapacity;

            UpdateVisual();
        }

        void UpdateVisual()
        {
            if (coinIcon != null) coinIcon.SetActive(currentStoredGold > 0);
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (currentStoredGold > 0)
            {
                CollectGold();
                if (bannerScript != null) bannerScript.gameObject.SetActive(false);
                return;
            }

            if (bannerScript != null)
            {
                bool isActive = bannerScript.gameObject.activeSelf;
                bannerScript.gameObject.SetActive(!isActive);
            }
        }

        void CollectGold()
        {
            if (ResourceManager.main != null) ResourceManager.main.AddResources(currentStoredGold, 0);

            if (popupPrefab != null)
            {
                GameObject popup = Instantiate(popupPrefab, transform.position, Quaternion.identity);
                popup.GetComponent<DamagePopup>()?.Setup(currentStoredGold);
            }

            currentStoredGold = 0;
            timer = 0f;
            UpdateVisual();
        }
    }
}
