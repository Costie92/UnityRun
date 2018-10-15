using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class MapPathGenerator : SingletonTemplate<MapPathGenerator>
    {
        int frontShowChunk;
        float margin;
        public float turnProbability=50;   //회전 확률
        int turningTerm = 10;

        public float GetTurnProbability() { return turnProbability; }
        public void SetGetTurnProbability(float value)
        {
            if (value >= 100) turnProbability = 100;
            else
            {
                turnProbability = value;
            }
        }


        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            frontShowChunk = MapObjManager.GetInstance().wantToShowNumOfChunks 
                - MapObjManager.GetInstance().wantToShowNumOfChunksInBehind;
        }
        private void Start()
        {
            margin = MapObjManager.GetInstance().GetChunkMargin();
        }

        public float WillTurn(float nowPos,float turnedPoint , out E_WhichTurn whichTurn,out Vector3 turnChunksPos)  //터닝포인트 (10단위 위치) 리턴
         {
            whichTurn = E_WhichTurn.NOT_TURN;
            turnChunksPos = Vector3.zero;
            Debug.Log("터닝텀은 = "+(turnedPoint + turningTerm * MapObjManager.GetInstance().GetChunkMargin()));
            if (turnedPoint + turningTerm * MapObjManager.GetInstance().GetChunkMargin() > nowPos) //터닝 텀.
                return 0;

            float turning = Random.Range(0f, 100f);   //방향전환확률
            if (turning >turnProbability) return 0;  //방향 전환 없음

            else //방향전환 있음
            {
                print("터닝 플래그 온");

                float makePos = nowPos + frontShowChunk * margin;
                float turningPoint = makePos+(2 * margin);
                turnChunksPos = new Vector3(0, 0, makePos);

                print("생성위치 ="+ turnChunksPos + "터닝포인트="+turningPoint);

                if(Random.Range(0,2)==0)    //0이면 왼쪽
                {
                    Debug.Log("왼쪽 결정");
                    whichTurn = E_WhichTurn.LEFT;
                    //왼쪽 기역 청크 생성
                }
                else
                {
                    Debug.Log("오른쪽 결정");
                    whichTurn = E_WhichTurn.RIGHT;
                    //오른쪽 청크 생성
                }
                return turningPoint;
            }
        }
    }
}