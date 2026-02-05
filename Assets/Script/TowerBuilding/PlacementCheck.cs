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
            if (GridManager.main == null) return false;

            // 1. Từ vị trí hiện tại (Tâm tháp) -> Tính ngược lại ra góc dưới trái (Grid Origin)
            // Công thức: Tâm - (Size / 2)
            float cellSize = GridManager.main.cellSize;
            Vector3 centerPos = transform.position;
            Vector3 originWorldPos = centerPos - new Vector3(data.towerSize.x * cellSize * 0.5f, data.towerSize.y * cellSize * 0.5f, 0);

            // 2. Chuyển sang tọa độ Grid
            int gridX, gridY;
            GridManager.main.GetLevelGrid().GetXY(originWorldPos + new Vector3(cellSize * 0.1f, cellSize * 0.1f), out gridX, out gridY);
            // (Cộng thêm 0.1f để tránh lỗi làm tròn số ở mép)

            // 3. Kiểm tra vùng đất
            return GridManager.main.IsAreaEmpty(new Vector2Int(gridX, gridY), data.towerSize);
        }

        public void UpdateVisual()
        {
            bool canPlace = CanPlace();
            if (sr != null) sr.color = canPlace ? new Color(1, 1, 1, 0.5f) : new Color(1, 0, 0, 0.5f);
            if (visualArea != null) visualArea.color = canPlace ? new Color(0, 1, 0, 0.4f) : new Color(1, 0, 0, 0.4f);
        }

        void Update() { UpdateVisual(); }

    }
}
