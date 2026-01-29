using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.TowerBuilding
{
    public class GridRenderer : MonoBehaviour
    {
        public float cellSize = 0.64f; // Kích thước ô (tương ứng 64x64)
        public int width = 100; // Số ô theo chiều ngang
        public int height = 100; // Số ô theo chiều dọc
        public Color gridColor = new Color(1, 1, 1, 0.2f); // Màu trắng mờ

        private void OnDrawGizmos() // Hiện trong Scene để bạn căn chỉnh
        {
            Gizmos.color = gridColor;
            for (float x = 0; x <= width * cellSize; x += cellSize)
            {
                Gizmos.DrawLine(new Vector3(x, 0, 0), new Vector3(x, height * cellSize, 0));
            }
            for (float y = 0; y <= height * cellSize; y += cellSize)
            {
                Gizmos.DrawLine(new Vector3(0, y, 0), new Vector3(width * cellSize, y, 0));
            }
        }
    }
}
