using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public abstract class ItemCtrl : MonoBehaviour
    {
        IObjToCharactor objToCharactor;
        MeshRenderer childModelRenderer;
        
        protected ItemST itemST;

        protected virtual void Awake()
        {
            itemST = new ItemST();

            objToCharactor = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<IObjToCharactor>();
            childModelRenderer = transform.Find("childModel").gameObject.GetComponent<MeshRenderer>();
        }
        
        protected  void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PLAYER"))
            {
                objToCharactor.GetItem(itemST);

                childModelRenderer.enabled = false;
            }
        }
        protected virtual void OnEnable() {
            if(childModelRenderer.enabled.Equals(false))
            {
                childModelRenderer.enabled = true;
            }
        }
    }
}