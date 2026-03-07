using Assets.Script.TowerBuilding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Script.Inventory___Shop
{
    public class ResourceDisplayUI : MonoBehaviour
    {
        [Header("UI Text References")]
        public TextMeshProUGUI goldTotalText;
        public TextMeshProUGUI woodTotalText;
        public TextMeshProUGUI monsterDropTotalText;

        private void Update()
        {
            // Lấy dữ liệu từ ResourceManager (Vàng và Gỗ)
            if (ResourceManager.main != null)
            {
                if (goldTotalText != null)
                    goldTotalText.text = ResourceManager.main.CurrentGold.ToString();

                if (woodTotalText != null)
                    woodTotalText.text = ResourceManager.main.CurrentWood.ToString();
            }

            // Lấy dữ liệu từ InventoryManager (Monster Drop)
            if (InventoryManager.instance != null)
            {
                if (monsterDropTotalText != null)
                    monsterDropTotalText.text = InventoryManager.instance.monsterDropTotal.ToString();
            }
        }
    }
}
