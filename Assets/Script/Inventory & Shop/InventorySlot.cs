using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Script.Inventory___Shop
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        public ItemSO item;
        public int quantity;
        public Image itemImage;
        public TMP_Text quantityText;

        private InventoryManager inventoryManager;

        private void Start()
        {
            // Tự động tìm Manager ở node cha
            inventoryManager = GetComponentInParent<InventoryManager>();
        }

        public void UpdateUI()
        {
            if (item != null && quantity > 0)
            {
                itemImage.sprite = item.itemIcon;
                itemImage.gameObject.SetActive(true);
                quantityText.text = quantity > 1 ? quantity.ToString() : "";
            }
            else
            {
                itemImage.gameObject.SetActive(false);
                quantityText.text = "";
                item = null;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Click chuột trái để sử dụng
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (item != null && quantity > 0)
                {
                    inventoryManager.UseItem(this);
                }
            }
        }
    }
}
