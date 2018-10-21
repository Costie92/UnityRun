using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp {
    public static class FixedObjGenerator
    {
        public static void FixedObjGen(Transform spg, E_SPAWN_OBJ_TYPE[] objTypeArr, ref List<GameObject> objs)
        {
            if (spg == null || objs==null || objTypeArr==null) return;

            if (objTypeArr.Length > 3) ErrorManager.SpurtError("전달 받은 타입 배열의 길이가 3이상");

            GameObject spawned;

            for (int i = 0; i < spg.childCount; i++)
            {
                spawned=
                ObjFactory.ObjSpawnFactory(objTypeArr[i], spg.GetChild(i));
                if (spawned != null)
                    objs.Add(spawned);
            }
        }
    }
}