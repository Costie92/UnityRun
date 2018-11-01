using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public abstract class ItemCtrl : MonoBehaviour
    {
        protected IObjToCharactor objToCharactor;
        MeshRenderer childModelRenderer;
        GameObject childModel;
        
        protected ItemST itemST;

        protected virtual void Awake()
        {
            itemST = new ItemST();
            childModel = transform.Find("childModel").gameObject;
            objToCharactor = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<IObjToCharactor>();
            childModelRenderer = transform.Find("childModel").gameObject.GetComponent<MeshRenderer>();
        }
        
        protected  void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PLAYER"))
            {
                objToCharactor.GetItem(itemST);

                childModelRenderer.enabled = false;
                childModel.GetComponent<Collider>().enabled = false;
            }
        }
        protected virtual void OnEnable() {
            if(childModelRenderer.enabled.Equals(false))
            {
                childModelRenderer.enabled = true;
                childModel.GetComponent<Collider>().enabled = true;
            }
        }
    }
}