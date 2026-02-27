using Assets.Script.TowerBuilding;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager main;

    [SerializeField] private GameObject[] buildingPrefabs;
    [SerializeField] private GameObject[] ghostPrefabs;

    private int selectedIndex = -1;
    private GameObject currentGhost;

    private void Awake() { main = this; Debug.Log("BuildingManager đã sẵn sàng!"); }

    public void SelectTower(int index)
    {
        if (currentGhost != null) Destroy(currentGhost);
        selectedIndex = index;
        currentGhost = Instantiate(ghostPrefabs[index]);
    }

    private void Update()
    {
        if (currentGhost == null) return;
        if (GridManager.main == null || GridManager.main.GetLevelGrid() == null) return;

        // 1. Lấy dữ liệu và tham chiếu
        PlacementCheck ghostPlacement = currentGhost.GetComponent<PlacementCheck>();
        TowerData data = ghostPlacement.data; // Lấy data để dùng nhiều lần cho gọn
        Vector2Int size = data.towerSize;
        float cellSize = GridManager.main.cellSize;

        // 2. Lấy vị trí chuột -> Chuyển sang Grid Coordinate (x, y)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        int gridX, gridY;
        GridManager.main.GetLevelGrid().GetXY(mousePos, out gridX, out gridY);

        // 3. TÍNH TOÁN VỊ TRÍ SNAP
        Vector3 originPos = GridManager.main.GetLevelGrid().GetWorldPosition(gridX, gridY);
        Vector3 centerOffset = new Vector3(size.x * cellSize * 0.5f, size.y * cellSize * 0.5f, 0);
        currentGhost.transform.position = originPos + centerOffset;

        // 4. XỬ LÝ CLICK ĐỂ XÂY
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            // Kiểm tra xem có xây được không (PlacementCheck đã lo vụ check Tiền + check Mỏ/Đất)
            if (ghostPlacement.CanPlace())
            {
                // A. TRỪ TIỀN
                if (ResourceManager.main != null)
                {
                    ResourceManager.main.SpendResources(data.goldCost, data.woodCost);
                }

                // B. TẠO THÁP THẬT
                GameObject newTower = Instantiate(buildingPrefabs[selectedIndex], currentGhost.transform.position, Quaternion.identity);
                // Lấy script BaseTower của tháp vừa tạo ra
                BaseTower towerComponent = newTower.GetComponent<BaseTower>();

                // C. XỬ LÝ CHIẾM ĐẤT (Logic quan trọng mới thêm)
                // Nếu đây là công trình khai thác (Mỏ/Gỗ) -> Xử lý cục quặng
                if (data.resourceType != ResourceType.None)
                {
                    // Nếu tìm thấy cục quặng hợp lệ (đã được PlacementCheck tìm thấy)
                    if (ghostPlacement.currentValidSpot != null)
                    {
                        ghostPlacement.currentValidSpot.Occupy(); // Làm cục quặng biến mất
                        GridManager.main.OccupyArea(new Vector2Int(gridX, gridY), size);

                        // --- TRUYỀN THAM CHIẾU MỎ CHO THÁP GIỮ ---
                        if (towerComponent != null)
                        {
                            towerComponent.occupiedSpot = ghostPlacement.currentValidSpot;
                        }
                    }
                }
                else
                {
                    // Nếu là tháp thường -> Đánh dấu ô đất trên Grid là "Đã bị chiếm"
                    GridManager.main.OccupyArea(new Vector2Int(gridX, gridY), size);
                }

                // D. SẮP XẾP LAYER (Để tháp không bị đè lên nhau sai thứ tự)
                SpriteRenderer towerSr = newTower.GetComponentInChildren<SpriteRenderer>();
                if (towerSr != null) towerSr.sortingOrder = Mathf.RoundToInt(newTower.transform.position.y * -100);

                // E. DỌN DẸP
                Destroy(currentGhost);
                selectedIndex = -1;

                Debug.Log($"Đã xây {data.towerName}! Trừ {data.goldCost} Vàng, {data.woodCost} Gỗ.");
            }
            else
            {
                // In lỗi ra để debug
                if (ResourceManager.main != null && !ResourceManager.main.HasEnoughResources(data.goldCost, data.woodCost))
                {
                    Debug.Log("Không đủ tiền!");
                }
                else
                {
                    Debug.Log("Vị trí không hợp lệ (Vướng vật cản hoặc sai loại mỏ)!");
                }
            }
        }

        // 5. HỦY CHỌN (Chuột phải)
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(currentGhost);
            selectedIndex = -1;
        }
    }

}
