using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace hcp {
    [CustomEditor(typeof(ObjInCharge))]
    public class ObjInChageInspector : Editor
    {
        Dictionary<float, List<GameObject>> toShowDic;

        void OnEnable()
        {
            //Character 컴포넌트를 얻어오기
            if (DataOfMapObjMgr.spawnedObjDic != null)
                toShowDic = DataOfMapObjMgr.spawnedObjDic;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            foreach (var n in toShowDic)
            {
                foreach (var gameObj in n.Value)
                {
                    if (gameObj)
                        EditorGUILayout.LabelField(n.Key.ToString(), gameObj.name);
                }
            }
        }
    }


    public class DataOfMapObjMgr : SingletonTemplate<DataOfMapObjMgr> {
        //청크 관리 딕셔너리
        public static Dictionary<float, GameObject> chunkOnMap = new Dictionary<float, GameObject>();
        //게임 아이템,장애물 관리
        public static Dictionary<float, List<GameObject>> spawnedObjDic= new Dictionary<float, List<GameObject>>();
        //아이템,장애물 관리 리스트들 
        private static List<GameObject>[] genedObjLists;

        public static Queue<MapObjST> MapObjSTQue = new Queue<MapObjST>();   //맵 옵젝 저장구조.

        public static Queue<float> TurningPointQue = new Queue<float>();   //저장되있는걸 가져와서 터닝포인트큐와 맵옵젝큐에 넣음

        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            int i = MapObjManager.GetInstance().wantToShowNumOfChunks;
            genedObjLists = new List<GameObject>[i];
            for (int j = 0; j < i; j++)
            {
                genedObjLists[j] = new List<GameObject>();
            }
        }
        public static List<GameObject> FindEmptyGList()
        {
            for (int i = 0; i < genedObjLists.Length; i++)
            {
                if (genedObjLists[i].Count == 0)
                    return genedObjLists[i];
            }
            return null;
        }
    }
}