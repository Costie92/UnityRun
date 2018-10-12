using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp {
    public class ItemHpCtrl : ItemCtrl
    {
        protected override void Awake() {
            base.Awake();
            itemST.itemType = E_ITEM.HPPLUS;
            itemST.value = 1;
        }
    }
}