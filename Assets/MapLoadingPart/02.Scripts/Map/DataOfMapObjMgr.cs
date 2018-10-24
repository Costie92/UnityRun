using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace hcp {
#if UNITY_EDITOR
    [CustomEditor(typeof(DataOfMapObjMgr))]
    public class DataOfMapObjMgrInspector : Editor
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
#endif



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

        public void ChunkObjSTPoolInit(int capacity =Constants.wantToShowNumOfChunks+2)
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

        

        protected override void Awake()
        {
            base.Awake();
            chunkObjSTList.Clear();
            chunkObjSTPool.Clear();

            ChunkObjSTPoolInit();
        }

    }
}