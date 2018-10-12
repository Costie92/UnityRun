using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class ItemCoinCtrl : ItemCtrl
    {
        protected override void Awake()
        {
            base.Awake();
            itemST.itemType = E_ITEM.COIN;
            itemST.value = 1;
        }
    }
}