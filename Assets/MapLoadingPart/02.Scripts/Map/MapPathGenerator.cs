using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class MapPathGenerator : MonoBehaviour
    {
        enum E_WhichTurn
        {
            LEFT=0,
            RIGHT,
            NOT_TURN
        }
        ChunkLoading chunkLoading;
        public GameObject toLeftChunks;
        public GameObject toRightChunks;
        GameObject turnChunks;
        bool turnFlag=false;
        E_WhichTurn whichTurn = E_WhichTurn.NOT_TURN;
        int frontShowChunk;

        float turningPoint = 0;

        // Use this for initialization
        void Awake()
        {
            chunkLoading = GetComponent<ChunkLoading>();
            frontShowChunk = chunkLoading.wantToShowNumOfChunks - chunkLoading.wantToShowNumOfChunksInBehind;
        }

        // Update is called once per frame
        void Update()
        {
            if(turnFlag)    //터닝포인트 청크에서 회전시키기.
            {
                if(AtTurningPoint())
                {
                    //방향맞게 터닝시켜주기
                    if(whichTurn==E_WhichTurn.LEFT)
                    {
                        StartCoroutine(turnMap(0));
                       // turnChunks.transform.Rotate(new Vector3(0, 90, 0));
                    }
                    else if(whichTurn == E_WhichTurn.RIGHT)
                    {
                        StartCoroutine(turnMap(1));
                      //  turnChunks.transform.Rotate(new Vector3(0, -90, 0));
                    }
                    turnFlag = false;
                    whichTurn = E_WhichTurn.NOT_TURN;
                }
            }
        }
        IEnumerator turnMap(int whichSide)
        {
            if (whichSide == 0)
            {
                while (turnChunks.transform.eulerAngles.y < 90)
                {
                   
                    turnChunks.transform.Rotate(0.0f,  15f, 0.0f);
                    yield return new WaitForSeconds(Mathf.Epsilon * 1f);
                }
            }
            else
            {
                turnChunks.transform.Rotate(0.0f, -0.01f, 0.0f);//쿼터니언과 수 부분
                while (turnChunks.transform.eulerAngles.y >= 270)
                {
                   // print(turnChunks.transform.eulerAngles.y);
                    turnChunks.transform.Rotate(0.0f,  -15f, 0.0f);
                    yield return new WaitForSeconds(Mathf.Epsilon * 1f);
                }
            }
            
        }







        bool AtTurningPoint()   //터닝포인트와 나우포스가 맞는지 체크   나중에 ui 쪽이랑 맞춰줄것.
        {
            if (chunkLoading.GetNowPos() == turningPoint)
                return true;
            else return false;
        }

        //열번의 청크 생성 후 호출되도록
        public float willTurn(float nowPos)  //터닝포인트 10단위 위치 리턴
        {
            if (turningPoint + 10 > nowPos) //터닝 텀.
                return 0;

            int turning = Random.Range(0, 2);
            if (turning != 0) return 0;  //방향 전환 없음
            else //방향전환 있음
            {
                print("터닝 플래그 온");
                turnFlag = true;

                float makePos = nowPos + frontShowChunk * chunkLoading.GetChunkMargin();
                turningPoint = makePos+(2 * chunkLoading.GetChunkMargin());
                Vector3 pos = new Vector3(0, 0, makePos);
                print("생성위치 ="+pos+"터닝포인트="+turningPoint);
                GameObject temp;
                if(Random.Range(0,2)==0)    //0이면 왼쪽
                {
                    Debug.Log("왼쪽 결정");
                    whichTurn = E_WhichTurn.LEFT;
                    //왼쪽 기역 청크 생성
                    temp=Instantiate(toLeftChunks, pos,Quaternion.identity);//나중에 풀링으로 하기
                    turnChunks = temp.transform.Find("TurningPoint").gameObject;

                }
                else
                {
                    Debug.Log("오른쪽 결정");
                    whichTurn = E_WhichTurn.RIGHT;
                    //오른쪽 청크 생성
                    temp=Instantiate(toRightChunks, pos, Quaternion.identity);//나중에 풀링으로 하기
                    turnChunks = temp.transform.Find("TurningPoint").gameObject;
                }
                return turningPoint;
            }
        }
      



    }
}