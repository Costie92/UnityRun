using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace hcp
{
    public class ChunkLoading : SingletonTemplate<ChunkLoading>
    {
        [System.Serializable]
        public struct ShowCandidate
        {
            public float pos;
            public bool alreadyIn;
        }

        private float margin;//청크의 z축 크기(실제 바닥면 개념 큐브등의 크기)
        private int wantToShowNumOfChunks; //보여줄 청크의 총 수
        private int wantToShowNumOfChunksInBehind ;//후방에 남겨둘 청크 수

        //키로 float, 청크의 생성위치, value는 청크 게임오브젝트
        public Dictionary<float, GameObject>        chunkOnMap          = new Dictionary<float, GameObject>();
        public Dictionary<float, List<GameObject>>  spawnedObjOnChunk   = new Dictionary<float, List<GameObject>>();
        ShowCandidate[] showCandidates;

        protected override void Awake()
        {
            base.Awake();
            wantToShowNumOfChunks = MapObjManager.GetInstance().wantToShowNumOfChunks;
            wantToShowNumOfChunksInBehind = MapObjManager.GetInstance().wantToShowNumOfChunksInBehind;
            showCandidates = new ShowCandidate[wantToShowNumOfChunks];

            for (int i = 0; i < showCandidates.Length; i++)
            {
                showCandidates[i].pos = ((-1 * wantToShowNumOfChunksInBehind) + i);   //후방과 전방에 놓을 청크의 수 저장
                showCandidates[i].alreadyIn = false;
            }
            margin = MapObjManager.GetInstance().GetChunkMargin();

        }

        public void ChunkLoad(float nowPos, flagInTurning turnFlagSet)
        {
            CandidateReady();
             Debug.Log("청크로드");
            if (turnFlagSet.flag && turnFlagSet.readyForTurn > 0)
                CandidateReadyForTurn(turnFlagSet.readyForTurn);

            foreach (var item in chunkOnMap.Keys.ToList())   //삭제할 청크 찾음
            {
               // Debug.Log("item=" + item.ToString());
                if (!InShowListChk(nowPos, item)) //있는 청크리스트 중 현재 보일 청크가 아님
                    //삭제할 청크만 체크하는 것.
                {
                    GameObject temp = chunkOnMap[item];
                    if (chunkOnMap.ContainsKey(item))
                    {
                        if (chunkOnMap.Remove(item)) ;
                            Debug.Log(item.ToString() + " 위치 청크 삭제");
                    }
                    if (temp)
                    {
                        MapAndObjPool.GetInstance(). TurnInPoolObj(temp);
                        //Destroy(temp);
                    }

                    if (spawnedObjOnChunk.ContainsKey(item))//스폰 됐던 아이템들 삭제
                    {
                        foreach (var spawned in spawnedObjOnChunk[item])
                        {
                            if(spawned.transform.position.z< MapObjManager.GetInstance().GetPosByChunkMargin()
                                &&spawned.activeSelf==true)
                            spawned.SetActive(false);
                        }
                        spawnedObjOnChunk.Remove(item);
                    }
                }
            }

            for (int i = 0; i < showCandidates.Length; i++)    //생성할 청크(후방 이동과 전진이동에 유연하기 위해서)
            {
                if (!showCandidates[i].alreadyIn)    //청크를 생성해야할 위치
                {
                   Debug.Log(showCandidates[i].pos + "자리에 청크 생성");
                    float makePos = nowPos + showCandidates[i].pos * margin;
                    Vector3 pos = new Vector3(0, 0, makePos);
                    var temp = MapAndObjPool.GetInstance().GetChunkInPool();
                    List<GameObject> spawnedThingsTemp = new List<GameObject>();

                    if (temp != null)
                    {
                        Debug.Log("&&&&&&&&&&&&&&&&&&");
                        temp.transform.position = pos;
                        temp.transform.rotation = Quaternion.identity;
                        temp.SetActive(true);

                        Transform spawnPointGroup = temp.transform.Find("SpawnPointGroup");
                        //스폰포인트에 오브젝트들을 자식으로 붙이면 안 됨 위치에만 두되 따로 관리할 수 있는 자료구조를 이용하여 
                        //청크가 삭제될때 그 자리의 스폰 포인트 들도 다 삭제할 수 있도록 해야함.
                        for (int s = 0; s < spawnPointGroup.transform.childCount; s++)
                        {
                            List<GameObject> t=
                            RandomObjGenerator.GetInstance().RandomObjGen(spawnPointGroup.GetChild(s), s);
                            if(null != t)
                            spawnedThingsTemp.AddRange(t);
                        }

                    }
                    else
                    {
                       Debug.Log("받아온 게 널임");
                    }
                    chunkOnMap.Add(makePos, temp);

                    spawnedObjOnChunk.Add(makePos, spawnedThingsTemp);


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
        void CandidateReadyForTurn(int readyForTurn)
        {
            for (int i = 0; i < readyForTurn; i++)
            {
                showCandidates[i].alreadyIn = true;
            }
        }
    }
};