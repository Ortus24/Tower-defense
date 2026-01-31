using Assets.Script.TowerBuilding;
using Mono.Cecil.Cil;
using NUnit.Framework;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager main;
    public TowerData data; // Data này chứa towerSize (Vector2Int)
    [SerializeField] private GameObject[] buildingPrefabs; // Tháp thật
    [SerializeField] private GameObject[] ghostPrefabs;    // Tháp mờ
    [SerializeField] private float snapOffsetX = 0.7f;
    [SerializeField] private float snapOffsetY = 0.3f;
    [SerializeField] private float snapOffsetXBarrack = 1f;
    [SerializeField] private float snapOffsetYBarrack = 0.3f;
    private int selectedIndex = -1;
    private GameObject currentGhost;


    private void Awake() { main = this; }

    public void SelectTower(int index)
    {
        if (currentGhost != null) Destroy(currentGhost);
        selectedIndex = index;
        currentGhost = Instantiate(ghostPrefabs[index]);
    }

    private void Update()
    {
        if (currentGhost == null) return;

        // A. Lấy kích thước tháp từ Ghost đang chọn
        PlacementCheck ghostPlacement = currentGhost.GetComponent<PlacementCheck>();
        Vector2Int size = ghostPlacement.data.towerSize;
        // B. Tính toán Snapping tự động
        // Nếu size là số chẵn (như 2), offset sẽ là 0.5 (hoặc snapOffset của bạn)
        // Nếu size là số lẻ (như 3), tháp sẽ nằm chính xác tại Floor(mousePos)
        float offsetX = (size.x % 2 == 0) ? snapOffsetX : snapOffsetXBarrack;
        float offsetY = (size.y % 2 == 0) ? snapOffsetY : snapOffsetYBarrack;

        // 1. Lấy vị trí chuột và chuyển đổi sang tọa độ thế giới
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        // Logic Snapping cho tháp
        float snappedX = Mathf.Floor(mousePos.x) + offsetX;
        float snappedY = Mathf.Floor(mousePos.y) + offsetY;
        currentGhost.transform.position = new Vector3(snappedX, snappedY, 0);

        // 3. Click chuột trái để xây tháp
        if (Input.GetMouseButtonDown(0) &&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && // Ngăn xây tháp khi bấm vào Button UI
            currentGhost.GetComponent<PlacementCheck>().CanPlace())
        {
            // A. Lấy script PlacementCheck từ Ghost để lấy dữ liệu TowerData
            //PlacementCheck ghostPlacement = currentGhost.GetComponent<PlacementCheck>();

            // B. Tạo tháp thật tại vị trí của Ghost
            GameObject newTower = Instantiate(buildingPrefabs[selectedIndex], currentGhost.transform.position, Quaternion.identity);

            // C. Cập nhật Grid: Đánh dấu các ô đất đã bị tháp này chiếm dụng (2x2 hoặc 2x3)
            Vector2Int gridPos;
            if (size.x % 2 == 0 && size.y % 2 == 0)
            {
                gridPos = new Vector2Int(Mathf.FloorToInt(currentGhost.transform.position.x), Mathf.FloorToInt(currentGhost.transform.position.y));
            }
            else
            {
                gridPos = new Vector2Int(
                    Mathf.RoundToInt(currentGhost.transform.position.x - (size.x / 3f)),
                    Mathf.RoundToInt(currentGhost.transform.position.y - (size.y / 2f))
                );
            }
            GridManager.main.OccupyArea(gridPos, ghostPlacement.data.towerSize);

            // D. Xử lý hiển thị: Tự động gán Sorting Order dựa trên tọa độ Y để tháp không đè lên nhau sai thứ tự
            // Tháp càng thấp (Y nhỏ), Sorting Order càng cao -> hiện lên trên
            SpriteRenderer towerSr = newTower.GetComponentInChildren<SpriteRenderer>();
            if (towerSr != null)
            {
                towerSr.sortingOrder = Mathf.RoundToInt(newTower.transform.position.y * -100);
            }
            // E. Dọn dẹp
            Destroy(currentGhost);
            selectedIndex = -1;
        }

        // 4. Click chuột phải để hủy chọn tháp (Nên thêm để trải nghiệm tốt hơn)
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(currentGhost);
            selectedIndex = -1;
        }
    }

}
