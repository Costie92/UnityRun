using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    public GameObject[] Hps;
    public GameObject UI_Shield;
    public GameObject UI_Magnet;
    public GameObject UI_Invincible;
    public Text CoinText;

    // Use this for initialization
    void Start () {
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
}
