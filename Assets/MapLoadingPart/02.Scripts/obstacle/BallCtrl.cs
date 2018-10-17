using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public class BallCtrl : ObstacleCtrl
    {
        public float moveSpeed = 0.1f;
        Vector3 n = new Vector3(15, 0, 0);

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            obsST.obstacleType = E_OBSTACLE.BALL;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        void Update()
        {
            childModel.transform.Rotate(n, Space.Self);
            transform.Translate(Vector3.forward * moveSpeed, Space.Self);
        }
      
        public override void FromChildOnTriggerEnter(GameObject child, Collider other)
        {
            base.FromChildOnTriggerEnter(child, other);
           
            if (other.gameObject.CompareTag("PLAYER")&&!obsST.beenHit)
            {
                Debug.Log("볼 장애물 플레이어 피격");
                obsST.beenHit = true;
                objToCharactor.BeenHitByObs(obsST);
            }
        }
    }
};