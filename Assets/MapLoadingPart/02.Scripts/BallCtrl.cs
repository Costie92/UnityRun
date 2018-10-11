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
            transform.Translate(Vector3.forward * moveSpeed, Space.Self);
            if (transform.position.z < playerTr.position.z - 10)
                MapAndObjPool.instance.TurnInPoolObj(this.gameObject);
        }


        public override void FromChildOnTriggerEnter(GameObject child, Collider other)
        {
            print("오버라이드---자식모델링에서 콜리전받음");
            if (other.gameObject.CompareTag("PLAYER"))
            {
               
                print(gameObject.name + "트리거 콜리전!!!!!!!!!!!!");
                objToCharactor.BeenHitByObs(obsST);
            }
        }

        public override void FromChildOnCollisionEnter(GameObject child, Collision collision)
        {
            print("오버라이드---자식모델링에서 트리거받음");
            if (collision.gameObject.CompareTag("PLAYER"))
            {
                
                print(gameObject.name + "콜리전!!!!!!!!!!!!");
                objToCharactor.BeenHitByObs(obsST);
            }
        }
    }
};