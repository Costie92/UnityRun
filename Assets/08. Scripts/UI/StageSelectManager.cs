using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using hcp;

public class StageSelectManager : MonoBehaviour
{
    int Count;
    public RectTransform Alert;
    public GameObject StageSelect;
    public GameObject EditSelect;
    public GameObject StageMenu;
    public GameObject EditMenu;

    public Vector3 ScaleV = new Vector3(1.2f, 1.2f, 1.2f);
    public Vector3 NormalV = new Vector3(1, 1, 1);

    // Use this for initialization
    private void Awake()
    {
        StageSelect = GameObject.Find("StageSelect");
        EditSelect = GameObject.Find("EditSelect");
        StageMenu = GameObject.Find("StageMenu");
        EditMenu = GameObject.Find("EditMenu");
    }
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            EditMenu.transform.GetChild(i).gameObject.SetActive(false);
        }
        StageMenu.SetActive(false);
        EditMenu.SetActive(false);
        Alert.gameObject.SetActive(false);
        // 시작씬, 에디터씬 제외
        Count = SceneManager.sceneCountInBuildSettings - 5;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickSelect(GameObject Select)
    {
        if (Select == EditSelect)
        {
            StageMenu.SetActive(false);
            EditMenu.SetActive(true);
        }
        else
        {
            StageMenu.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                EditMenu.transform.GetChild(i).gameObject.SetActive(false);
            }
            EditMenu.SetActive(false);
        }
        LeanTween.reset();
        ResizeScale();
        LeanTween.scale(Select, ScaleV, 1.0f).setLoopPingPong();
    }
    public void OnClickCreateModify()
    {
        SceneManager.LoadScene(Constants.stageEditorSceneName);
    }
    public void OnClickPlay()
    {
        SceneManager.LoadScene(Constants.editedStageSceneName);
    }
    public void OnClickEdit(Button btn)
    {
        int StageNum = int.Parse(btn.GetComponentInChildren<Text>().text);
        StageManager.stageNum = E_STAGE.NONE;
        switch (StageNum)
        {
            case 1:
                StageManager.fileNameForEdit = Constants.editStage_1_fileName;
                break;
            case 2:
                StageManager.fileNameForEdit = Constants.editStage_2_fileName;
                break;
            case 3:
                StageManager.fileNameForEdit = Constants.editStage_3_fileName;
                break;
            case 4:
                StageManager.fileNameForEdit = Constants.editStage_4_fileName;
                break;
            case 5:
                StageManager.fileNameForEdit = Constants.editStage_5_fileName;
                break;
            case 6:
                StageManager.fileNameForEdit = Constants.editStage_6_fileName;
                break;
            case 7:
                StageManager.fileNameForEdit = Constants.editStage_7_fileName;
                break;
            case 8:
                StageManager.fileNameForEdit = Constants.editStage_8_fileName;
                break;
            case 9:
                StageManager.fileNameForEdit = Constants.editStage_9_fileName;
                break;
            case 10:
                StageManager.fileNameForEdit = Constants.editStage_10_fileName;
                break;
            case 11:
                StageManager.fileNameForEdit = Constants.editStage_11_fileName;
                break;
            case 12:
                StageManager.fileNameForEdit = Constants.editStage_12_fileName;
                break;
            case 13:
                StageManager.fileNameForEdit = Constants.editStage_13_fileName;
                break;
            case 14:
                StageManager.fileNameForEdit = Constants.editStage_14_fileName;
                break;
            case 15:
                StageManager.fileNameForEdit = Constants.editStage_15_fileName;
                break;
            case 16:
                StageManager.fileNameForEdit = Constants.editStage_16_fileName;
                break;
        }
        if (StageDataMgr.IsFileExist(StageManager.fileNameForEdit))
        {
            EditMenu.transform.GetChild(0).gameObject.SetActive(false);
            EditMenu.transform.GetChild(1).gameObject.SetActive(true);
            EditMenu.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            EditMenu.transform.GetChild(0).gameObject.SetActive(true);
            EditMenu.transform.GetChild(1).gameObject.SetActive(false);
            EditMenu.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
    public void OnClickStage(Button btn)
    {
        int StageNum = int.Parse(btn.GetComponentInChildren<Text>().text);
        string StageName = "STAGE_" + StageNum;
        StageManager.fileNameForEdit = null;
        switch (StageNum)
        {
            case 1:
                StageManager.stageNum = E_STAGE.STAGE_1;
                break;
            case 2:
                StageManager.stageNum = E_STAGE.STAGE_2;
                break;
            case 3:
                StageManager.stageNum = E_STAGE.STAGE_3;
                break;
            case 4:
                StageManager.stageNum = E_STAGE.STAGE_4;
                break;
            case 5:
                StageManager.stageNum = E_STAGE.STAGE_5;
                break;
            case 6:
                StageManager.stageNum = E_STAGE.STAGE_6;
                break;
            case 7:
                StageManager.stageNum = E_STAGE.STAGE_7;
                break;
            case 8:
                StageManager.stageNum = E_STAGE.STAGE_8;
                break;
            case 9:
                StageManager.stageNum = E_STAGE.STAGE_9;
                break;
            case 10:
                StageManager.stageNum = E_STAGE.STAGE_10;
                break;
            case 11:
                StageManager.stageNum = E_STAGE.STAGE_11;
                break;
            case 12:
                StageManager.stageNum = E_STAGE.STAGE_12;
                break;
            case 13:
                StageManager.stageNum = E_STAGE.STAGE_13;
                break;
            case 14:
                StageManager.stageNum = E_STAGE.STAGE_14;
                break;
            case 15:
                StageManager.stageNum = E_STAGE.STAGE_15;
                break;
            case 16:
                StageManager.stageNum = E_STAGE.STAGE_16;
                break;
        }
        if (StageNum <= Count)
        {
            SceneManager.LoadScene(StageName);
        }
        else
        {
            {
                Alert.gameObject.SetActive(true);
                if (Alert.GetComponent<Text>().color.a == 0)
                {
                    LeanTween.textAlpha(Alert, 1f, 1.0f).setOnComplete(AlertComplete);
                }
                print("해당 씬이 없습니다.");
            }
        }
    }
    void AlertComplete()
    {
        LeanTween.textAlpha(Alert, 0f, 1.0f);
    }

    void ResizeScale()
    {
        StageSelect.transform.localScale = NormalV;
        EditSelect.transform.localScale = NormalV;
    }
}