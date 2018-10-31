using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp {
    public class BossBreathCtrl : MonoBehaviour {
        ParticleSystem[] particles;


        ObstacleST obsST;
        IObjToCharactor objToCharactor;

        private void Awake()
        {
            obsST.obstacleType = E_OBSTACLE.BOSS_BREATH;
            obsST.beenHit = true;

            Transform pg = transform.GetChild(0);
            for (int i = 0; i < pg.childCount; i++)
            {
                particles[i] = pg.GetChild(i).gameObject.GetComponent<ParticleSystem>();
            }
            objToCharactor = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<IObjToCharactor>();
        }
        private void OnEnable()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
        }
        private void OnDisable()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Stop();
            }
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