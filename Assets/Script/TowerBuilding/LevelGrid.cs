using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.TowerBuilding
{
    public class LevelGrid
    {
        private int width;
        private int height;
        private float cellSize;
        private int[,] gridArray;
        private Vector3 originPosition;

        public LevelGrid(int width, int height, float cellSize, Vector3 originPosition)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;
            gridArray = new int[width, height];

            // Vẽ lưới màu trắng (Code cũ của bạn)
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + originPosition;
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }

        public void SetValue(int x, int y, int value)
        {
            if (IsValid(x, y)) gridArray[x, y] = value;
        }

        public int GetValue(int x, int y)
        {
            if (IsValid(x, y)) return gridArray[x, y];
            return -1;
        }

        public bool IsValid(int x, int y)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
        }

        // --- THÊM 2 HÀM NÀY ĐỂ GRID MANAGER CÓ THỂ DUYỆT QUA ---
        public int GetWidth() { return width; }
        public int GetHeight() { return height; }
    }
}
