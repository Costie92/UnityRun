﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class TurnPartInCharge : SingletonTemplate<TurnPartInCharge>
    {
        public GameObject toLeftChunks;
        public GameObject toRightChunks;

        private GameObject leftTurnChunks;
        private GameObject rightTurnChunks;
        public Transform whereToTurn;
        Quaternion originRot;
        E_WhichTurn whichTurn;

        protected override void Awake()
        {
            base.Awake();
            //여기서 간단하게 풀링
            GameObject turnChunksParent = new GameObject("turnChunksParent");
            leftTurnChunks = Instantiate<GameObject>(toLeftChunks,turnChunksParent.transform);
            rightTurnChunks= Instantiate<GameObject>(toRightChunks, turnChunksParent.transform);
            whereToTurn = leftTurnChunks.transform.Find("TurningPoint");
            originRot = whereToTurn.transform. rotation;
            whereToTurn = null;
            leftTurnChunks.SetActive(false);
            rightTurnChunks.SetActive(false);
        }


        public void GenerateTurnChunks(E_WhichTurn whichTurn, Vector3 pos)
        {
            this.whichTurn = whichTurn;
            if(whichTurn==E_WhichTurn.LEFT)
            {
                whereToTurn = leftTurnChunks.transform.Find("TurningPoint");
                leftTurnChunks.transform.position = pos;
                whereToTurn.transform.rotation = originRot;
                leftTurnChunks.SetActive(true);
                
            }
            else if(whichTurn==E_WhichTurn.RIGHT)
            {
                whereToTurn = rightTurnChunks.transform.Find("TurningPoint");
                rightTurnChunks.transform.position = pos;
                whereToTurn.transform.rotation = originRot;
                rightTurnChunks.SetActive(true);
               
            }
            else { Debug.Log("턴파트에서 오류001"); }

        }

        public void Turn()
        {
            StartCoroutine(turnMap(whichTurn));
        }

        public void Reset()
        {
            whichTurn = E_WhichTurn.NOT_TURN;
            whereToTurn = null;
            leftTurnChunks.SetActive(false);
            rightTurnChunks.SetActive(false);
        }

        IEnumerator turnMap(E_WhichTurn whichTurn)
        {
            if (whereToTurn == null) yield break;
            if (whichTurn == E_WhichTurn.LEFT)
            {
                while (whereToTurn != null && whereToTurn.eulerAngles.y < 90)   //다 돌기전에 지나가버리는 사태를 대비해서whereToTurn != null 조건문 추가
                {
               
                    whereToTurn.Rotate(0.0f, 15f, 0.0f);
                    yield return new WaitForSeconds(Mathf.Epsilon * 1f);
                }
            }
            else if(whichTurn == E_WhichTurn.RIGHT)
            {
                whereToTurn.Rotate(0.0f, -0.01f, 0.0f);//쿼터니언과 수 부분
                while (whereToTurn != null&& whereToTurn.eulerAngles.y >= 270)
                {
                  
                    // print(turnChunks.transform.eulerAngles.y);
                    whereToTurn.Rotate(0.0f, -15f, 0.0f);
                    yield return new WaitForSeconds(Mathf.Epsilon * 1f);
                }
            }
        }
    }
}