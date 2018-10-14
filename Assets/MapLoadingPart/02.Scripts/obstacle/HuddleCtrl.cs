using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public class HuddleCtrl : ObstacleCtrl
    {
        public override void FromChildOnCollisionEnter(GameObject child, Collision coll)
        {

        }

        public override void FromChildOnTriggerEnter(GameObject child, Collider other)
        {

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