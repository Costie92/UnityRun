using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace hcp
{
    public class StageSelectManager : MonoBehaviour
    {

        StageManager SManager;
        // Use this for initialization
        void Start()
        {
            SManager = GameObject.Find("StageManager").GetComponent<StageManager>();
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
                    SManager.StageNum = E_STAGE.STAGE_1;
                    SceneManager.LoadScene("STAGE_1");
                    break;
                case 2:
                    SManager.StageNum = E_STAGE.STAGE_2;
                    SceneManager.LoadScene(2);
                    break;
            }
        }
    }
}
