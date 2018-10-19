using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
        public  enum E_ITEM 
        {
            HPPLUS = 0,
            INVINCIBLE,
            SHIELD,
            COIN,
            MAGNET,
            EITEMMAX
        };

        public  enum E_OBSTACLE 
        {
            BALL = 0,
            HUDDLE,
            UPPER_HUDDLE,
            FIRE,
            EOBSMAX
        };
    public enum E_SPAWN_OBJ_TYPE
    {
        HPPLUS = 0,
        INVINCIBLE,
        SHIELD,
        COIN,
        COIN_STRAIGHT,
        COIN_PARABOLA,
        MAGNET,

        BALL,
        HUDDLE,
        UPPER_HUDDLE_1,
        UPPER_HUDDLE_2,
        UPPER_HUDDLE_3,
        FIRE,

        NOTHING,
        EOBJTYPEMAX
    };
    [System.Serializable]
    public class ChunkObjST //여기서 직접 장애물들 가져오자 그냥.  (무한모드일때)
    {
        public float position;
        public GameObject chunk = null;
        public List<GameObject> objs = new List<GameObject>();//스폰포인트 넘버 상관 없이 오브젝트만 다 관리
        //코인 라인도 프리팹 했으니 리스트 말고 배열을 쓰는것도 고려해볼것.

        public void ObjSpawn(E_OBJ_SPAWN_WAY way)
        {
            if (chunk == null)
            {
                ErrorManager.SpurtError("청크도 없는데 오브젝트생성하려고 함");
                return;
            }
            if (objs.Count > 0) objs.Clear();

            Transform spg = chunk.transform.GetChild(3);  
            //3은 스폰포인트 그룹의 자식 넘버 순서에 상당히 의존한 값이므로 필히 조심!
            
            switch (way)
            {
                case E_OBJ_SPAWN_WAY.RANDOM:
                            RandomObjGenerator.GetInstance().RandomObjGen(spg, ref objs);
                    break;

                default: break;
            }
        }

        public bool IsEmpty()
        {
            if (chunk == null && objs.Count == 0)
                return true;
            else return false;
        }
        
        public void PutInObj(GameObject obj)
        {
            if (objs != null)
                objs.Add(obj);
        }
        public void PutInObj(List<GameObject> objList)
        {
            if (objList != null)
                objs.AddRange(objList);
        }
        public void Reset()
        {
            position = -1;
            if (chunk != null)
            {
                MapAndObjPool.GetInstance().TurnInPoolObj(chunk);
                chunk = null;
            }
            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i] != null && objs[i].activeSelf == true)
                    MapAndObjPool.GetInstance().TurnInPoolObj(objs[i]);
            }
            objs.Clear();//참조만 날라감. 괜찮음.
        }
    }

    [System.Serializable]
    public class ItemST
    {
        public E_ITEM itemType { set; get; }
        public float value { set; get; }
    };
    [System.Serializable]
    public class ObstacleST
    {
        public E_OBSTACLE obstacleType { set; get; }
        public bool beenHit {set;get;}
    };


    [System.Serializable]
    public class MapObjST   //얘는 저장용으로.
    {
        public float? keyPos=null;
        E_SPAWN_OBJ_TYPE[] spawnObjType=
            { E_SPAWN_OBJ_TYPE.NOTHING, E_SPAWN_OBJ_TYPE.NOTHING, E_SPAWN_OBJ_TYPE.NOTHING,};
    };
}