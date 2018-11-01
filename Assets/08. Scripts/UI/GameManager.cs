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
    void Start()
    {
        _instance = this;
        
        if (StageManager.stageNum != E_STAGE.NONE)
        {
            if (IsThisStageClear.GetInstance().IsThisStageFirstPlay(StageManager.stageNum))
            {

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
