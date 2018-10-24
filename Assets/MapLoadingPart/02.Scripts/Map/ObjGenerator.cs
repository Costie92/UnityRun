using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp {

    public static class ObjGenerator
    {
        public static void ObjGen(E_STAGE whatStage,Transform spawnPointGroup, ref List<GameObject> objs)
        {
            if (objs == null||spawnPointGroup==null) return;

            if (StageST.objQueue.Count == 0)
            {
                if (whatStage == E_STAGE.INFINITY)
                    InfinityFactory.GetInstance().MakeObjQueNode();

                else return;
            }
            
            StageObjArr soa = StageST.objQueue.Dequeue();
            FixedObjGenerator.FixedObjGen(spawnPointGroup, soa.spawnObjType, ref objs);
        }
    }
}