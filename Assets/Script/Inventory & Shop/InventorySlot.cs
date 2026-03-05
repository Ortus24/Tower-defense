using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Inventory___Shop
{
    public class InventorySlot : MonoBehaviour
    {
        public ItemSO item;
        public int quantity;
        public Image itemImage;
        public TMP_Text quantityText;

        public void UpdateUI()
        {
            if (item != null)
            {
                itemImage.sprite = item.itemIcon;
                itemImage.gameObject.SetActive(true);
                quantityText.text = quantity >= 1 ? quantity.ToString() : "";
            }
            else
            {
                itemImage.gameObject.SetActive(false);
                quantityText.text = "";
            }

        }

    }
}
