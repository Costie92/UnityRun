﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;


public class UIManager : MonoBehaviour {
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
    public static bool isPause;
    public GameObject[] Hps;
    public GameObject UI_Shield;
    public GameObject UI_Magnet;
    public GameObject UI_Invincible;
    public GameObject PauseMenu;
    public GameObject Result;
    public Text CoinText;
    private TouchScreenKeyboard keyboard;

    // Use this for initialization
    private void Awake()
    {
        Hps = new GameObject[5];
        for (int i = 0; i < 5; i++) {
            Hps[i] = GameObject.Find("Hps").transform.GetChild(i).gameObject;
        }
        UI_Shield = GameObject.Find("Shield");
        UI_Magnet = GameObject.Find("Magnet");
        UI_Invincible = GameObject.Find("Invincible");
        PauseMenu = GameObject.Find("PauseMenu");
        Result = GameObject.Find("Result");
        CoinText = GameObject.Find("CoinCount").GetComponent<Text>();
    }
    void Start() {
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

    public void DisplayHp() {
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
    public void OnClickPause()
    {
        isPause = true;
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        //keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);

    }
    public void OnClickResume()
    {
        isPause = false;
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }
    public void OnClickQuit() {
        ShowResult();
    }
    public void OnClickExit() {
        isPause = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("StageSelect");
    }
    public void OnClickRetry() {
        isPause = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void ShowResult() {
        isPause = true;
        Time.timeScale = 0;
        Result.SetActive(true);
        Result.transform.Find("ResultCoin").GetComponent<Text>().text = CoinText.text;
    }
}
