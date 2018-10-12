using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class ItemShieldCtrl : ItemCtrl
    {
        protected override void Awake()
        {
            base.Awake();
            itemST.itemType = E_ITEM.SHIELD;
            itemST.value = 1;
        }
    }

    
}