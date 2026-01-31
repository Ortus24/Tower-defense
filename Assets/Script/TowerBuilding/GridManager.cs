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

        // Sử dụng HashSet để lưu trữ tọa độ các ô đã bị chiếm (hiệu năng cao hơn List)
        private HashSet<Vector2Int> occupiedNodes = new HashSet<Vector2Int>();

        // Nếu bạn muốn tháp tự khớp vào Tilemap, hãy gán Grid của Scene vào đây
        public Grid gridSystem;

        void Awake()
        {
            if (main == null) main = this;
            else Destroy(gameObject);
        }

        /// <summary>
        /// Kiểm tra xem một vùng diện tích (size) bắt đầu từ vị trí (startPos) có trống không.
        /// </summary>
        public bool IsAreaEmpty(Vector2Int startPos, Vector2Int size)
        {
            // Duyệt qua từng ô trong phạm vi kích thước của tháp (Ví dụ: 2x2 hoặc 2x3)
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int nodeToCheck = startPos + new Vector2Int(x, y);

                    // Nếu bất kỳ ô nào trong vùng này đã bị chiếm, trả về false
                    if (occupiedNodes.Contains(nodeToCheck))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Đánh dấu một vùng diện tích đã bị tháp chiếm đóng sau khi xây dựng thành công.
        /// </summary>
        public void OccupyArea(Vector2Int startPos, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int nodeToOccupy = startPos + new Vector2Int(x, y);
                    if (!occupiedNodes.Contains(nodeToOccupy))
                    {
                        occupiedNodes.Add(nodeToOccupy);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            // 1. Vẽ tất cả các ô đã bị chiếm đóng (Màu đỏ)
            if (occupiedNodes != null)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.4f); // Màu đỏ trong suốt
                foreach (var node in occupiedNodes)
                {
                    // Vẽ hình lập phương tại tâm ô (cộng 0.5f vì tọa độ Int nằm ở góc ô)
                    Vector3 center = new Vector3(node.x + 0.5f, node.y + 0.5f, 0);
                    Gizmos.DrawCube(center, new Vector3(0.9f, 0.9f, 0.1f));
                }
            }

            // 2. Tùy chọn: Vẽ một lưới mờ để dễ căn chỉnh
            Gizmos.color = new Color(1f, 1f, 1f, 0.1f);
            for (int x = -20; x < 20; x++) // Bạn có thể thay đổi phạm vi tùy theo map
            {
                for (int y = -20; y < 20; y++)
                {
                    Vector3 pos = new Vector3(x + 0.5f, y + 0.5f, 0);
                    Gizmos.DrawWireCube(pos, Vector3.one);
                }
            }
        }
    }
}
