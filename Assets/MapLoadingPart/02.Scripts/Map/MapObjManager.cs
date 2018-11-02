using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hcp
{
    [RequireComponent(typeof(MapAndObjPool))]
    [RequireComponent(typeof(ChunkLoading))]
    [RequireComponent(typeof(TurnPartInCharge))]
    [RequireComponent(typeof(DataOfMapObjMgr))]
    [RequireComponent(typeof(RandomObjGenerator))]
    public class MapObjManager : SingletonTemplate<MapObjManager>
    {
        public GameObject endingStageLand;
        public float endingPoint;

        public GameObject chunk;    //청크
        private Transform playerTr;
        private float chunkMargin;
        private float chunkMarginDiv;

        public float nowPos = -1;    //z축 기준 현재는
        public float newPos = 0;

        StageTurnPointSet turnSet;

        IMapTurnToUI mapTurnToUI;

        E_STAGE whatStage;

        WaitForSeconds ws = new WaitForSeconds(0.15f);

        List<ChunkObjST> tempCOSTList = new List<ChunkObjST>();

        public float GetChunkMargin() { return chunkMargin; }

        bool stageRemain;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            chunkMargin = chunk.GetComponentInChildren<Renderer>().bounds.size.z;
            chunkMarginDiv = 1 / chunkMargin;
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
            mapTurnToUI = GameObject.Find("GameMgr").GetComponent<IMapTurnToUI>();

            nowPos = -100;

            turnSet = new StageTurnPointSet(-1,E_WhichTurn.NOT_TURN);
            turnSet.DisableThisSet();
            stageRemain = true;
        }
        private void Start()
        {
            
            whatStage = StageManager.stageNum;    

            MapAndObjPool.GetInstance().ChunkPoolInit(10);

            MapAndObjPool.GetInstance().obsBallPoolInit(10);
            MapAndObjPool.GetInstance().obsHuddlePoolInit(10);
            MapAndObjPool.GetInstance().obsUpperHuddle_1_PoolInit(10);
            MapAndObjPool.GetInstance().obsUpperHuddle_2_PoolInit(10);
            MapAndObjPool.GetInstance().obsUpperHuddle_3_PoolInit(10);
            MapAndObjPool.GetInstance().obsFirePoolInit(10);

            MapAndObjPool.GetInstance().itemHPPlusPoolInit(10);
            MapAndObjPool.GetInstance().itemInvinciblePoolInit(10);
            MapAndObjPool.GetInstance().itemShieldPoolInit(10);
            MapAndObjPool.GetInstance().itemCoinPoolInit(10);
            MapAndObjPool.GetInstance().itemMagnetPoolInit(10);
            MapAndObjPool.GetInstance().itemCoin_Parabola_PoolInit(10);
            MapAndObjPool.GetInstance().itemCoin_StraightLine_PoolInit(10);

            StartCoroutine(checkPos()); //부하를 줄이기 위해 0.2초 단위로 체크
        }
        bool isStage()
        {
            if (whatStage == E_STAGE.INFINITY)
                return false;
            else return true;
        }


        void SetObjToNewChunks(List<ChunkObjST> genedCOSTList)
        {
            genedCOSTList.Sort(
                  delegate (ChunkObjST x, ChunkObjST y)
                  {
                      int compareDate = x.position.CompareTo(y.position);
                      return compareDate;
                  }
                );


            for (int i = 0; i < genedCOSTList.Count; i++)   //새로 생긴 청크들 모두에게
            {
                if(genedCOSTList[i].position >= (Constants.firstObjSpawn*chunkMargin) )
                genedCOSTList[i].ObjSpawn(whatStage);  //오브젝트 스폰
            }
            genedCOSTList.Clear();
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
        

        void InitOfTurnPlan(StageTurnPointSet turnSet)
        {
                    //터닝청크 생성
                    TurnPartInCharge.GetInstance().GenerateTurnChunks(turnSet.whichTurn, 
                        turnSet.ConvertTurnPointToRealTurnPoint(chunkMargin));

                    //ui 쪽에 터닝포인트와 방향 알려줌
                    if (mapTurnToUI != null)
                    {
                        mapTurnToUI.SetTurningPointToUI(turnSet.ConvertTurnPointToRealTurnPoint(chunkMargin));
                        mapTurnToUI.SetWhichTurnToUI(turnSet.whichTurn);
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

        void Update()   //지금 랜덤 무한 생성 모드와 픽스드 모드를 어떻게 할지 결정해야하는데 고민이야.
            //변수 하나 받아와서 여기서 그냥 분기할까 클래스를 나눌까...
        {
            // newPos = GetPosByChunkMargin(); //코루틴으로 체크 부하를 줄임


            if (isStage()&&stageRemain == false)   //스테이지에서 더 생산 할 게 없음. 종료. 필요.
            {

                if (playerTr.position.z >= endingPoint + 2f)
                {
                   playerTr.gameObject.GetComponent<CharacterAnimation>().WinAnimation();
                }
                return;
            }

            if (newPos == nowPos)
                return;//변화 없으면 리턴
            
            nowPos = newPos;

            if (isStage()&&StageST.AllQueIsDeqed() && turnSet.IsDisabled() )    //회전도 다돌고 옵젝큐도 뭐도 없고 끝
            {
                Debug.Log("스테이지 종료."+nowPos);   //프론트 청크 -1 만큼 청크가 앞에 있응 상황에서 선언됨
                stageRemain = false;

                endingPoint = nowPos + ((Constants.frontShowChunks - 1) * chunkMargin);
                StageEnding(nowPos);
                return;
            }

            if (turnSet.IsDisabled() )    //회전이 끝났을때.
            {
                Debug.Log("맵턴큐 젠 시작");
                turnSet = MapPathGenerator.TurningGen(whatStage, nowPos, chunkMarginDiv , ref turnSet);
            }

            if (false ==  turnSet.IsDisabled()  && turnSet.Init == false )
            {
                InitOfTurnPlan(turnSet);
                //터닝 초기화 부
                turnSet.Init = true;
            }
            

            if (turnSet.IsDisabled() == false)
            {
                tempCOSTList =
                       ChunkLoading.GetInstance().ChunkLoad(nowPos, turnSet.ConvertTurnPointToRealTurnPoint(chunkMargin));
                SetObjToNewChunks(tempCOSTList);
            }
            else {  //옵젝큐만 남았거나 하는 상황.
                tempCOSTList =
                       ChunkLoading.GetInstance().ChunkLoad(nowPos);
                SetObjToNewChunks(tempCOSTList);
            }

            WhenTurningFinished();
        }
        
        void WhenTurningFinished()
        {
            if ((nowPos >= (turnSet.ConvertTurnPointToRealTurnPoint(chunkMargin) + ( (3 + 1) * chunkMargin) )))
                //포지션이 완전히 기역자 청크를 지나침.터닝 프로세스 완료. 터닝 초기화! 
                //3은 굉장히 기역자 청크에 종속적인 숫자고. 1은 기역자 청크가 갑자기 없어지면 화면이 이상해서 넣은것
            {
                print("터닝 피니쉬드!");

                //turnSet = null; //지금 ==null 체크가 다음 업데이트 프레임 넘어가면서 if 구문에서 걸리지를 않음.
                //if (turnSet == null) Debug.Log("널이 되었다 드디어");

                turnSet.DisableThisSet();

                //print(turnSet.Init.ToString()+ turnSet.turningPoint.ToString()+ turnSet.whichTurn.ToString());
                TurnPartInCharge.GetInstance().Reset();

                //ui 쪽에 터닝포인트와 방향 리셋
                    if (mapTurnToUI != null)
                    {
                        mapTurnToUI.SetTurningPointToUI(0);
                        mapTurnToUI.SetWhichTurnToUI(E_WhichTurn.NOT_TURN);
                    }
                }
        }

        void StageEnding(float nowPos)
        {
            Instantiate(endingStageLand, new Vector3(0, 0,endingPoint), Quaternion.identity);
            
            
        }
    }
}
