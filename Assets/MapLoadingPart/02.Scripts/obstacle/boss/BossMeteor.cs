using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp {
    public class BossMeteor : MonoBehaviour,IOnlyBossPatternMove {
        public ParticleSystem[] particles ;

        GameObject meteor;

        Transform playerTr;
        private void Awake()
        {
            particles = new ParticleSystem[4];
            Transform pg = transform.GetChild(0);
            for (int i = 0; i < pg.childCount; i++)
            {
                particles[i] = pg.GetChild(i).gameObject.GetComponent<ParticleSystem>();
            }
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
            meteor = transform.Find("Meteor").gameObject;
        }

        private void OnEnable()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
            meteor.transform.Translate(Vector3.up * 20f);
        }
        private void OnDisable()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Stop();
            }
            meteor.transform.position = transform.position ;
        }
        private void Update()
        {
            if (playerTr.position.z + 20f > transform.position.z)
            {
                MeteorKaboom();
            }
        }
        void MeteorKaboom()
        {
            if (meteor.transform.position.y > 0)
            {
                meteor.transform.Translate(Vector3.down * Time.deltaTime * 100.0f, Space.Self);
            }
            else {
                meteor.transform.position = transform.position;
            }

        }

        public void MoveAtOwnSpeed()
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 20f);
        }
    }
}