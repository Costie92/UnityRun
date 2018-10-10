using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ForChildModel : MonoBehaviour  {

    private void OnCollisionEnter(Collision collision)
    {
        transform.parent.gameObject.GetComponent<IChildModel>().FromChildOnCollisionEnter(collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.gameObject.GetComponent<IChildModel>().FromChildOnTriggerEnter(other);
    }

}
