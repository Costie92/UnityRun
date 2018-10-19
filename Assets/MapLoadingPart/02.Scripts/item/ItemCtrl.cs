using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public abstract class ItemCtrl : MonoBehaviour
    {
        IObjToCharactor objToCharactor;
        Transform playerTr;
        GameObject childModel;
        
        protected ItemST itemST;
        // Use this for initialization
        protected virtual void Awake()
        {
            itemST = new ItemST();
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
            objToCharactor = playerTr.gameObject.GetComponent<IObjToCharactor>();
            childModel = transform.Find("childModel").gameObject;
        }
        
       
        protected  void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PLAYER"))
            {
                Debug.Log(itemST.itemType + "아이템 트리거 콜리전");
                objToCharactor.GetItem(itemST);


                this.gameObject.SetActive(false);
            }
        }

        protected virtual void OnDisable()
        {
           // this.gameObject.transform.SetParent(null);
        }
    }
}