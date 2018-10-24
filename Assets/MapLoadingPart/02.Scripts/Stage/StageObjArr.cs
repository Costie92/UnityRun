using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    [System.Serializable]
    public class StageObjArr   //얘는 저장용으로. 이걸 큐로 엮어서 저장해서 다시 불러오는 방식.
    {
        public E_SPAWN_OBJ_TYPE[] spawnObjType
            =
            { E_SPAWN_OBJ_TYPE.NOTHING, E_SPAWN_OBJ_TYPE.NOTHING, E_SPAWN_OBJ_TYPE.NOTHING};

        public StageObjArr(E_SPAWN_OBJ_TYPE obj1st, E_SPAWN_OBJ_TYPE obj2nd, E_SPAWN_OBJ_TYPE obj3rd)
        {
            spawnObjType[0] = obj1st;
            spawnObjType[1] = obj2nd;
            spawnObjType[2] = obj3rd;
        }
        public StageObjArr(): this(E_SPAWN_OBJ_TYPE.NOTHING, E_SPAWN_OBJ_TYPE.NOTHING, E_SPAWN_OBJ_TYPE.NOTHING)
        { }
    }
}