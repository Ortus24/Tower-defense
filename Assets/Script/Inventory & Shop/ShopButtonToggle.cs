using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Inventory___Shop
{
    public class ShopButtonToggle : MonoBehaviour
    {
        public void OpenItemShop()
        {
            if (Shopkeeper.currentShopkeeper != null)
                Shopkeeper.currentShopkeeper.OpenItemShop();
        }

        public void OpenWeaponShop()
        {
            if (Shopkeeper.currentShopkeeper != null)
                Shopkeeper.currentShopkeeper.OpenWeaponShop();
        }

        public void OpenArmorShop()
        {
            if (Shopkeeper.currentShopkeeper != null)
                Shopkeeper.currentShopkeeper.OpenArmorShop();
        }
    }
}