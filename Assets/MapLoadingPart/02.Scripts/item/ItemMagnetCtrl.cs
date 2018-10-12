using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class ItemMagnetCtrl : ItemCtrl
    {
        protected override void Awake()
        {
            base.Awake();
            itemST.itemType = E_ITEM.MAGNET;
            itemST.value = 1;
        }
    }
}
