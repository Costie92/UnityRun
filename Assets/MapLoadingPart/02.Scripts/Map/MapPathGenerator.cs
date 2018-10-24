using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public static class MapPathGenerator 
    {
        public static StageTurnPointSet TurningGen(E_STAGE whatStage,float nowPos,float chunkMarginDiv , ref StageTurnPointSet turnSet)
        {
            if (StageST.turningPointQueue.Count == 0)
            {
                if (whatStage == E_STAGE.INFINITY)
                    InfinityFactory.GetInstance().MakeTurnSetQueNode(nowPos, chunkMarginDiv);
                else
                {
                    Debug.Log("맵 턴 큐 비었음.");
                    turnSet.DisableThisSet();
                    return turnSet;
                }
            }
            return StageST.turningPointQueue.Dequeue();
        }
    }
}