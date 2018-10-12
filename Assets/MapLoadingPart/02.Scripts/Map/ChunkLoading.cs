using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace hcp
{
  
    [RequireComponent(typeof(MapAndObjPool))]
    public class ChunkLoading : MonoBehaviour
    {
        struct ShowCandidate
    {
        public float pos;
        public bool alreadyIn;
    }
    struct flagInTurning
    {
            public int readyForTurn;
            public  bool flag;
            public float turningPoint;
    }

        public GameObject chunk;//청크 프리팹
        private Transform playerTr;
        //키로 float, 청크의 생성위치, value는 청크 게임오브젝트
        public Dictionary<float, GameObject> chunkOnMap = new Dictionary<float, GameObject>();
        public float margin;//청크의 z축 크기(실제 바닥면 개념 큐브등의 크기)
        ShowCandidate[] showCandidates;
        public int wantToShowNumOfChunks = 7; //후방 하나, 나머지 앞의 청크들
        public int wantToShowNumOfChunksInBehind = 1;
        public float nowPos = 0;    //z축 기준 현재는
        public float newPos = 0;
        // Use this for initialization

        flagInTurning turnFlag;


        MapPathGenerator mapPathG;
        public float GetChunkMargin() { return margin; }
        public float GetNowPos() { return nowPos; }

        void Start()
        {
            turnFlag.flag = false;
            turnFlag.readyForTurn = 0;
            turnFlag.turningPoint = 0;
            MapAndObjPool.instance.obsBallPoolInit(7);
            MapAndObjPool.instance.obsHuddlePoolInit(2);
            MapAndObjPool.instance.ChunkPoolInit(wantToShowNumOfChunks);
            margin = chunk.GetComponentInChildren<Renderer>().bounds.size.z;
            showCandidates = new ShowCandidate[wantToShowNumOfChunks];
            playerTr = GameObject.FindWithTag("PLAYER").transform;
            for (int i = 0; i < showCandidates.Length; i++)
            {
                showCandidates[i].pos = ((-1 * wantToShowNumOfChunksInBehind) + i);   //후방과 전방에 놓을 청크의 수 저장
                showCandidates[i].alreadyIn = false;
            }
            ChunkLoad(playerTr.position.z);
            mapPathG = GetComponent<MapPathGenerator>();
        }

        // Update is called once per frame
        void Update()
        {
            float z = playerTr.position.z;
            int temp = (int)(z / margin);
            if (z < 0) temp += -1;
            newPos = temp * margin;    //청크의 길이 단위로 플레이어의 위치값 체크
            if (newPos != nowPos)
            {
                nowPos = newPos;
                CandidateReady();
                if(nowPos>10&&turnFlag.flag==false)
                turnFlag.turningPoint=mapPathG.willTurn(nowPos);



                if (turnFlag.turningPoint == 0)
                {
                    ChunkLoad(nowPos);
                    return;
                }
                else if (turnFlag.turningPoint > 0) //터닝이 나왔으면
                {
                    if (turnFlag.flag == false) //터닝 준비 초기화
                    {
                        ChunkLoad(nowPos);
                        turnFlag.readyForTurn = 3 +wantToShowNumOfChunksInBehind;  //터닝포인트 부터 기역자 청크의 꺽이는 나머지 청크들
                        turnFlag.flag = true;
                    }
                    if (turnFlag.flag && nowPos >= turnFlag.turningPoint)    //터닝포인트에 들어왔을때 3개 청크 남기기 처리해줄것
                    {
                        CandidateReadyForTurn();
                        ChunkLoad(nowPos);
                        if(nowPos >=turnFlag.turningPoint+3) //터닝 프로세스 완료. 터닝 초기화!
                        {
                            turnFlag.flag = false;
                            turnFlag.readyForTurn = 0;
                            turnFlag.turningPoint = 0;
                        }
                    }
                }
            }
        }

        void ChunkLoad(float nowPos)
        {
           // Debug.Log("청크로드");

            foreach (var item in chunkOnMap.Keys.ToList())   //삭제할 청크 찾음
            {
               // Debug.Log("item=" + item.ToString());
                if (!InShowListChk(nowPos, item)) //있는 청크리스트 중 현재 보일 청크가 아님
                {
                    GameObject temp = chunkOnMap[item];
                    if (chunkOnMap.ContainsKey(item))
                    {
                        if (chunkOnMap.Remove(item)) ;
                        //    Debug.Log(item.ToString() + " 위치 청크 삭제");
                    }
                    if (temp)
                    {
                        MapAndObjPool.instance. TurnInPoolObj(temp);


                        //Destroy(temp);
                    }
                }
            }

            for (int i = 0; i < showCandidates.Length; i++)    //생성할 청크(후방 이동과 전진이동에 유연하기 위해서)
            {
                if (!showCandidates[i].alreadyIn)    //청크를 생성해야할 위치
                {
               //     Debug.Log(showCandidates[i].pos + "자리에 청크 생성");
                    float makePos = nowPos + showCandidates[i].pos * margin;
                    Vector3 pos = new Vector3(0, 0, makePos);
                    var temp = MapAndObjPool.instance.GetChunkInPool();
                    //오브젝트 풀링경우


                    if (temp != null)
                    {
                    //    Debug.Log("&&&&&&&&&&&&&&&&&&");
                        temp.transform.position = pos;
                        temp.transform.rotation = Quaternion.identity;
                        temp.SetActive(true);
                    }
                    else
                    {
                  //      Debug.Log("받아온 게 널임");
                    }


                    // temp= Instantiate(chunk,pos,Quaternion.identity);
                    chunkOnMap.Add(makePos, temp);
                }
            }

        }
        bool InShowListChk(float nowPos, float item) //청크리스트[item]이 보여야하는 청크인지 체크
        {
            for (int i = 0; i < showCandidates.Length; i++)
            {
                if (nowPos + (showCandidates[i].pos * margin) == item) //보여야하는 청크면 true반환,item은 청크의 시작위치이자 딕셔너리의 키
                {
                    showCandidates[i].alreadyIn = true;  //후보가 이미 생성되있다는 의미
                    return true;
                }
            }
            return false;
        }
        void CandidateReady()
        {
            for (int i = 0; i < showCandidates.Length; i++)
            {
                showCandidates[i].alreadyIn = false;
            }
        }
        void CandidateReadyForTurn()
        {
            for (int i = 0; i < turnFlag.readyForTurn; i++)
            {

                showCandidates[i].alreadyIn = true;
            }
            turnFlag.readyForTurn--;
        }
    }
};