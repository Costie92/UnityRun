using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class ItemCoinCtrl : ItemCtrl
    {
        Transform playerTr;
        bool magneted = false;
        protected override void Awake()
        {
            base.Awake();
            itemST.itemType = E_ITEM.COIN;
            itemST.value = 1;

        }
        private void Start()
        {
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
        }
        private void Update()
        {
            if (!magneted && transform.position.z <= playerTr.position.z + 15.0f && objToCharactor.GetMagnetState())
                magneted = true;

            if(magneted)
                transform.position = Vector3.Lerp(transform.position, playerTr.position,Time.deltaTime*50f);
        }
        private void OnDisable()
        {
            magneted = false;
        }

    }
}