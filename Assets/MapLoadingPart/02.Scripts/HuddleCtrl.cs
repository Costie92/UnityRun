using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public class HuddleCtrl : MonoBehaviour, IChildModel
    {
        ObstacleST huddleST ;
        public Material[] materials;
        private GameObject childModel; //구형 장애물 모델링 자식 추출

        public void FromChildOnCollisionEnter(GameObject child, Collision coll)
        {

        }

        public void FromChildOnTriggerEnter(GameObject child, Collider other)
        {

        }

        // Use this for initialization
        void Awake()
        {
           
            huddleST.obstacleType = E_OBSTACLE.HUDDLE;
            huddleST.beenHit = false;
            childModel = transform.Find("childModel").gameObject; //구형 장애물 모델링 자식 추출  (transform.find로 자식 중 내에서 검색)
        }

        private void OnEnable()
        {
            childModel.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }
        private void OnDisable()
        {
            huddleST.beenHit = false;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
};