using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoinParabolaAdjust : MonoBehaviour {
    public GameObject huddle;
    public GameObject fire;
    public GameObject[] coinObjs;

    float coinLineHighPointY;
    float jumpStartPointZ;
    float a;
    Vector3 pos;

    private void Awake()
    {
        coinLineHighPointY = 3;
        jumpStartPointZ = 3;    //점프 시작위치에 맞춰서 설정

        a = -coinLineHighPointY / (jumpStartPointZ * jumpStartPointZ);

        pos = new Vector3();

        coinObjs = new GameObject[10];
        for (int i = 0; i < 10; i++)
        {
            coinObjs[i] = transform.GetChild(i).gameObject;
        }
        huddle = transform.Find("Huddle").gameObject;
        fire = transform.Find("Fire").gameObject;
    }
    private void OnEnable()
    {
        int g = Random.Range(0, 3);
        if (g.Equals(0))
        {
            huddle.SetActive(false);
            fire.SetActive(false);
            return;
        }
        if (g.Equals(1))
        {
            huddle.SetActive(true);
            fire.SetActive(false);
            return;
        }
        if (g.Equals(2))
        {
            huddle.SetActive(false);
            fire.SetActive(true);
            return;
        }
    }
    private void OnDisable()
    {  
        transform.position = Vector3.zero;
        float x = 0;
        float zp = 4.5f;
        float y=0;
        pos.x = x;
        
            for (int i = 0; i < coinObjs.Length; i++)
            {
                y = a * (zp*zp) + coinLineHighPointY;
                if (y < 0) y = 0;

                pos.y = y;
                pos.z = zp;

                coinObjs[i].transform.position = pos;
                zp -= 1;
            }

        huddle.transform.position = transform.position;
        huddle.transform.rotation = transform.rotation;
    }
}
