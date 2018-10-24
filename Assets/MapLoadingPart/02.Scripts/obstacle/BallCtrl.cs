using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public class BallCtrl : ObstacleCtrl
    {
        public float moveSpeed = 0.1f;
        Vector3 n = new Vector3(15, 0, 0);
        bool playerFound = false;
        float margin;
        float posMargin;
        WaitForSeconds ws = new WaitForSeconds(0.3f);

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            obsST.obstacleType = E_OBSTACLE.BALL;
        }
        private void Start()
        {
             margin = MapObjManager.GetInstance().GetChunkMargin();
            posMargin = margin * 1.7f;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine( checkPlayerPos());
            playerFound = false;
            transform.position = transform.position + Vector3.up * 10.0f;
        }

        void Update()
        {
            if (playerFound)
            {
                if (Kaboom())
                {
                    childModel.transform.Rotate(n, Space.Self);
                    transform.Translate(Vector3.forward * moveSpeed, Space.Self);
                }
            }
        }

        bool Kaboom()
        {
            if (transform.position.y > 0)
            {
               
                transform.Translate(0f, -1f, 0f);
                return false;
            }
            return true;
        }
        IEnumerator checkPlayerPos()
        {
            while (true)
            {
                if (playerTr.position.z + posMargin >= transform.position.z)
                {
                    playerFound = true;
                    StopCoroutine(checkPlayerPos());
                }
                yield return ws;
            }
        }
        public override void FromChildOnTriggerEnter(GameObject child, Collider other)
        {
            if (other.gameObject.CompareTag("PLAYER")&&!obsST.beenHit)
            {
                obsST.beenHit = true;
                objToCharactor.BeenHitByObs(obsST);
            }
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            StopCoroutine(checkPlayerPos());
            playerFound = false;
        }
    }
};