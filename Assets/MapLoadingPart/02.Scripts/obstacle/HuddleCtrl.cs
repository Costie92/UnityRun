using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public class HuddleCtrl : ObstacleCtrl
    {
        bool beenHitByObs = false;
        bool beenHitByPlayer = false;

        float rotationX = 0;
        public override void FromChildOnTriggerEnter(GameObject child, Collider other)
        {
            base.FromChildOnTriggerEnter(child, other);
           
            if (other.gameObject.CompareTag("PLAYER"))
            {
             //   Debug.Log("허들 플레이어 피격");
                obsST.beenHit = true;
                if(!beenHitByObs)beenHitByPlayer = true;
                objToCharactor.BeenHitByObs(obsST);
            }
            if (other.gameObject.CompareTag("OBSTACLE") || (other.transform.parent != null && other.transform.parent.CompareTag("OBSTACLE")))
            {
               if(!beenHitByPlayer) beenHitByObs = true;
             //   Debug.Log("허들 볼에 피격");
            }
            childModel.GetComponent<Collider>().enabled = false;
        }

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            obsST.obstacleType = E_OBSTACLE.HUDDLE;
        }

        private void Update()
        {
            if (beenHitByObs)
            {
                if (transform.eulerAngles.x < 90)
                    transform.Rotate(5f, 0f, 0f);
            }
            if (beenHitByPlayer)
            {
                if (rotationX >= -70)
                {
                    rotationX = rotationX - 5;

                    transform.rotation = Quaternion.Euler(rotationX, - 180, 0);
                    transform.Translate(Vector3.up*0.014f, Space.World);
                }
            }
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            beenHitByObs = false;
            beenHitByPlayer = false;
            rotationX = 0;
        }

    }
};