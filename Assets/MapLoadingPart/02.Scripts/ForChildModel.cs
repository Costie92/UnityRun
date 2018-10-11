using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hcp
{
    public class ForChildModel : MonoBehaviour
    {
        IChildModel iChildModel;
        private void Awake()
        {
            iChildModel = transform.parent.gameObject.GetComponent<IChildModel>();
            if (iChildModel == null) Debug.Log("IChildModel 인터페이스 추출 불가"); 
        }

        private void OnCollisionEnter(Collision collision)
        {
            iChildModel.FromChildOnCollisionEnter(this.gameObject,  collision);
        }
        private void OnTriggerEnter(Collider other)
        {
            iChildModel.FromChildOnTriggerEnter(this.gameObject, other);
        }

    }
};