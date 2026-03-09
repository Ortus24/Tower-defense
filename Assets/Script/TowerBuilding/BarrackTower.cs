using Assets.Script.TowerBuilding.EconomyTower;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

public class BarrackTower : BaseTower
{
    [Header("Điểm tập trung (Rally Points)")]
    public Transform[] spawnPoints; // Kéo các vị trí đứng gác vào đây

    [Header("Cài đặt UI (Banner & Range)")]
    public BuildingBanner bannerScript;
    public PlacementVidual rangeVisual;

    // Quản lý danh sách lính
    private List<KnightAI> activeSoldiers = new List<KnightAI>();
    private float respawnTimer = 0f;

    protected override void Start()
    {
        base.Start();
        // Setup UI (Banner & Range)
        if (bannerScript != null)
        {
            bannerScript.Setup(gameObject, data);
            bannerScript.gameObject.SetActive(false);
        }

        if (rangeVisual != null && data != null)
        {
            rangeVisual.SetRange(data.range); // Range này là vùng lính đi tuần tra
            rangeVisual.ToggleRange(false);
        }
    }

    void Update()
    {
        if (!isBuilt) return;

        // 1. Dọn dẹp danh sách (Xóa lính đã chết - null)
        activeSoldiers.RemoveAll(soldier => soldier == null);

        // 2. Logic Hồi sinh (Respawn)
        // Lấy số lượng lính tối đa từ Data
        int maxSoldiers = data.soldierCount > 0 ? data.soldierCount : 1;

        if (activeSoldiers.Count < maxSoldiers)
        {
            respawnTimer -= Time.deltaTime;

            if (respawnTimer <= 0f)
            {
                SpawnOneSoldier();

                // Reset bộ đếm theo Data
                respawnTimer = data.respawnTime > 0 ? data.respawnTime : 10f;
            }
        }
    }

    void SpawnFullSquad()
    {
        ClearAllSoldiers(); // Xóa lính cũ (nếu có) trước khi spawn mới

        int maxSoldiers = data.soldierCount > 0 ? data.soldierCount : 1;
        for (int i = 0; i < maxSoldiers; i++)
        {
            SpawnOneSoldier();
        }
    }

    void SpawnOneSoldier()
    {
        if (data.soldierPrefab == null || spawnPoints.Length == 0) return;

        // Chọn vị trí đứng: Lính 1 -> Điểm 1, Lính 2 -> Điểm 2 (Chia lấy dư)
        int spawnIndex = activeSoldiers.Count % spawnPoints.Length;
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Tạo lính
        GameObject soldierGO = Instantiate(data.soldierPrefab, spawnPoint.position, Quaternion.identity);
        KnightAI knightScript = soldierGO.GetComponent<KnightAI>();

        if (knightScript != null)
        {
            // --- QUAN TRỌNG: TRUYỀN CHỈ SỐ TỪ THÁP SANG LÍNH ---
            // Lính sẽ dùng Damage và HP của Tháp
            knightScript.SetupSoldier(this, data.damage, data.maxHP, spawnPoint.position);
            // ----------------------------------------------------
        }

        activeSoldiers.Add(knightScript);
    }

    void ClearAllSoldiers()
    {
        foreach (var soldier in activeSoldiers)
        {
            if (soldier != null) Destroy(soldier.gameObject);
        }
        activeSoldiers.Clear();
    }

    // Xử lý Click chuột (Hiện Banner + Range)
    private void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        bool newState = false;

        // Bật/Tắt Banner và Vòng tròn Range
        if (bannerScript != null)
        {
            newState = !bannerScript.gameObject.activeSelf;
            bannerScript.gameObject.SetActive(newState);

            if (rangeVisual != null) rangeVisual.ToggleRange(newState);
        }

        // ĐỒNG BỘ THANH MÁU LUÔN
        if (healthBarScript != null)
        {
            healthBarScript.Toggle(newState);
            if (newState == true)
            {
                healthBarScript.UpdateHealthUI(currentHP, data.maxHP);
            }
        }
    }

    // Khi tháp bị phá hủy -> Lính cũng chết theo
    private void OnDestroy()
    {
        ClearAllSoldiers();
    }
}