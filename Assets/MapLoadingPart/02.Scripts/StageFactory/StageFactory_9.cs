using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace hcp {
    public class StageFactory_9 :MonoBehaviour  {
        
        void Awake()
        {
            StageST.InitForStageLoading();
        }
        private void Start()
        {
            if (StageManager.stageNum == E_STAGE.STAGE_9)
                Stage_9_Making();
            else ErrorManager.SpurtError("스테이지 넘버링 오류! 스테이지_1이여야만함!");
        }
    

        void Stage_9_Making()
        {
            StageST.InitForStageLoading();
            
            /********************************  맵 구성요소 저장   ******************************************/
            
            StageST.EnqueStageTurningPoint(16, E_WhichTurn.RIGHT);
            StageST.EnqueStageTurningPoint(Constants.turningTerm*2, E_WhichTurn.LEFT);
            StageST.EnqueStageTurningPoint( Constants.turningTerm*3, E_WhichTurn.RIGHT);

            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.SHIELD, E_SPAWN_OBJ_TYPE.NOTHING, E_SPAWN_OBJ_TYPE.SHIELD); //이걸 쭉 이어나가서 큐 만들기.
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.BALL);
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);

            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);

            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);

            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);

            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);

            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);

            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);


            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);

            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);

            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);
            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);

            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);

            StageST.EnqueStageObjs(E_SPAWN_OBJ_TYPE.BALL, E_SPAWN_OBJ_TYPE.HUDDLE, E_SPAWN_OBJ_TYPE.COIN);
            
        }
    }
}