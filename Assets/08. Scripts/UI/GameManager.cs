using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hcp;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    // Use this for initialization
    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("GameManager is NULL");
            return _instance;
        }
    }

    public int coins;
    public bool NeedTuto = false;
    void Start()
    {
        _instance = this;
        if (Application.platform == RuntimePlatform.Android)
        {
            Application.targetFrameRate = 60;
        }
        if (StageManager.stageNum != E_STAGE.NONE)
        {
            if (IsThisStageClear.GetInstance().IsThisStageFirstPlay(StageManager.stageNum))
            {
                //1스테이지 처음 플레이시 튜토리얼 시작
                if (StageManager.stageNum == E_STAGE.STAGE_1)
                {
                    NeedTuto = true;
                }
                coins = 0;
            }
            else
            {
                //처음 플레이가 아닐 record 코인 받아오기
                StageClearDataST scdst = null;
                scdst = IsThisStageClear.GetInstance().GetClearDataOfThisStage(StageManager.stageNum);
                coins = scdst.coins;

            }
        }
    }
	// Update is called once per frame
	void Update () {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    public void ClearStage()
    {
        //클리어시 coin값 비교 후 record에 올리고 클리어 값 true로 변경
        if (ObjEat.Coin >= coins) {
            coins = ObjEat.Coin;
        }
        IsThisStageClear.GetInstance().SaveClearData(StageManager.stageNum, true, coins);
    }

}
