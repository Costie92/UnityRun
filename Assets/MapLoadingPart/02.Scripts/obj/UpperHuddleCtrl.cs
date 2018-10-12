using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp {
    public class UpperHuddleCtrl :  ObstacleCtrl{
        public override void FromChildOnCollisionEnter(GameObject child, Collision coll)
        {

        }

        public override void FromChildOnTriggerEnter(GameObject child, Collider other)
        {

        }
        protected override void Awake()
        {
            base.Awake();
            obsST.obstacleType = E_OBSTACLE.UPPER_HUDDLE;
        }

    }
}
