using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp {
    public class BossMeteor : MonoBehaviour {
        ParticleSystem[] particles;

        GameObject meteor;

        Transform playerTr;
        private void Awake()
        {

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
            meteor.transform.position= Vector3.up*20.0f;
        }
        private void OnDisable()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Stop();
            }
        }
        private void Update()
        {
            if (playerTr.position.z + 10f < transform.position.z)
            {
                MeteorKaboom();
            }
        }
        void MeteorKaboom()
        {
            if (meteor.transform.position.y > 0)
            {
                meteor.transform.Translate(Vector3.down * Time.deltaTime * 30.0f, Space.Self);
            }

        }

        
    }
}