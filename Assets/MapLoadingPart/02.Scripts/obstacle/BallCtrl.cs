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
            transform.LookAt(playerTr, Vector3.up);
        }

        void Update()
        {
            childModel.transform.Rotate(n, Space.Self);
            /*
            transform.Translate(Vector3.forward * moveSpeed, Space.Self);
            if (transform.position.z < playerTr.position.z - 10)
                MapAndObjPool.GetInstance().TurnInPoolObj(this.gameObject);
                */
                
        }
        public override void FromChildOnCollisionEnter(GameObject child, Collision collision)
        {
            base.FromChildOnCollisionEnter(child, collision);
            Debug.Log("볼 장애물 콜리전엔터 이벤트");
            if (collision.gameObject.CompareTag("PLAYER"))
            {
                objToCharactor.BeenHitByObs(obsST);
                
            }
            obsST.beenHit = false;
        }


        public override void FromChildOnTriggerEnter(GameObject child, Collider other)
        {
            base.FromChildOnTriggerEnter(child, other);
            Debug.Log("볼 장애물 트리거엔터 이벤트");
            if (other.gameObject.CompareTag("PLAYER"))
            {
                objToCharactor.BeenHitByObs(obsST);
                
            }
            obsST.beenHit = false;
        }

     
    }
};