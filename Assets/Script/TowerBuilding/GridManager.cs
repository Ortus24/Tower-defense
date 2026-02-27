using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.TowerBuilding
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager main;

        [Header("Cài đặt Lưới")]
        public int width = 20;
        public int height = 15;
        public float cellSize = 1f;
        public Vector3 originPosition = Vector3.zero;

        private LevelGrid levelGrid;

        private void Awake()
        {
            if (main == null) main = this;
            
        }

        private void Start()
        {
            levelGrid = new LevelGrid(width, height, cellSize, originPosition);
        }

        public LevelGrid GetLevelGrid() { return levelGrid; }

        public bool IsAreaEmpty(Vector2Int startGridPos, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (levelGrid.GetValue(startGridPos.x + x, startGridPos.y + y) != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void OccupyArea(Vector2Int startGridPos, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    levelGrid.SetValue(startGridPos.x + x, startGridPos.y + y, 1);
                }
            }
        }

        // --- THÊM HÀM NÀY VÀO ĐỂ GIẢI PHÓNG Ô ĐẤT ---
        public void FreeArea(Vector2Int startPos, Vector2Int size)
        {
            // Duyệt qua tất cả các ô lưới mà tháp này đang chiếm dụng
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    int checkX = startPos.x + x;
                    int checkY = startPos.y + y;

                    // Vì ở hàm OccupyArea bạn dùng SetValue là 1 (đã chiếm)
                    // Nên ở đây giải phóng đất, ta chỉ cần SetValue về 0 (đất trống)
                    levelGrid.SetValue(checkX, checkY, 0);
                }
            }
        }

        // --- ĐOẠN CODE MỚI ĐỂ HIỂN THỊ Ô ĐÃ CHIẾM ---
        private void OnDrawGizmos()
        {
            // Chỉ vẽ khi game đang chạy và Grid đã được tạo
            if (levelGrid == null) return;

            Gizmos.color = new Color(1, 0, 0, 0.3f); // Màu đỏ trong suốt (Alpha = 0.3)

            for (int x = 0; x < levelGrid.GetWidth(); x++)
            {
                for (int y = 0; y < levelGrid.GetHeight(); y++)
                {
                    // Nếu ô này có giá trị (tức là = 1) -> Vẽ ô vuông đỏ
                    if (levelGrid.GetValue(x, y) == 1)
                    {
                        // Tính toán tâm của ô vuông để vẽ Gizmos cho chuẩn
                        // GetWorldPosition trả về góc dưới trái, nên phải cộng thêm nửa ô
                        Vector3 centerPos = levelGrid.GetWorldPosition(x, y) + new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

                        Gizmos.DrawCube(centerPos, new Vector3(cellSize, cellSize, 0.1f));
                    }
                }
            }
        }
    }
}
