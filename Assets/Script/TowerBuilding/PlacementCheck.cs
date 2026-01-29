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

        void Start() { sr = GetComponentInChildren<SpriteRenderer>(); }

        public bool CanPlace()
        {
            if (GridManager.main == null)
            {
                Debug.LogError("Chưa có GridManager trong Scene!");
                return false;
            }

            // 2. Kiểm tra TowerData đã được gán chưa
            if (data == null)
            {
                Debug.LogError("Ghost " + gameObject.name + " chưa được gán TowerData!");
                return false;
            }

            // 3. Thực hiện logic kiểm tra ô đất
            Vector2Int gridPos = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
            return GridManager.main.IsAreaEmpty(gridPos, data.towerSize);
        }
        public SpriteRenderer visualArea;
        public void UpdateVisual()
        {
            bool canPlace = CanPlace();

            // 1. Đổi màu chính tháp Ghost (Logic cũ của bạn)
            if (sr != null)
            {
                sr.color = canPlace ? new Color(1, 1, 1, 0.5f) : new Color(1, 0, 0, 0.5f);
            }

            // 2. Đổi màu ô sáng dưới chân (Xanh nếu trống, Đỏ nếu bị chiếm)
            if (visualArea != null)
            {
                // Màu xanh: canPlace đúng | Màu đỏ: canPlace sai
                visualArea.color = canPlace ? new Color(0, 1, 0, 0.4f) : new Color(1, 0, 0, 0.4f);
            }
        }

        void Update() { UpdateVisual(); }

    }
}
