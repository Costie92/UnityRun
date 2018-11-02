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
        
        if (StageManager.stageNum != E_STAGE.NONE)
        {
            print("aa");
            if (IsThisStageClear.GetInstance().IsThisStageFirstPlay(StageManager.stageNum))
            {
                print("bb");
                if (StageManager.stageNum == E_STAGE.STAGE_1)
                {
                    print("cc");
                    NeedTuto = true;
                }
                coins = ObjEat.Coin;
            }
            else
            {
                StageClearDataST scdst = null;
                scdst = IsThisStageClear.GetInstance().GetClearDataOfThisStage(StageManager.stageNum);
                coins = scdst.coins;

            }
        }
    }
	// Update is called once per frame
	void Update () {
		
	}

    public void ClearStage()
    {
        print("Saved");
        if (ObjEat.Coin >= coins) {
            coins = ObjEat.Coin;
        }
        IsThisStageClear.GetInstance().SaveClearData(StageManager.stageNum, true, coins);
    }

}
