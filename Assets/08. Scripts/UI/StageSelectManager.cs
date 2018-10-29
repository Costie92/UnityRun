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
        public GameObject StageSelect;
        public GameObject EditSelect;
        public GameObject StageMenu;
        public GameObject EditMenu;
        public Vector3 ScaleV = new Vector3(1.2f, 1.2f, 1.2f);
        public Vector3 NormalV = new Vector3(1, 1, 1);
        // Use this for initialization
        void Start()
        {
            StageMenu.SetActive(false);
            EditMenu.SetActive(false);
            // 시작씬, 스테이지씬 제외
            Alert.gameObject.SetActive(false);
            Count = SceneManager.sceneCountInBuildSettings -2;
        }
        // Update is called once per frame
        void Update()
        {
            
        }
        public void OnClickSelect(GameObject Select)
        {
            if (Select == EditSelect)
            {
                EditMenu.SetActive(true);
            }
            else {
                StageMenu.SetActive(true);
            }
            LeanTween.reset();
            ResizeScale();
            LeanTween.scale(Select, ScaleV, 1.0f).setLoopPingPong();
            
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
                    if (Alert.GetComponent<Text>().color.a == 0)
                    {
                        LeanTween.textAlpha(Alert, 1f, 1.0f).setOnComplete(AlertComplete);
                    }
                    print("해당 씬이 없습니다.");
                }
            }
        }
        void AlertComplete() {
            LeanTween.textAlpha(Alert, 0f, 1.0f);
        }

        void ResizeScale() {
            StageSelect.transform.localScale = NormalV;
            EditSelect.transform.localScale = NormalV;
        }
    }
}
