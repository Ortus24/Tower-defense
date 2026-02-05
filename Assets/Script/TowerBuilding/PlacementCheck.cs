using Assets.Script.TowerBuilding.EconomyTower;
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

        // Biến lưu trữ mỏ quặng tìm thấy (nếu có)
        [HideInInspector] public ResourceSpot currentValidSpot;

        void Start() { sr = GetComponentInChildren<SpriteRenderer>(); }

        public bool CanPlace()
        {
            currentValidSpot = null; // Reset mỗi lần check

            if (GridManager.main == null) return false;

            // 1. Check Tiền
            if (ResourceManager.main != null)
            {
                if (!ResourceManager.main.HasEnoughResources(data.goldCost, data.woodCost)) return false;
            }

            float cellSize = GridManager.main.cellSize;

            // =================================================================
            // TRƯỜNG HỢP 1: THÁP CHIẾN ĐẤU (NONE) - KHÔNG ĐƯỢC ĐÈ LÊN MỎ
            // =================================================================
            if (data.resourceType == ResourceType.None)
            {
                // Bước A: Check Grid xem có tháp nào xây chưa (Code cũ)
                Vector3 originWorldPos = transform.position - new Vector3(data.towerSize.x * cellSize * 0.5f, data.towerSize.y * cellSize * 0.5f, 0);
                int gridX, gridY;
                GridManager.main.GetLevelGrid().GetXY(originWorldPos + new Vector3(0.1f, 0.1f), out gridX, out gridY);

                if (!GridManager.main.IsAreaEmpty(new Vector2Int(gridX, gridY), data.towerSize))
                {
                    return false; // Grid đã bị chiếm -> Không xây được
                }

                // Bước B: Check Vật Lý (Code Mới) - Quét xem có vướng đá/cây không
                // Tạo vùng quét đúng bằng kích thước tháp (nhỏ hơn 1 chút để không bị dính mép)
                Vector2 boxSize = new Vector2(data.towerSize.x * cellSize * 0.9f, data.towerSize.y * cellSize * 0.9f);

                Collider2D[] obstacles = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);

                foreach (var col in obstacles)
                {
                    // Nếu va phải bất kỳ cái gì có gắn script ResourceSpot -> CẤM XÂY
                    if (col.GetComponent<ResourceSpot>() != null)
                    {
                        return false;
                    }
                }

                return true; // Grid trống và không vướng đá -> OK
            }

            // =================================================================
            // TRƯỜNG HỢP 2: XÂY MỎ (GOLD/TREE) - PHẢI ĐÈ LÊN ĐÚNG LOẠI
            // =================================================================
            else
            {
                // Quét vùng rộng 3x3 để tìm mỏ
                Vector2 scanAreaSize = new Vector2(cellSize * 3f, cellSize * 3f);
                Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, scanAreaSize, 0f);

                foreach (var hit in hits)
                {
                    ResourceSpot spot = hit.GetComponent<ResourceSpot>();

                    if (spot != null && spot.myType == data.resourceType)
                    {
                        currentValidSpot = spot;
                        return true;
                    }
                }

                return false;
            }
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
