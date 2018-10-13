using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public abstract class ItemCtrl : MonoBehaviour
    {
        IObjToCharactor objToCharactor;
        Transform playerTr;
        
        protected ItemST itemST;
        // Use this for initialization
        protected virtual void Awake()
        {
            itemST = new ItemST();
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
            objToCharactor = playerTr.gameObject.GetComponent<IObjToCharactor>();
        }
        
        protected  void OnCollisionEnter(Collision collision)
        { print("아이템 콜리전");
            objToCharactor.GetItem(itemST);
           
            this.gameObject.SetActive(false);
            
        }
        protected  void OnTriggerEnter(Collider other)
        {   print("아이템 트리거 콜리전");
            objToCharactor.GetItem(itemST);
           
            this.gameObject.SetActive(false);
          
        }

        protected virtual void OnDisable()
        {
           // this.gameObject.transform.SetParent(null);
        }
        
    }
}