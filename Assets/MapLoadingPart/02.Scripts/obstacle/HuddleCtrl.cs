using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public class HuddleCtrl : ObstacleCtrl
    {
        public override void FromChildOnCollisionEnter(GameObject child, Collision coll)
        {
            base.FromChildOnCollisionEnter(child, coll);
            Debug.Log("허들 장애물 콜리전엔터 이벤트");
            if (coll.gameObject.CompareTag("PLAYER"))
            {
                objToCharactor.BeenHitByObs(obsST);
               
            }
            obsST.beenHit = false;
        }

        public override void FromChildOnTriggerEnter(GameObject child, Collider other)
        {
            base.FromChildOnTriggerEnter(child, other);
            Debug.Log("허들 장애물 트리거엔터 이벤트");
            if (other.gameObject.CompareTag("PLAYER"))
            {
                objToCharactor.BeenHitByObs(obsST);
               
            }
            obsST.beenHit = false;
        }

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            obsST.obstacleType = E_OBSTACLE.HUDDLE;
        }
        /*
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        */
        // Update is called once per frame
        void Update()
        {

        }
    }
};