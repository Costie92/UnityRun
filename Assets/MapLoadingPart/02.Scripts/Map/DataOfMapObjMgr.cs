using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace hcp {
    [CustomEditor(typeof(DataOfMapObjMgr))]
    public class ObjInChageInspector : Editor
    {
        List<ChunkObjST> toShowDic;

        void OnEnable()
        {
            //Character 컴포넌트를 얻어오기
            if (DataOfMapObjMgr.chunkObjSTList != null)
                toShowDic = DataOfMapObjMgr.chunkObjSTList;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (DataOfMapObjMgr.chunkObjSTList != null)
                toShowDic = DataOfMapObjMgr.chunkObjSTList;
            foreach (var n in toShowDic)
            {
                foreach(var m in n.objs)
                EditorGUILayout.LabelField(n.position.ToString(),m.name);

            }
        }
    }

    


    public class DataOfMapObjMgr : SingletonTemplate<DataOfMapObjMgr> {

        public static List<ChunkObjST> chunkObjSTList = new List<ChunkObjST>();

        private static List<ChunkObjST> chunkObjSTPool = new List<ChunkObjST>();

        List<float> posList = new List<float>();

        public List<float> GetPosList()
        {
            posList.Clear();
            for (int i = 0; i < chunkObjSTList.Count; i++)
            {
                posList.Add(chunkObjSTList[i].position);
            }
            return posList;
        }

        public void TurnInChunkObjSTPool(ChunkObjST temp)
        {
            if (!temp.IsEmpty())
            {
                temp.Reset();
            }
            if (chunkObjSTList.Remove(temp))
            {
                chunkObjSTPool.Add(temp);
            }
            else ErrorManager.SpurtError("청크 풀에 반납시 문제001");
        }

        public void TurnInChunkObjSTPool(float pos)
        {
            for (int i = 0; i < chunkObjSTList.Count; i++)
            {
                if (chunkObjSTList[i].position == pos && !chunkObjSTList[i].IsEmpty())
                {
                    TurnInChunkObjSTPool(chunkObjSTList[i]);
                    return;
                }
            }
            ErrorManager.SpurtError("문제여문제");

        }

        public void ChunkObjSTPoolInit(int capacity =10)
        {
            for (int i = 0; i < capacity; i++)
            {
                ChunkObjST temp = new ChunkObjST();
                temp.Reset();
                chunkObjSTPool.Add(temp);
            }
        }

        public ChunkObjST GetEmptyChunkObjSTFromPool()
        {
            ChunkObjST temp;
            for (int i = 0; i < chunkObjSTPool.Count; i++)
            {
                if (chunkObjSTPool[i].IsEmpty())
                {
                    temp = chunkObjSTPool[i]; 
                    chunkObjSTList.Add(chunkObjSTPool[i]);
                    chunkObjSTPool.RemoveAt(i);
                    return temp;
                }
            }
            return null;
        }

        public ChunkObjST GetEmptyChunkObjSTFromPool(GameObject chunk,float pos)
        {
            if (chunk == null) ErrorManager.SpurtError("청크가 널임");
            ChunkObjST temp;
            for (int i = 0; i < chunkObjSTPool.Count; i++)
            {
                if (chunkObjSTPool[i].IsEmpty())
                {
                    temp = chunkObjSTPool[i];
                    chunkObjSTList.Add(chunkObjSTPool[i]);
                    chunkObjSTPool.RemoveAt(i);

                    temp.chunk = chunk;
                    temp.position = pos;

                    return temp;
                }
            }
            return null;
        }



        public static Queue<MapObjST> MapObjSTQue = new Queue<MapObjST>();   //맵 옵젝 저장구조.

        public static Queue<float> TurningPointQue = new Queue<float>();   //저장되있는걸 가져와서 터닝포인트큐와 맵옵젝큐에 넣음

        protected override void Awake()
        {
            base.Awake();
            ChunkObjSTPoolInit();
        }

    }
}