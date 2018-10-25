using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace hcp {


#if UNITY_EDITOR
    [CustomEditor(typeof(StageManager))]
    public class StageManagerEd : Editor
    {
        public Queue<StageTurnPointSet> turningPointQueue; //스테이지 에서 쓰이는 큐
        public Queue<StageObjArr> objQueue;//스테이지 에서 쓰이는 큐

        void OnEnable()
        {
            //Character 컴포넌트를 얻어오기
            if (StageST.turningPointQueue != null)
                turningPointQueue = StageST.turningPointQueue;
            if (StageST.objQueue != null)
                objQueue = StageST.objQueue;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (StageST.turningPointQueue != null)
            {
                turningPointQueue = StageST.turningPointQueue;

                foreach (var n in turningPointQueue)
                {
                    EditorGUILayout.LabelField(n.turningPoint.ToString(), n.whichTurn.ToString());
                }
            }
            if (StageST.objQueue != null)
            {
                objQueue = StageST.objQueue;
                foreach (var n in objQueue)
                {
                    EditorGUILayout.LabelField(n.spawnObjType[0].ToString(), n.spawnObjType[1].ToString());

                    EditorGUILayout.LabelField(n.spawnObjType[2].ToString());
                }
            }
        }
    }

#endif
    
    public static class StageManager{

        public static E_STAGE stageNum;
    }
}