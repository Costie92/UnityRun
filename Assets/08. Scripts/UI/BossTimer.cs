using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTimer : MonoBehaviour
{
    private BossAttack bossAttack;
    public Text Timer;
    private float TimeCount;
    private bool BossFly = false;
    // Use this for initialization
    void Start()
    {
        bossAttack = GameObject.Find("DragonBoss").GetComponent<BossAttack>();
        Time.timeScale = 1;
        BossFly = false;
        TimeCount = 30.0f;
        Timer = this.GetComponent<Text>();
        Timer.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeCount > 0)
        {
            TimeCount -= Time.deltaTime;
        }
        else if (TimeCount <= 0)
        {
            TimeCount = 0.0f;
        }
        if (TimeCount < 10) {
            Timer.color = Color.red;
        }
        if (!BossFly && TimeCount <= 4.0f)
        {
            BossFly = true;
            bossAttack.End();
        }
        Timer.text = TimeCount.ToString("0.0");
    }
}
