using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public static class StageST
    {
        public static Queue<StageTurnPointSet> turningPointQueue = new Queue<StageTurnPointSet>(); //스테이지 에서 쓰이는 큐
        public static Queue<StageObjArr> objQueue = new Queue<StageObjArr>();//스테이지 에서 쓰이는 큐
        static float lastTurningPoint=0;
        
        
        public static void EnqueStageObjs(E_SPAWN_OBJ_TYPE obj1st, E_SPAWN_OBJ_TYPE obj2nd, E_SPAWN_OBJ_TYPE obj3rd)
        {
            StageObjArr tempObjQue = new StageObjArr(obj1st, obj2nd, obj3rd);
            objQueue.Enqueue(tempObjQue);
        }
        public static void EnqueStageObjs(StageObjArr soa)
        {
            objQueue.Enqueue(soa);
        }
        public static void EnqueStageTurningPoint(float tp, E_WhichTurn wt)
        {
            if (tp < Constants.turningTerm || wt == E_WhichTurn.NOT_TURN || tp < lastTurningPoint+Constants.turningTerm)  return;    //터닝 포인트 조건 충족 못함.

                StageTurnPointSet tempTPS = new StageTurnPointSet(tp, wt);
                turningPointQueue.Enqueue(tempTPS);
                lastTurningPoint = tp;
        }
        public static void InitForStageLoading()
        {
            turningPointQueue.Clear();
            objQueue.Clear();
            lastTurningPoint = 0;
        }
        public static bool AllQueIsDeqed()
        {
            if (turningPointQueue.Count == 0 && objQueue.Count == 0)
            {
                return true;
            }
            else return false;
        }
    }
}