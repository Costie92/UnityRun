using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hcp
{
    [System.Serializable]
    public struct flagInTurning
        {
            public int readyForTurn;
            public bool flag;
            public float turningPoint;
        }
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
    public class MapObjManager : SingletonTemplate<MapObjManager>
    {
     
        public GameObject chunk;    //청크
        private Transform playerTr;
        public float chunkMargin;
        public int wantToShowNumOfChunks = 7; //후방 하나, 나머지 앞의 청크들
        public int wantToShowNumOfChunksInBehind = 1;
        public float nowPos = 0;    //z축 기준 현재는
        public float newPos = 0;
        public float turnedPoint=0;
        Vector3 turnChunksPos;
        flagInTurning turnFlagSet;
        E_WhichTurn whichTurn=E_WhichTurn.NOT_TURN;

        public float GetChunkMargin() { return chunkMargin; }


        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();

            turnFlagSet.turningPoint = 0;
            turnFlagSet.flag = false;
            turnFlagSet.readyForTurn = 0;
         
            chunkMargin = chunk.GetComponentInChildren<Renderer>().bounds.size.z;
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
        }
        private void Start()
        {
            MapAndObjPool.GetInstance().obsBallPoolInit(5);
            MapAndObjPool.GetInstance().obsHuddlePoolInit(5);
            MapAndObjPool.GetInstance().ChunkPoolInit(wantToShowNumOfChunks);
            ChunkLoading.GetInstance().ChunkLoad(GetPosByChunkMargin(), turnFlagSet);
        }

        //청크의 길이 단위로 플레이어의 위치값 체크
        float GetPosByChunkMargin()
        {
            float z = playerTr.position.z;
            int temp = (int)(z / chunkMargin);
            if (z < 0) temp += -1;
            float tempPos = temp * chunkMargin;    
            return tempPos;
        }
        
        // Update is called once per frame
        void Update()
        {
            newPos = GetPosByChunkMargin();
            if (newPos != nowPos)   //청크 단위길이 포지션 업데이트
            {
                nowPos = newPos;

                if (nowPos > 10*chunkMargin && turnFlagSet.flag == false)   //회전 여부 체크
                    turnFlagSet.turningPoint = MapPathGenerator.GetInstance().WillTurn(nowPos,turnedPoint,out whichTurn,out turnChunksPos);

                if (turnFlagSet.turningPoint == 0)
                {
                    ChunkLoading.GetInstance().ChunkLoad(nowPos, turnFlagSet);
                    return;
                }
                else if (turnFlagSet.turningPoint > 0&&whichTurn!=E_WhichTurn.NOT_TURN) //터닝이 나왔으면
                {
                    if (turnFlagSet.flag == false) //터닝 준비 초기화
                    {
                       //터닝청크 생성
                        TurnPartInCharge.GetInstance().GenerateTurnChunks(whichTurn,turnChunksPos);
                        
                        turnedPoint = turnFlagSet.turningPoint;
                        ChunkLoading.GetInstance().ChunkLoad(nowPos, turnFlagSet);
                        turnFlagSet.flag = true;
                        turnFlagSet.readyForTurn = 0;
                    }
                    //터닝포인트 구간에 들어가기 전까지 청크로드 하지 않음.
                    //터닝포인트 진입시 청크로드 안할 구간 체크.
                    //터닝포인트 진입
                    if (turnFlagSet.flag && nowPos >= turnFlagSet.turningPoint)
                    {
                        //지형돌리기 작업
                        if(AtTurningPoint(nowPos,turnFlagSet.turningPoint))
                        {
                            if (whichTurn == E_WhichTurn.NOT_TURN)
                                Debug.Log("위치턴 오류");
                            else
                            {
                                TurnPartInCharge.GetInstance().Turn();
                            }
                        }

                        if (turnFlagSet.readyForTurn == 0)
                            turnFlagSet.readyForTurn = 3 + wantToShowNumOfChunksInBehind;  //터닝포인트 부터 기역자 청크의 꺽이는 나머지 청크들
                                                                                           // CandidateReadyForTurn(); 청크로드에서 해주기
                        ChunkLoading.GetInstance().ChunkLoad(nowPos, turnFlagSet);

                        turnFlagSet.readyForTurn--;
                        if (nowPos >= turnFlagSet.turningPoint + (3*chunkMargin)) //터닝 프로세스 완료. 터닝 초기화!
                        {
                            whichTurn = E_WhichTurn.NOT_TURN;
                            turnFlagSet.flag = false;
                            turnFlagSet.readyForTurn = 0;
                            turnFlagSet.turningPoint = 0;
                            TurnPartInCharge.GetInstance().Reset();
                        }
                    }
                }
            }
        }

        //터닝포인트와 나우포스가 맞는지 체크   나중에 ui 쪽이랑 맞춰줄것.
        bool AtTurningPoint(float nowPos, float turningPoint)   
        {
            if (nowPos == turningPoint)
                return true;
            else return false;
        }






    }
}