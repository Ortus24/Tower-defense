using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.TowerBuilding.EconomyTower
{
    public class ResourceSpot :MonoBehaviour
    {
        public ResourceType myType; // Chọn GoldMine cho cục vàng, Tree cho cái cây

        // Hàm này để khi xây xong thì ẩn cục đá đi (nhìn cho đỡ vướng)
        public void Occupy()
        {
            gameObject.SetActive(false); // Hoặc đổi sprite thành sprite đã khai thác
        }

    }
}
