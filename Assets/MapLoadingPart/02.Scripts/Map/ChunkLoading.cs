using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using UnityEditor;

namespace hcp
{
  
    /*
    [CustomEditor(typeof(ChunkLoading))]
    public class ChunkInspector : Editor
    {
        List<float> toShowDic;

        void OnEnable()
        {
            //Character 컴포넌트를 얻어오기
            if (ChunkLoading.GetInstance().posList != null)
                toShowDic = ChunkLoading.GetInstance().posList;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (ChunkLoading.GetInstance().posList != null)
                toShowDic = ChunkLoading.GetInstance().posList;
            foreach (var n in toShowDic)
            {
                    EditorGUILayout.LabelField(n.ToString());

            }
        }
    }
    */

    public class ChunkLoading : SingletonTemplate<ChunkLoading>
    {
        [System.Serializable]
        enum E_CHUNK_CREATE_DEL
        {
            CREATE=0,
            DEL,
            MAX
        };
        [System.Serializable]
        public struct ShowCandidate
        {
            public float pos;
            public bool alreadyIn;
        }

        public static bool debugLog = false;

        private float margin;//청크의 z축 크기(실제 바닥면 개념 큐브등의 크기)
        private float marginDiv;

        public List<float> posList;
        public ShowCandidate[] showCandidates;
        List<float>[] crtAndDelPosLists;
        List<ChunkObjST> rtChunkObjSTList = new List<ChunkObjST>();

        protected override void Awake()
        {
            base.Awake();

            crtAndDelPosLists = new List<float>[(int)E_CHUNK_CREATE_DEL.MAX];
            for (int i = 0; i < (int)E_CHUNK_CREATE_DEL.MAX; i++)
            {
                crtAndDelPosLists[i] = new List<float>();
            }
        }

        private void Start()
        {
            showCandidates = new ShowCandidate[MapObjManager.GetInstance().wantToShowNumOfChunks];

            for (int i = 0; i < showCandidates.Length; i++) //청크 후보 자리 초기화
            {
                showCandidates[i].pos = ((-1 * MapObjManager.GetInstance().wantToShowNumOfChunksInBehind) + i);   //후방과 전방에 놓을 청크의 수 저장
                showCandidates[i].alreadyIn = false;
            }
            margin = MapObjManager.GetInstance().GetChunkMargin();
            if (margin == 0) ErrorManager.SpurtError();
            marginDiv = 1 / margin;
        }

        public List<ChunkObjST> ChunkLoad(float nowPos)
        {
            rtChunkObjSTList.Clear();

            CleanCrtDelLists();
            CandidateReady();
            posList = DataOfMapObjMgr.GetInstance().GetPosList();
            

            CandidateCheck(nowPos);   //후보군 체크
            ChunkCreateAndDel();

            return rtChunkObjSTList;
        }
        
        public List<ChunkObjST> ChunkLoad(float nowPos, float turningPoint)
        {
            rtChunkObjSTList.Clear();

            CleanCrtDelLists();
            CandidateReadyForTurn(nowPos,turningPoint);
            posList = DataOfMapObjMgr.GetInstance().GetPosList();

            CandidateCheck(nowPos);   //터닝 포인트 들어온 대로 후보군 체크
            ChunkCreateAndDel();

            return rtChunkObjSTList;
        }

        void CandidateCheck(float nowPos)//청크리스트를 이용하여 생성과 삭제할 자리를 찾아냄.
        {
            bool checkFinished = false;
            
            foreach (float item in posList)   //생성과 삭제 관리
            {
                checkFinished = false;
                for (int i = 0; i < showCandidates.Length; i++) //후보군
                {
                    float pos = nowPos + (showCandidates[i].pos * margin);
                    if (pos == item&&showCandidates[i].alreadyIn==false) //후보군에 있다면
                    {
                        showCandidates[i].alreadyIn = true;  //후보가 이미 생성되있다는 의미
                        //이 item은 삭제되지 않음.
                        checkFinished = true;
                        break;
                    }
                }
                if(checkFinished==false)
                //후보군에 들지 못한 청크.
                crtAndDelPosLists[(int)E_CHUNK_CREATE_DEL.DEL].Add(item);
            }

            for (int i = 0; i < showCandidates.Length; i++)
            {
                if (showCandidates[i].alreadyIn == false)
                {
                    float pos = nowPos + (showCandidates[i].pos * margin);
                    //새로 생성할 청크 포지션
                    crtAndDelPosLists[(int)E_CHUNK_CREATE_DEL.CREATE].Add(pos);
                }
            }
        }

