using Assets.Script.TowerBuilding;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager main;

    [SerializeField] private GameObject[] buildingPrefabs;
    [SerializeField] private GameObject[] ghostPrefabs;

    // --- BỎ CÁC BIẾN SNAP OFFSET CŨ ---

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

        // 1. Lấy dữ liệu
        PlacementCheck ghostPlacement = currentGhost.GetComponent<PlacementCheck>();
        Vector2Int size = ghostPlacement.data.towerSize;
        float cellSize = GridManager.main.cellSize;

        // 2. Lấy vị trí chuột -> Chuyển sang Grid Coordinate (x, y)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        int gridX, gridY;
        GridManager.main.GetLevelGrid().GetXY(mousePos, out gridX, out gridY);

        // 3. Giới hạn không cho ghost chạy ra ngoài bản đồ (Optional)
        // gridX = Mathf.Clamp(gridX, 0, GridManager.main.width - size.x);
        // gridY = Mathf.Clamp(gridY, 0, GridManager.main.height - size.y);

        // 4. TÍNH TOÁN VỊ TRÍ SNAP CHUẨN XÁC
        // Lấy vị trí thế giới của góc dưới trái ô (gridX, gridY)
        Vector3 originPos = GridManager.main.GetLevelGrid().GetWorldPosition(gridX, gridY);

        // Cộng thêm nửa kích thước tháp để căn tâm
        Vector3 centerOffset = new Vector3(size.x * cellSize * 0.5f, size.y * cellSize * 0.5f, 0);

        currentGhost.transform.position = originPos + centerOffset;

        // 5. Click để xây
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            // PlacementCheck bây giờ chỉ cần check dựa trên vị trí đã snap
            if (ghostPlacement.CanPlace())
            {
                // --- ĐOẠN CODE MỚI: KIỂM TRA VÀ TRỪ TIỀN ---

                // Lấy thông tin giá tiền từ Data
                TowerData data = ghostPlacement.data;

                // Kiểm tra xem có ResourceManager và có đủ tiền không
                if (ResourceManager.main != null && ResourceManager.main.HasEnoughResources(data.goldCost, data.woodCost))
                {
                    // 1. Trừ tiền
                    ResourceManager.main.SpendResources(data.goldCost, data.woodCost);

                    // 2. Xây tháp (Code cũ)
                    GameObject newTower = Instantiate(buildingPrefabs[selectedIndex], currentGhost.transform.position, Quaternion.identity);

                    // 3. Đánh dấu Grid (Code cũ)
                    GridManager.main.OccupyArea(new Vector2Int(gridX, gridY), size);

                    // 4. Sorting Order (Code cũ)
                    SpriteRenderer towerSr = newTower.GetComponentInChildren<SpriteRenderer>();
                    if (towerSr != null) towerSr.sortingOrder = Mathf.RoundToInt(newTower.transform.position.y * -100);

                    Destroy(currentGhost);
                    selectedIndex = -1;

                    Debug.Log($"Đã xây tháp! Trừ {data.goldCost} Vàng, {data.woodCost} Gỗ.");
                }
                else
                {
                    Debug.Log("Không đủ tiền để xây!");
                    // Ở đây bạn có thể thêm hiệu ứng nhấp nháy đỏ UI hoặc âm thanh báo lỗi
                }
            }
        }
        // 6. Hủy
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(currentGhost);
            selectedIndex = -1;
        }
    }

}
