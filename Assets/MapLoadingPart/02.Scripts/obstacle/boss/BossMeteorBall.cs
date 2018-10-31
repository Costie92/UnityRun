using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public class BossMeteorBall : MonoBehaviour
    {
        public ParticleSystem particle;
        ObstacleST obsST = new ObstacleST();
        IObjToCharactor objToCharactor;

        private void Awake()
        {
            obsST.obstacleType = E_OBSTACLE.BOSS_METEOR;
            obsST.beenHit = true;
            objToCharactor = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<IObjToCharactor>();
            particle = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        }
        private void OnEnable()
        {
            particle.Play();
        }
        private void OnDisable()
        {
            particle.Stop();
        }
        private void OnTriggerEnter(Collider other)
        {
            objToCharactor.BeenHitByObs(obsST);
        }
    }
}