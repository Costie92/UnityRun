using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp {
    public class BossBreathCtrl : MonoBehaviour {
        public ParticleSystem[] particles;


        ObstacleST obsST = new ObstacleST();
        IObjToCharactor objToCharactor;

        private void Awake()
        {
            particles = new ParticleSystem[4];
            obsST.obstacleType = E_OBSTACLE.BOSS_BREATH;
            obsST.beenHit = true;

            Transform pg = transform.GetChild(0);
            print(pg.name+"카운트"+pg.childCount);
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