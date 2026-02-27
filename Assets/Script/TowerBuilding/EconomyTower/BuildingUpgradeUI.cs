using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.TowerBuilding.EconomyTower
{
    public class BuildingUpgradeUI : MonoBehaviour
    {
        public static BuildingUpgradeUI main;

        [Header("1. Panel cha")]
        public GameObject uiPanel;

        [Header("2. Text hiển thị")]
        public TextMeshProUGUI nameText;

        [Header("Hai cột chỉ số")]
        public TextMeshProUGUI currentStatsText;
        public TextMeshProUGUI nextStatsText;

        [Header("Hiển thị Giá & Icon")]
        public TextMeshProUGUI costGoldText;
        public TextMeshProUGUI costWoodText;
        public GameObject costGoldIcon;
        public GameObject costWoodIcon;

        [Header("3. Button")]
        public Button upgradeButton;
        public Button closeButton;

        private GameObject _selectedBuilding;
        private TowerData _nextLevelData;
        private GameObject _nextLevelPrefab;

        private void Awake()
        {
            if (main == null) main = this;
            else Destroy(gameObject);

            Hide();

            if (upgradeButton) upgradeButton.onClick.AddListener(OnUpgradeClicked);
            if (closeButton) closeButton.onClick.AddListener(Hide);
        }

        // --- 1. HÀM TẠO TEXT CHỈ SỐ CƠ BẢN (Cho cột trái) ---
        string GetBaseStats(TowerData data)
        {
            if (data == null) return "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // Chỉ số chiến đấu (Chỉ hiện nếu > 0)
            if (data.maxHP > 0) sb.AppendLine($"HP: {data.maxHP}");
            if (data.damage > 0) sb.AppendLine($"DMG: {data.damage}");
            if (data.range > 0) sb.AppendLine($"Range: {data.range}");
            if (data.attackSpeed > 0) sb.AppendLine($"Spd: {data.attackSpeed}/s");

            // --- THÊM MỚI: SỐ MŨI TÊN (Chỉ hiện nếu bắn > 1 tên) ---
            if (data.projectilesPerShot > 0)
            {
                // Màu Cyan (#00FFFF) để nổi bật chỉ số đặc biệt
                sb.AppendLine($"Arrows:</color> {data.projectilesPerShot}");
            }
            if (data.soldierCount > 0)
            {
                // Màu Cyan (#00FFFF) để nổi bật
                sb.AppendLine($"Soldiers:</color> {data.soldierCount}");
                sb.AppendLine($"Respawn: {data.respawnTime}s");
            }


            // Kinh tế Vàng
            if (data.goldPerSecond > 0) sb.AppendLine($"Gold: {data.goldPerSecond}/s");
            if (data.maxGoldCapacity > 0) sb.AppendLine($"G.Cap: {data.maxGoldCapacity}");

            // Kinh tế Gỗ
            if (data.woodPerSecond > 0) sb.AppendLine($"Wood: {data.woodPerSecond}/s");
            if (data.maxWoodCapacity > 0) sb.AppendLine($"W.Cap: {data.maxWoodCapacity}");

            return sb.ToString();
        }

        // --- 2. HÀM SO SÁNH (Cho cột phải) ---
        // So sánh chỉ số next với current. Nếu tăng thì hiện màu xanh.
        string GetComparisonStats(TowerData current, TowerData next)
        {
            if (next == null) return "<color=red>MAX LEVEL</color>";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // Hàm con cục bộ (Local Function) để code đỡ bị lặp lại
            void CompareLine(string label, float valCur, float valNext, string suffix = "")
            {
                // Nếu cả cấp cũ và mới đều không có chỉ số này (== 0) thì bỏ qua
                if (valCur <= 0 && valNext <= 0) return;

                if (valNext > valCur)
                {
                    // Có tăng trưởng -> Hiện giá trị mới + phần chênh lệch màu xanh
                    float diff = valNext - valCur;
                    // Mẹo: Dùng <color=#00FF00> là màu xanh lá sáng
                    sb.AppendLine($"{label}: {valNext}{suffix} <color=#00FF00>(+{diff})</color>");
                }
                else
                {
                    // Giữ nguyên hoặc giảm -> Chỉ hiện giá trị mới
                    sb.AppendLine($"{label}: {valNext}{suffix}");
                }
            }

            // Gọi hàm so sánh lần lượt từng chỉ số
            CompareLine("HP", current.maxHP, next.maxHP);
            CompareLine("DMG", current.damage, next.damage);
            CompareLine("Range", current.range, next.range);
            CompareLine("Spd", current.attackSpeed, next.attackSpeed, "/s");

            // --- THÊM MỚI: SO SÁNH SỐ MŨI TÊN ---
            // Chỉ so sánh nếu một trong 2 cấp có Multishot
            if (current.projectilesPerShot > 1 || next.projectilesPerShot > 1)
            {
                CompareLine("Arrows", current.projectilesPerShot, next.projectilesPerShot);
            }
            if (current.soldierCount > 0 || next.soldierCount > 0)
            {
                CompareLine("Soldiers", current.soldierCount, next.soldierCount);

                // Riêng Respawn Time: GIẢM đi là TỐT -> Logic màu ngược lại
                float curR = current.respawnTime;
                float nxtR = next.respawnTime;

                // Chỉ hiện dòng Respawn nếu một trong 2 cấp có dữ liệu
                if (curR > 0 || nxtR > 0)
                {
                    float diff = curR - nxtR; // Thời gian giảm đi bao nhiêu

                    if (nxtR < curR) // Giảm thời gian -> Tốt (Màu xanh)
                        sb.AppendLine($"Respawn: {nxtR}s <color=#00FF00>(-{diff}s)</color>");

                    else if (nxtR > curR) // Tăng thời gian -> Xấu (Màu đỏ)
                        sb.AppendLine($"Respawn: {nxtR}s <color=#FF0000>(+{nxtR - curR}s)</color>");

                    else // Bằng nhau -> Màu trắng
                        sb.AppendLine($"Respawn: {nxtR}s");
                }
            }

            CompareLine("Gold", current.goldPerSecond, next.goldPerSecond, "/s");
            CompareLine("G.Cap", current.maxGoldCapacity, next.maxGoldCapacity);

            CompareLine("Wood", current.woodPerSecond, next.woodPerSecond, "/s");
            CompareLine("W.Cap", current.maxWoodCapacity, next.maxWoodCapacity);

            return sb.ToString();
        }

        public void Show(GameObject buildingObj, TowerData currentData)
        {
            if (currentData == null) return;

            _selectedBuilding = buildingObj;
            _nextLevelData = currentData.nextLevelData;
            _nextLevelPrefab = currentData.nextLevelPrefab;

            // 1. CẬP NHẬT CỘT TRÁI
            if (currentStatsText != null)
            {
                currentStatsText.text = GetBaseStats(currentData); // Bạn nhớ paste lại hàm GetBaseStats vào nhé
                currentStatsText.alignment = TextAlignmentOptions.Center;
            }
                

            if (nameText != null) nameText.text = currentData.towerName;

            // 2. XỬ LÝ CẤP TIẾP THEO
            if (_nextLevelData != null)
            {
                // Cột Phải
                if (nextStatsText != null)
                {
                    nextStatsText.gameObject.SetActive(true);
                    nextStatsText.text = GetComparisonStats(currentData, _nextLevelData); // Bạn nhớ paste lại hàm GetComparisonStats vào nhé
                    nextStatsText.alignment = TextAlignmentOptions.Center;
                }

                if (nameText != null)
                    nameText.text = $"{currentData.towerName}";

                // --- [QUAN TRỌNG] SỬA Ở ĐÂY ---
                // Hiển thị giá NÂNG CẤP (Cost Upgrade) chứ không phải giá xây dựng (Cost)
                SetCostUI(true, _nextLevelData.goldCostUpgrade, _nextLevelData.woodCostUpgrade);
                // ------------------------------

                if (upgradeButton != null) upgradeButton.gameObject.SetActive(true);
            }
            else
            {
                // MAX LEVEL
                if (nextStatsText != null)
                {
                    nextStatsText.gameObject.SetActive(true);
                    nextStatsText.text = "<size=120%><color=red>MAX LEVEL</color></size>";
                    nextStatsText.alignment = TextAlignmentOptions.Center;
                }

                SetCostUI(false, 0, 0);

                if (upgradeButton != null) upgradeButton.gameObject.SetActive(false);
            }

            if (uiPanel != null) uiPanel.SetActive(true);
        }

        void SetCostUI(bool isActive, int gold, int wood)
        {
            if (costGoldText) costGoldText.gameObject.SetActive(isActive);
            if (costWoodText) costWoodText.gameObject.SetActive(isActive);
            if (costGoldIcon) costGoldIcon.SetActive(isActive);
            if (costWoodIcon) costWoodIcon.SetActive(isActive);

            if (isActive)
            {
                if (costGoldText) costGoldText.text = gold.ToString();
                if (costWoodText) costWoodText.text = wood.ToString();

                // Đổi màu nếu không đủ tiền (Sử dụng Resource Manager)
                if (ResourceManager.main != null)
                {
                    // Lưu ý: ResourceManager của bạn phải Public biến currentGold/Wood hoặc dùng Property như bài trước
                    if (costGoldText) costGoldText.color = ResourceManager.main.CurrentGold >= gold ? Color.white : Color.red;
                    if (costWoodText) costWoodText.color = ResourceManager.main.CurrentWood >= wood ? Color.white : Color.red;
                }
            }
        }

        public void Hide()
        {
            if (uiPanel != null) uiPanel.SetActive(false);
            _selectedBuilding = null;
        }

        void OnUpgradeClicked()
        {
            if (_nextLevelData != null)
            {
                Debug.Log($"Đang thử nâng cấp. Giá Gold: {_nextLevelData.goldCostUpgrade} - Giá Wood: {_nextLevelData.woodCostUpgrade}");
            }
            if (_selectedBuilding == null || _nextLevelData == null) return;
            
            // --- [QUAN TRỌNG] SỬA Ở ĐÂY ---
            // Kiểm tra và Trừ tiền theo giá UPGRADE
            int costG = _nextLevelData.goldCostUpgrade;
            int costW = _nextLevelData.woodCostUpgrade;

            if (ResourceManager.main.HasEnoughResources(costG, costW))
            {
                ResourceManager.main.SpendResources(costG, costW);

                Vector3 oldPos = _selectedBuilding.transform.position;
                Quaternion oldRot = _selectedBuilding.transform.rotation;
                Destroy(_selectedBuilding);
                Instantiate(_nextLevelPrefab, oldPos, oldRot);

                Hide();
            }
            else
            {
                Debug.Log("Không đủ tiền để nâng cấp!");
            }
        }
    }
}
