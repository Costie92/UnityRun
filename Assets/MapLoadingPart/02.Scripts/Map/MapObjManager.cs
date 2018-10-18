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

    [RequireComponent(typeof(MapAndObjPool))]
    [RequireComponent(typeof(ChunkLoading))]
    [RequireComponent(typeof(MapPathGenerator))]
    [RequireComponent(typeof(RandomObjGenerator))]
    [RequireComponent(typeof(TurnPartInCharge))]
    [RequireComponent(typeof(DataOfMapObjMgr))]
    [RequireComponent(typeof(ObjInCharge))]
    public class MapObjManager : SingletonTemplate<MapObjManager>
    {
        public GameObject chunk;    //청크
        private Transform playerTr;
        private float chunkMargin;
        private float chunkMarginDiv;
        public int wantToShowNumOfChunks = 7; //후방 하나, 나머지 앞의 청크들
        public int wantToShowNumOfChunksInBehind = 1;
        public float nowPos = 0;    //z축 기준 현재는
        public float newPos = 0;
        public float turnedPoint = 0;
        Vector3 turnChunksPos;
        flagInTurning turnFlagSet;
        E_WhichTurn whichTurn = E_WhichTurn.NOT_TURN;

        IMapTurnToUI mapTurnToUI;

        WaitForSeconds ws = new WaitForSeconds(0.15f);

        public float GetChunkMargin() { return chunkMargin; }



        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            turnFlagSet.Reset();
            chunkMargin = chunk.GetComponentInChildren<Renderer>().bounds.size.z;
            chunkMarginDiv = 1 / chunkMargin;
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
            mapTurnToUI = GameObject.Find("GameMgr").GetComponent<IMapTurnToUI>();
        }
        private void Start()
        {
            MapAndObjPool.GetInstance().ChunkPoolInit(10);

            MapAndObjPool.GetInstance().obsBallPoolInit(5);
            MapAndObjPool.GetInstance().obsHuddlePoolInit(5);
            MapAndObjPool.GetInstance().obsUpperHuddle_1_PoolInit();
            MapAndObjPool.GetInstance().obsUpperHuddle_2_PoolInit();
            MapAndObjPool.GetInstance().obsUpperHuddle_3_PoolInit();
            MapAndObjPool.GetInstance().obsFirePoolInit();

            MapAndObjPool.GetInstance().itemHPPlusPoolInit();
            MapAndObjPool.GetInstance().itemInvinciblePoolInit();
            MapAndObjPool.GetInstance().itemShieldPoolInit();
            MapAndObjPool.GetInstance().itemCoinPoolInit(100);
            MapAndObjPool.GetInstance().itemMagnetPoolInit();
            

            ChunkLoading.GetInstance().ChunkLoad(GetPosByChunkMargin());
            ObjInCharge.GetInstance(). AdjustObjSpawn();

            StartCoroutine(checkPos()); //부하를 줄이기 위해 0.2초 단위로 체크
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
          //  newPos = GetPosByChunkMargin(); //코루틴으로 체크 부하를 줄임

            if (newPos == nowPos)
                return;//변화 없으면 리턴
            
            nowPos = newPos;

            if (false == IsTurnPlanOn())   //회전 상태여부 체크
            {
                ChunkLoading.GetInstance().ChunkLoad(nowPos);
                ObjInCharge.GetInstance().AdjustObjSpawn();
                return;
            }

            //회전 플래그 상태
            if (turnFlagSet.flag == false)  //초기화
                InitOfTurnPlan();
            
            ChunkLoading.GetInstance().ChunkLoad(nowPos, turnFlagSet.turningPoint);
            ObjInCharge.GetInstance().AdjustObjSpawn();

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
