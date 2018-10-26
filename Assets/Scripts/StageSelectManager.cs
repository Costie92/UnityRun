using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace hcp
{

    public class StageSelectManager : MonoBehaviour
    {
        int Count;
        public RectTransform Alert;
        // Use this for initialization
        void Start()
        {
            // 시작씬, 스테이지씬 제외
            Alert.gameObject.SetActive(false);
            Count = SceneManager.sceneCountInBuildSettings -2;
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OnClickStage(Button btn)
        {
            int StageNum = int.Parse(btn.GetComponentInChildren<Text>().text);
            switch (StageNum)
            {
                case 1: 
                    StageManager.stageNum = E_STAGE.STAGE_1;
                    break;
                case 2:
                    StageManager.stageNum = E_STAGE.STAGE_2;
                    break;
            }
            if (StageNum <= Count)
            {
                SceneManager.LoadScene(StageNum);
            }
            else {
                {
                    Alert.gameObject.SetActive(true);
                    LeanTween.textAlpha(Alert, 1f, 1.0f).setOnComplete(AlertComplete);
                    print("해당 씬이 없습니다.");
                }
            }
        }
        void AlertComplete() {
            LeanTween.textAlpha(Alert, 0f, 1.0f);
        }
    }
}
