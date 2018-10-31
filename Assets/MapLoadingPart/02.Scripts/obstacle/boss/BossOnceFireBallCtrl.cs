using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class BossOnceFireBallCtrl : MonoBehaviour
    {
        ParticleSystem particle;
        ObstacleST obsST;
        IObjToCharactor objToCharactor;
        Transform playerTr;
        float moveSpeed = 10.0f;

        private void Awake()
        {
            obsST.obstacleType = E_OBSTACLE.BOSS_FIREBALL;
            obsST.beenHit = true;
            objToCharactor = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<IObjToCharactor>();
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
            particle = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        }
        

        private void OnEnable()
        {
            transform.LookAt(playerTr.position);
            particle.Play();
        }
        private void OnDisable()
        {
            particle.Stop();
        }
        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PLAYER"))
            {
                objToCharactor.BeenHitByObs(obsST);
            }
        }
    }
}