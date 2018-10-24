using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    [System.Serializable]
    public class ChunkObjST
    {
        public float position;
        public GameObject chunk = null;
        public List<GameObject> objs = new List<GameObject>();//스폰포인트 넘버 상관 없이 오브젝트만 다 관리
                                                              //코인 라인도 프리팹 했으니 리스트 말고 배열을 쓰는것도 고려해볼것.

        public void ObjSpawn(E_STAGE stage)
        {
            if (chunk == null)
            {
                ErrorManager.SpurtError("청크도 없는데 오브젝트생성하려고 함");
                return;
            }
            if (objs.Count > 0) objs.Clear();

            Transform spg = chunk.transform.GetChild(3);
            //3은 스폰포인트 그룹의 자식 넘버 순서에 상당히 의존한 값이므로 필히 조심!

            ObjGenerator.ObjGen(stage, spg, ref objs);
        }

        public bool IsEmpty()
        {
            if (chunk == null && objs.Count == 0)
                return true;
            else return false;
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
}