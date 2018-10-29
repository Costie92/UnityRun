using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp {
    public class InfinityFactory : SingletonTemplate<InfinityFactory>
    {
        float exTurnPoint = 0;
        WaitForSeconds ws = new WaitForSeconds(2f);
        protected override void Awake()
        {
            base.Awake();

            StageST.InitForStageLoading();
        }

        private void Start()
        {
            if (StageManager.stageNum == E_STAGE.INFINITY)
                InfinityStageMaking();
            else ErrorManager.SpurtError("스테이지 넘버 오류! 인피니티여야함.");
            StartCoroutine(EnqueForInfinity());
        }
        IEnumerator EnqueForInfinity()
        {
            while (true)
            {
                if (StageST.objQueue.Count < 10)
                {
                    MakeObjQueNode(5);
                }
                if (StageST.turningPointQueue.Count < 5)
                {
                    MakeTurnSetQueNode();
                }
                yield return ws;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(2))
            {
                print(StageST.objQueue.Count + "-오브젝트큐카운트  ,  " + StageST.turningPointQueue.Count);
            }
        }


        void InfinityStageMaking()
        {
            StageST.InitForStageLoading();
            exTurnPoint = 0;

                MakeObjQueNode(100);
                MakeTurnSetQueNode(20);
        }

        public void MakeTurnSetQueNode(int number = 1)
        {
            if (number < 1) number = 1;
            for (int i = 0; i < number; i++)
            {
                float tp = exTurnPoint + Random.Range(Constants.turningTerm, Constants.turningTerm+10);    //10~ 20 후에 터닝포인트 생성
                exTurnPoint = tp;
                E_WhichTurn wt = (Random.Range(0, 2) == 0) ? wt = E_WhichTurn.LEFT : wt = E_WhichTurn.RIGHT;

                StageST.EnqueStageTurningPoint(tp, wt);
            }
        }
        public void MakeTurnSetQueNode(float nowPos,float chunkMarginDiv)
        {
            if (exTurnPoint <= nowPos * chunkMarginDiv)
                exTurnPoint = (nowPos * chunkMarginDiv) + 5;
            MakeTurnSetQueNode();
        }

        public void MakeObjQueNode(int number=1)
        {
            if (number < 1) number = 1;
            for (int i = 0; i < number; i++)
            {
                StageObjArr soa = RandomObjGenerator.GetInstance().RandomObjGen();

                StageST.EnqueStageObjs(soa);
            }
        }
    }
}