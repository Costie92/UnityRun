using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public abstract class ObstacleCtrl : MonoBehaviour,IChildModel
    {
        ObstacleST obstacleST;
        public virtual void FromChildOnCollisionEnter(GameObject child, Collision coll)
        {

        }
 

        public virtual void FromChildOnTriggerEnter(GameObject child, Collider other)
        {
           
        }

        private void Awake()
        {
            obstacleST.beenHit = false;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}