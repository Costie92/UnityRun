using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public class FireCtrl : MonoBehaviour
    {
        [SerializeField]
        ParticleSystem fireParticle;
        IObjToCharactor objToCharactor;
        ObstacleST obsST;
        Collider myCollider;

        void Awake()
        {
            obsST = new ObstacleST();
            obsST.beenHit = true;
            obsST.obstacleType = E_OBSTACLE.FIRE;
            fireParticle = transform.Find("FireParticle").gameObject.GetComponent<ParticleSystem>();
            objToCharactor = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<IObjToCharactor>();
            myCollider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            fireParticle.Play();
        }
        private void OnDisable()
        {
            if (myCollider.enabled == false)
                myCollider.enabled = true;
            fireParticle.Stop();
        }

        private void OnTriggerEnter(Collider other)
        {
              if (other.gameObject.CompareTag("PLAYER"))
            {
                Debug.Log("FIRE 장애물 트리거엔터 이벤트");
                obsST.beenHit = true;
                myCollider.enabled = false;
                objToCharactor.BeenHitByObs(obsST);
                
            }
        }

    }
}