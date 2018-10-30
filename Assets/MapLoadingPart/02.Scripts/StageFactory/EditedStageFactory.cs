using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp {
    public class EditedStageFactory : MonoBehaviour {
        List<StageEditorST> editedList;

        // Use this for initialization
        void Start() {
            editedList = StageDataMgr.GetInstance().LoadData(StageManager.fileNameForEdit);
            EditedStageMaking();
        }
        void EditedStageMaking()
        {
            StageST.InitForStageLoading();
            if (editedList == null) ErrorManager.SpurtError("파일이 없음");

            editedList.Sort(
                delegate (StageEditorST x, StageEditorST y)
                {
                    return x.pos.CompareTo(y.pos);
                }
                );

            for (int i = 0; i < editedList.Count; i++)
            {
                if (editedList[i].IsTurnChunks())   //턴 청크면
                {
                    StageST.EnqueStageTurningPoint(editedList[i].GetRealTurningPoint(), editedList[i].whichTurn);
                }
                else {

                    StageST.EnqueStageObjs(editedList[i].soa.spawnObjType[0],
                        editedList[i].soa.spawnObjType[1],
                        editedList[i].soa.spawnObjType[2]);
                }
            }
            editedList.Clear();
        }
    }
}