using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.TowerBuilding
{
    public class PlacementCheck : MonoBehaviour
    {
        public TowerData data;
        private SpriteRenderer sr;
        public SpriteRenderer visualArea;

        void Start() { sr = GetComponentInChildren<SpriteRenderer>(); }

        public bool CanPlace()
        {
            // 1. Kiểm tra GridManager tồn tại không
            if (GridManager.main == null) return false;

            // ---------------------------------------------------------
            // BƯỚC 1: TÍNH TOÁN VỊ TRÍ TRÊN GRID (Giữ nguyên code cũ)
            // ---------------------------------------------------------
            float cellSize = GridManager.main.cellSize;
            Vector3 centerPos = transform.position;

            // Tính ngược ra góc dưới trái
            Vector3 originWorldPos = centerPos - new Vector3(data.towerSize.x * cellSize * 0.5f, data.towerSize.y * cellSize * 0.5f, 0);

            // Chuyển sang tọa độ Grid (x, y)
            int gridX, gridY;
            GridManager.main.GetLevelGrid().GetXY(originWorldPos + new Vector3(cellSize * 0.1f, cellSize * 0.1f), out gridX, out gridY);


            // ---------------------------------------------------------
            // BƯỚC 2: KIỂM TRA ĐIỀU KIỆN (SỬA ĐỔI TẠI ĐÂY)
            // ---------------------------------------------------------

            // Điều kiện A: Đất phải trống
            bool isAreaEmpty = GridManager.main.IsAreaEmpty(new Vector2Int(gridX, gridY), data.towerSize);

            // Điều kiện B: Phải đủ tiền (Thêm mới)
            bool hasMoney = false;
            if (ResourceManager.main != null)
            {
                hasMoney = ResourceManager.main.HasEnoughResources(data.goldCost, data.woodCost);
            }

            // KẾT QUẢ: Chỉ cho phép đặt khi CẢ 2 điều kiện đều đúng
            return isAreaEmpty && hasMoney;
        }

        public void UpdateVisual()
        {
            // Hàm này gọi CanPlace(). 
            // Nếu CanPlace() trả về false (do vướng đất HOẶC hết tiền), nó sẽ tô màu đỏ.
            bool canPlace = CanPlace();

            if (sr != null)
                sr.color = canPlace ? new Color(1, 1, 1, 0.5f) : new Color(1, 0, 0, 0.5f); // Trắng mờ hoặc Đỏ mờ

            if (visualArea != null)
                visualArea.color = canPlace ? new Color(0, 1, 0, 0.4f) : new Color(1, 0, 0, 0.4f); // Xanh lá hoặc Đỏ
        }

        void Update() { UpdateVisual(); }

    }
}
