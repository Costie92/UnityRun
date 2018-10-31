using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using hcp;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;
    public static UIManager instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UIManager is NULL");
            return _instance;
        }
    }

    private TouchScreenKeyboard keyboard;
    public UnityAdsHelper adsHelper;

    public static bool isPause;
    public GameObject[] Hps;
    public GameObject UI_Shield;
    public GameObject UI_Magnet;
    public GameObject UI_Invincible;
    public GameObject PauseMenu;
    public GameObject Result;
    public Text CoinText;
    public Button Btn_Pause;
    public Button Btn_Resume;
    public Button Btn_Quit;
    public Button Btn_Retry;
    public Button Btn_Exit;


    // Use this for initialization
    private void Awake()
    {
        Hps = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            Hps[i] = GameObject.Find("Hps").transform.GetChild(i).gameObject;
        }

        UI_Shield = GameObject.Find("Shield");
        UI_Magnet = GameObject.Find("Magnet");
        UI_Invincible = GameObject.Find("Invincible");
        PauseMenu = GameObject.Find("PauseMenu");
        Result = GameObject.Find("Result");

        CoinText = GameObject.Find("CoinCount").GetComponent<Text>();
        Btn_Pause = GameObject.Find("Btn_Pause").GetComponent<Button>();
        Btn_Resume = GameObject.Find("Btn_Resume").GetComponent<Button>();
        Btn_Quit = GameObject.Find("Btn_Quit").GetComponent<Button>();
        Btn_Retry = GameObject.Find("Btn_Retry").GetComponent<Button>();
        Btn_Exit = GameObject.Find("Btn_Exit").GetComponent<Button>();

        Btn_Pause.onClick.AddListener(() => OnClickPause());
        Btn_Resume.onClick.AddListener(() => OnClickResume());
        Btn_Quit.onClick.AddListener(() => OnClickQuit());
        Btn_Retry.onClick.AddListener(() => OnClickRetry());
        Btn_Exit.onClick.AddListener(() => OnClickExit());
    }
    void Start()
    {
        adsHelper = this.gameObject.GetComponent<UnityAdsHelper>();
        print(StageManager.stageNum.ToString());
        isPause = false;
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        Result.SetActive(false);
        CoinText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        CoinText.text = ObjEat.Coin.ToString();
        DisplayHp();
        UI_Invincible.SetActive(ObjEat.Invincible);
        UI_Shield.SetActive(ObjEat.Shield);
        UI_Magnet.SetActive(ObjEat.Magnet);
    }

    void DisplayHp()
    {
        if (ObjEat.HP >= 0)
        {
            for (int i = 0; i < ObjEat.HP; i++)
            {
                Hps[i].SetActive(true);
            }
            for (int j = ObjEat.HP; j < 5; j++)
            {
                Hps[j].SetActive(false);
            }
        }
    }
    void OnClickPause()
    {
        isPause = true;
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
        //keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);

    }
    void OnClickResume()
    {
        isPause = false;
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    void OnClickQuit()
    {
        ShowResult();
    }
    void OnClickExit()
    {
        isPause = false;
        Time.timeScale = 1;
        //adsHelper.ShowRewardedAd();
        SceneManager.LoadScene("StageSelect");        
    }
    void OnClickRetry()
    {
        
        isPause = false;
        Time.timeScale = 1;
        if (StageManager.stageNum == E_STAGE.NONE)
        {
            SceneManager.LoadScene(Constants.editedStageSceneName);
        }
        else
        {
            SceneManager.LoadScene(StageManager.stageNum.ToString());
        }
    }
    public void ShowResult()
    {
        isPause = true;
        Result.SetActive(true);
        Result.transform.Find("ResultCoin").GetComponent<Text>().text = CoinText.text;
        Time.timeScale = 0;
    }
}