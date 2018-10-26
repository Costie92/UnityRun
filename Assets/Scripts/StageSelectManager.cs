using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace hcp
{
    public class StageSelectManager : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OnClickStage(Button btn)
        {
            switch (int.Parse(btn.GetComponentInChildren<Text>().text))
            {
                case 1: 
                    StageManager.stageNum = E_STAGE.STAGE_1;
                    SceneManager.LoadScene("STAGE_1");
                    break;
                case 2:
                    StageManager.stageNum = E_STAGE.STAGE_2;
                    SceneManager.LoadScene(2);
                    break;
            }
        }
    }
}