        void ChunkCreateAndDel()
        {
            foreach (float pos in crtAndDelPosLists[(int)E_CHUNK_CREATE_DEL.DEL])   //삭제할 청크
            {
                if (posList.Contains(pos))
                {
                    DataOfMapObjMgr.GetInstance().TurnInChunkObjSTPool(pos);
                }
                else Debug.Log("청크 삭제 중 오류 001");
            }

            foreach (float pos in crtAndDelPosLists[(int)E_CHUNK_CREATE_DEL.CREATE])    //생성할 청크
            {
                Vector3 createPos = new Vector3(0, 0, pos);
                GameObject crtChunk = MapAndObjPool.GetInstance().GetChunkInPool();
                ChunkObjST temp;
                if (crtChunk != null)
                {
                    crtChunk.transform.SetPositionAndRotation(createPos, Quaternion.identity);
                    crtChunk.SetActive(true);
                    temp = DataOfMapObjMgr.GetInstance().GetEmptyChunkObjSTFromPool(crtChunk,pos);
                    if (temp != null)
                        rtChunkObjSTList.Add(temp);
                    else ErrorManager.SpurtError("청크자료구조 받아오던중 에러");
                }
                else Debug.Log("청크 풀 부족");
            }
        }

        void CleanCrtDelLists()
        {
            for (int i = 0; i < (int)E_CHUNK_CREATE_DEL.MAX; i++)
                crtAndDelPosLists[i].Clear();
        }

        void CandidateReady()
        {
            for (int i = 0; i < showCandidates.Length; i++)
            {
                showCandidates[i].alreadyIn = false;
            }
        }

        void CandidateReadyForTurn(float nowPos,float turningPoint) //터닝포인트 회절길 만들어주면됨.
        {
            //3과 2가 기역자 청크에 종속적인 값들.
            int frontShowChunk = MapObjManager.GetInstance().wantToShowNumOfChunks - MapObjManager.GetInstance().wantToShowNumOfChunksInBehind;    //앞쪽으로의 청크의 숫자
            float dis = (turningPoint - nowPos)*marginDiv; //터닝포인트에서 현재위치 까지의 거리
            //기역자 청크에 상당히 의존한 겂으로써 -4는 무시, -3부터 0 포지션 부터 청크 생성
            //-2는 1번째 포지션 자리부터 청크 생성...

            if ((-1 * (3 + MapObjManager.GetInstance().wantToShowNumOfChunksInBehind)) >= dis //음수, 현재 위치가 터닝포인트보다 앞. 회전 포인트와 상관이 없어질때의 위치만큼 앞에 가있을때.
                ||
                dis >= (frontShowChunk + 2))    //양수, 현재위치가 충분히 터닝포인트보다 뒤에 있을때.
            {
                CandidateReady();
                return;
            }   
            
           
            float createStartPosition = dis + 3;    //기역자 청크를 넘어서 새로 만들때 포지션
            float beHoldPosition = createStartPosition - 6;//이것 역시 기역자 청크에 의존한 값    기역자 청크 전에 남겨둬야한 청크 포지션
           //dis+3 이 새로 생성해야할 청크포지션 시작점임.(alreadyIn을 false로 만들어야함. 체킹캔디데이트로 권한을 넘기는것.)
            for (int i = 0; i < showCandidates.Length; i++)
            {
                if(showCandidates[i].pos >= createStartPosition || showCandidates[i].pos <=beHoldPosition)
                showCandidates[i].alreadyIn = false;
                else showCandidates[i].alreadyIn = true; //true = 청크가 삭제도 생성도 안되게 만듦.
            }
        }
    }
};