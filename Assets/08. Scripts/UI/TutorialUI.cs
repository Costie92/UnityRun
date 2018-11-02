using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour {

    public GameObject[] Pointers;
    private GameObject Player;
    private GameManager GameMgr;
    private float ZPos;
	// Use this for initialization
	void Start () {
        Pointers = new GameObject[6]; //LeftTap, RightTap, SwipeUp, SwipeDown, SwipeRight, SwipeLeft
        GameMgr = GameObject.Find("GameMgr").GetComponent<GameManager>();
        Player = GameObject.FindWithTag("PLAYER");
        for (int i = 0; i < 6; i++)
        {
            Pointers[i] = GameObject.Find("Tutorial").transform.GetChild(i).gameObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (GameMgr.NeedTuto)
        {
            ZPos = Player.transform.position.z;

            if (ZPos > 20 && ZPos < 40 && !Pointers[1].activeSelf)
            {
                Pointers[1].SetActive(true);
                LeanTween.scale(Pointers[1], new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setLoopPingPong();
            }
            if (ZPos > 40 && Pointers[1].activeSelf)
            {
                LeanTween.reset();
                Pointers[1].SetActive(false);
            }

            if (ZPos > 70 && ZPos < 90 && !Pointers[0].activeSelf)
            {
                Pointers[0].SetActive(true);
                LeanTween.scale(Pointers[0], new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setLoopPingPong();
            }
            if (ZPos > 90 && Pointers[0].activeSelf)
            {
                LeanTween.reset();
                Pointers[0].SetActive(false);
            }

            if (ZPos > 150 && ZPos < 165 && !Pointers[4].activeSelf)
            {
                Pointers[4].SetActive(true);
                LeanTween.moveLocalX(Pointers[4], 300, 1f).setLoopClamp();
            }
            if (ZPos > 165 && Pointers[4].activeSelf)
            {
                LeanTween.reset();
                Pointers[4].SetActive(false);
            }

            if (ZPos > 185 && ZPos < 205 && !Pointers[3].activeSelf)
            {
                Pointers[3].SetActive(true);
                LeanTween.moveLocalY(Pointers[3], 250, 0.75f).setLoopClamp();
            }
            if (ZPos > 205 && Pointers[3].activeSelf)
            {
                LeanTween.reset();
                Pointers[3].SetActive(false);
            }

            if (ZPos > 245 && ZPos < 265 && !Pointers[2].activeSelf)
            {
                Pointers[2].SetActive(true);
                LeanTween.moveLocalY(Pointers[2], 250, 0.75f).setLoopClamp();
            }
            if (ZPos > 265 && Pointers[2].activeSelf)
            {
                LeanTween.reset();
                Pointers[2].SetActive(false);
            }

            if (ZPos > 310 && ZPos < 325 && !Pointers[5].activeSelf)
            {
                Pointers[5].SetActive(true);
                LeanTween.moveLocalX(Pointers[5], -300, 1f).setLoopClamp();
            }
            if (ZPos > 325 && Pointers[5].activeSelf)
            {
                LeanTween.reset();
                Pointers[5].SetActive(false);
            }
        }
	}
}
