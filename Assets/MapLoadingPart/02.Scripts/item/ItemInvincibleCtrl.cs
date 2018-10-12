using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class ItemInvincibleCtrl : ItemCtrl
    {
        protected override void Awake()
        {
            base.Awake();
            itemST.itemType = E_ITEM.INVINCIBLE;
            itemST.value = 1;
        }
    }
}