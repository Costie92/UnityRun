using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public class FireCtrl : ObstacleCtrl
    {
        public override void FromChildOnTriggerEnter(GameObject child, Collider other)
        {
            base.FromChildOnTriggerEnter(child, other);
         
            if (other.gameObject.CompareTag("PLAYER"))
            {
                Debug.Log("FIRE 장애물 트리거엔터 이벤트");
                obsST.beenHit = true;
                objToCharactor.BeenHitByObs(obsST);
            }
        }
        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            obsST.obstacleType = E_OBSTACLE.FIRE;
        }
    }
}