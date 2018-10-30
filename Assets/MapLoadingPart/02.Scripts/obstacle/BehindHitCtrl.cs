using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp {
   

public class BehindHitCtrl : MonoBehaviour {

        public static GameObject[] dangerZone = new GameObject[3];  //위험 표시해줄 물체
        //느낌표 같은 느낌으로 불투명하게
        
        Transform playerTr;
        bool playerFound = false;
        float dangerDistance=15f;
        WaitForSeconds ws = new WaitForSeconds(0.2f);
        int mySpawnLine = -1;
        ObstacleST obsST;
        IObjToCharactor objToCharactor;

        private void Awake()
        {
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
            //obsST.obstacleType  //뒤 장애물 옵스타클 이넘 선언하기
            obsST.beenHit = true;
            objToCharactor = playerTr.gameObject.GetComponent<IObjToCharactor>();
        }

        void Start () {
           
        }
        private void Update()
        {
            if (playerFound)
            {
                //위험 처리 해주기
               
                ShowDanger(); 
                
                //실린더 박는 처리 해주기.

                if (playerTr.position.z > transform.position.z)
                {
                    dangerZone[mySpawnLine].SetActive(false);
                    playerFound = false;
                }
            }
        }

        void ShowDanger()
        {
            if (dangerZone[mySpawnLine].activeSelf==true) return;

            switch (mySpawnLine)
            {
                //dangerZone 을 활성화시키는 역할
                case 0:
                    dangerZone[0].SetActive(true);
                    break;
                case 1:
                    dangerZone[1].SetActive(true);
                    break;
                case 2:
                    dangerZone[2].SetActive(true);
                    break;
            }
        }


        private void OnEnable()
        {
            StartCoroutine(CheckPlayerPos());
        }
        IEnumerator CheckPlayerPos()
        {
            while (playerTr.position.z + dangerDistance < transform.position.z)
                yield return ws;
            playerFound = true;
            CheckMySpawnLine();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            playerFound = false;
            mySpawnLine = -1;
        }

        void CheckMySpawnLine()
        {
            float x = transform.position.x;

            if (x < 1)
            {
                mySpawnLine = 0;
                return;
            }

            if (x < 5)
            {
                mySpawnLine = 1;
                return;
            }
            if (x < 9)
            {
                mySpawnLine = 2;
                return;
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
