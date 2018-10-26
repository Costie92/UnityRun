using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
namespace hcp
{
    public class StageEditorUIMgr : SingletonTemplate<StageEditorUIMgr>
    {
        StageEditorMgr sem;
        Text posValue;
        GameObject mainCanvas;
        GameObject ObjSelectPalette;
        GameObject ChunkSelectPalette;
        GameObject basicBtnGrp;
        GameObject WarningEditorExitAtThisPos;
        int nowChangeObjNum = -1;

        protected override void Awake()
        {
            base.Awake();
          
        }
        private void Start()
        {
            sem = GameObject.Find("StageEditMgr").GetComponent<StageEditorMgr>();
            mainCanvas = GameObject.Find("Canvas");
            posValue = mainCanvas.transform.Find("PosValueText").GetComponent<Text>();
            ObjSelectPalette = mainCanvas.transform.Find("ObjSelect").gameObject;
            ChunkSelectPalette = mainCanvas.transform.Find("ChunkSelect").gameObject;
            basicBtnGrp = mainCanvas.transform.Find("BasicBtnGroup").gameObject;
            WarningEditorExitAtThisPos = mainCanvas.transform.Find("WarningEditorExitAtThisPos").gameObject;
            ShowReset();
        }
         public void ChangePosText()
        {
            posValue.text = sem.Position.ToString();
        }
        void ShowReset()
        {
            WarningEditorExitAtThisPos.SetActive(false);
            ObjSelectPalette.SetActive(false);
            ChunkSelectPalette.SetActive(false);
            basicBtnGrp.SetActive(true);
            nowChangeObjNum = -1;
        }

        public void OnBtnForwardClick()
        {
            ShowReset();
            sem.MoveForward();
            ChangePosText();
            
        }
        public void OnBtnBackwardClick()
        {
            ShowReset();
            sem.MoveBackward();
            ChangePosText();
            
        }
        public void OnPosChangeByKeyboard(int number)
        {
            ShowReset();
            sem.MoveByNumber(number);
            ChangePosText();
          
        }
        public void OnChangeObj(int objSpawnNum)
        {
            nowChangeObjNum = objSpawnNum;

            basicBtnGrp.SetActive(false);
            ObjSelectPalette.SetActive(true);
        }
        public void OnBtnSelectExitClick()
        {
            ShowReset();
        }
        public void OnChunkSelectPaletteCallClick()
        {
            ShowReset();
            basicBtnGrp.SetActive(false);
            ChunkSelectPalette.SetActive(true);
        }
        public void OnChunkChangeClick(Button btn)
        {
            E_WhichTurn whichTurn=E_WhichTurn.NOT_TURN;

            switch (btn.name)
            {
                case "Btn_Left":
                    whichTurn = E_WhichTurn.LEFT;
                    break;
                case "Btn_NotTurn":
                    whichTurn = E_WhichTurn.NOT_TURN;
                    break;
                case "Btn_Right":
                    whichTurn = E_WhichTurn.RIGHT;
                    break;
            }
            sem.ChangeFloorChunk(whichTurn);
            ShowReset();
        }
        public void OnObjClick(Button btn)
        {
            if (nowChangeObjNum == -1) ErrorManager.SpurtError("스폰 넘버 로직 오류!");
            E_SPAWN_OBJ_TYPE spawnType=E_SPAWN_OBJ_TYPE.NOTHING;
            //버튼 이름으로 분기해서
            switch (btn.name)
            {
                case "Magnet":
                    spawnType = E_SPAWN_OBJ_TYPE.MAGNET;
                    break;
                case "Shield":
                    spawnType = E_SPAWN_OBJ_TYPE.SHIELD;
                    break;
                case "Invincible":
                    spawnType = E_SPAWN_OBJ_TYPE.INVINCIBLE;
                    break;
                case "Hp":
                    spawnType = E_SPAWN_OBJ_TYPE.HPPLUS;
                    break;
                case "Coin":
                    spawnType = E_SPAWN_OBJ_TYPE.COIN;
                    break;
                case "Btn_CoinStraight":
                    spawnType = E_SPAWN_OBJ_TYPE.COIN_STRAIGHT;
                    break;
                case "Btn_CoinParabola":
                    spawnType = E_SPAWN_OBJ_TYPE.COIN_PARABOLA;
                    break;


                case "Btn_Huddle":
                    spawnType = E_SPAWN_OBJ_TYPE.HUDDLE;
                    break;
                case "Btn_Ball":
                    spawnType = E_SPAWN_OBJ_TYPE.BALL;
                    break;
                case "Btn_Fire":
                    spawnType = E_SPAWN_OBJ_TYPE.FIRE;
                    break;
                case "Btn_Upper_1":
                    spawnType = E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_1;
                    break;
                case "Btn_Upper_2":
                    spawnType = E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_2;
                    break;
                case "Btn_Upper_3":
                    spawnType = E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_3;
                    break;
            }
            sem.ChangeObjThisChunk(nowChangeObjNum, spawnType);
            ShowReset();
        }
        public void OnExitNowPosBtnClick()
        {
            //경고 메세지 띄우rl
            ShowReset();
            WarningEditorExitAtThisPos.SetActive(true);
        }
        public void OnYesExitNowPosBtnClick()
        {
            //현재 시점에서 에디터 종료 버튼 눌렸을때
            sem.ForcedEditFinishedAtThisPoint();
            //에디터 종료하고 씬 로드 해주는 거 잊지말기!!
            sem.EditorIsDone();
        }

        public void OnEditSaveAndExitClick()
        {
            sem.EditorIsDone();
        }
    }
}