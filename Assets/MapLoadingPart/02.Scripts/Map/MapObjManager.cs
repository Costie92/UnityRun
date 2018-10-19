using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hcp
{
    [System.Serializable]
    public struct flagInTurning
    {
        public bool flag;
        public float turningPoint;

        public void Reset()
        {
            flag = false;
            turningPoint = 0;
        }
    }
    [System.Serializable]
    public enum E_WhichTurn
    {
        LEFT = 0,
        RIGHT,
        NOT_TURN
    }
    public enum E_OBJ_SPAWN_WAY
    {
        RANDOM=0,
    }

    [RequireComponent(typeof(MapAndObjPool))]
    [RequireComponent(typeof(ChunkLoading))]
    [RequireComponent(typeof(MapPathGenerator))]
    [RequireComponent(typeof(RandomObjGenerator))]
    [RequireComponent(typeof(TurnPartInCharge))]
    [RequireComponent(typeof(DataOfMapObjMgr))]
    [RequireComponent(typeof(ObjFactory))]
    public class MapObjManager : SingletonTemplate<MapObjManager>
    {
        public GameObject chunk;    //청크
        private Transform playerTr;
        private float chunkMargin;
        private float chunkMarginDiv;
        public int wantToShowNumOfChunks = 7; //후방 하나, 나머지 앞의 청크들
        public int wantToShowNumOfChunksInBehind = 1;
        public float nowPos = -1;    //z축 기준 현재는
        public float newPos = 0;
        public float turnedPoint = 0;
        Vector3 turnChunksPos;
        flagInTurning turnFlagSet;
        E_WhichTurn whichTurn = E_WhichTurn.NOT_TURN;

        IMapTurnToUI mapTurnToUI;

        WaitForSeconds ws = new WaitForSeconds(0.15f);

        List<ChunkObjST> tempCOSTList = new List<ChunkObjST>();

        public float GetChunkMargin() { return chunkMargin; }

        float time;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            turnFlagSet.Reset();
            chunkMargin = chunk.GetComponentInChildren<Renderer>().bounds.size.z;
            chunkMarginDiv = 1 / chunkMargin;
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
            mapTurnToUI = GameObject.Find("GameMgr").GetComponent<IMapTurnToUI>();

            nowPos = -100;

            time = Time.time;
        }
        private void Start()
        {
            MapAndObjPool.GetInstance().ChunkPoolInit(10);

            MapAndObjPool.GetInstance().obsBallPoolInit(100);
            MapAndObjPool.GetInstance().obsHuddlePoolInit(100);
            MapAndObjPool.GetInstance().obsUpperHuddle_1_PoolInit(100);
            MapAndObjPool.GetInstance().obsUpperHuddle_2_PoolInit(100);
            MapAndObjPool.GetInstance().obsUpperHuddle_3_PoolInit(100);
            MapAndObjPool.GetInstance().obsFirePoolInit(100);

            MapAndObjPool.GetInstance().itemHPPlusPoolInit(100);
            MapAndObjPool.GetInstance().itemInvinciblePoolInit(100);
            MapAndObjPool.GetInstance().itemShieldPoolInit(100);
            MapAndObjPool.GetInstance().itemCoinPoolInit(100);
            MapAndObjPool.GetInstance().itemMagnetPoolInit(100);
            MapAndObjPool.GetInstance().itemCoin_Parabola_PoolInit(10);
            MapAndObjPool.GetInstance().itemCoin_StraightLine_PoolInit(10);
            /*
            tempCOSTList=
            ChunkLoading.GetInstance().ChunkLoad(GetPosByChunkMargin());
            SetObjToNewChunks(E_OBJ_SPAWN_WAY.RANDOM);
            */

            StartCoroutine(checkPos()); //부하를 줄이기 위해 0.2초 단위로 체크
        }

        void SetObjToNewChunks(E_OBJ_SPAWN_WAY way)
        {
            //나중에 way 따라 분기하기
            //나중에 오브젝트들 큐 받아와서 하는 것도 오버로딩 해서 따로 구현하기.
            for (int i = 0; i < tempCOSTList.Count; i++)
            {
                tempCOSTList[i].ObjSpawn(way);
            }
            tempCOSTList.Clear();
        }

        //청크의 길이 단위로 플레이어의 위치값 체크
        public float GetPosByChunkMargin()
        {
            float z = playerTr.position.z;
            int temp = (int)(z *chunkMarginDiv);
            if (z < 0) temp += -1;
            float tempPos = temp * chunkMargin;
            return tempPos;
        }

        bool IsTurnPlanOn()
        {
            if (turnFlagSet.flag == true) return true;
            if (nowPos > 10 * chunkMargin && turnFlagSet.flag == false)   //회전 여부 체크
            {
                turnFlagSet.turningPoint = MapPathGenerator.GetInstance().WillTurn(nowPos, turnedPoint, out whichTurn, out turnChunksPos);
                if (turnFlagSet.turningPoint > 0)
                    return true;
                else return false;
            }
            else return false;
        }

        void InitOfTurnPlan()
        {
            if (turnFlagSet.turningPoint > 0 && whichTurn != E_WhichTurn.NOT_TURN) //터닝이 나왔으면
            {
                if (turnFlagSet.flag == false) //터닝 준비 초기화
                {
                    //터닝청크 생성
                    TurnPartInCharge.GetInstance().GenerateTurnChunks(whichTurn, turnChunksPos, turnFlagSet.turningPoint);

                    turnedPoint = turnFlagSet.turningPoint;
                    turnFlagSet.flag = true;

                    //ui 쪽에 터닝포인트와 방향 알려줌
                    if (mapTurnToUI != null)
                    {
                        mapTurnToUI.SetTurningPointToUI(turnFlagSet.turningPoint);
                        mapTurnToUI.SetWhichTurnToUI(whichTurn);
                    }
                }
            }
        }

        IEnumerator checkPos()  //부하를 줄이는 코루틴
        {
            while (true)
            {
                newPos = GetPosByChunkMargin();
                yield return ws;
            }
        }

        void Update()
        {
            // newPos = GetPosByChunkMargin(); //코루틴으로 체크 부하를 줄임

            if (newPos == nowPos)
                return;//변화 없으면 리턴
            
            nowPos = newPos;

            if (false == IsTurnPlanOn())   //회전 상태여부 체크
            {
                tempCOSTList =
               ChunkLoading.GetInstance().ChunkLoad(nowPos);
                SetObjToNewChunks(E_OBJ_SPAWN_WAY.RANDOM);
                return;
            }

            //회전 플래그 상태
            if (turnFlagSet.flag == false)  //초기화
                InitOfTurnPlan();

            tempCOSTList =
           ChunkLoading.GetInstance().ChunkLoad(nowPos, turnFlagSet.turningPoint);
            SetObjToNewChunks(E_OBJ_SPAWN_WAY.RANDOM);

            WhenTurningFinished();
        }

        void WhenTurningFinished()
        {
                if ((nowPos >= (turnFlagSet.turningPoint + ( (3 + 1) * chunkMargin) ))&& turnFlagSet.flag )
                //포지션이 완전히 기역자 청크를 지나침.터닝 프로세스 완료. 터닝 초기화! 
                //3은 굉장히 기역자 청크에 종속적인 숫자고. 1은 기역자 청크가 갑자기 없어지면 화면이 이상해서 넣은것
                {
                    print("터닝 피니쉬드!");
                    whichTurn = E_WhichTurn.NOT_TURN;
                    turnFlagSet.Reset();
                    TurnPartInCharge.GetInstance().Reset();
                    //ui 쪽에 터닝포인트와 방향 리셋
                    if (mapTurnToUI != null)
                    {
                        mapTurnToUI.SetTurningPointToUI(turnFlagSet.turningPoint);
                        mapTurnToUI.SetWhichTurnToUI(whichTurn);
                    }
                }
        }

    }
}
