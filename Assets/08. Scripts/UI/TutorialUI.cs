using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour {

    public GameObject[] Pointers;
    private GameObject Player;
    private float ZPos;
	// Use this for initialization
	void Start () {
        Pointers = new GameObject[6]; //LeftTap, RightTap, SwipeUp, SwipeDown, SwipeRight, SwipeLeft
        Player = GameObject.FindWithTag("PLAYER");
        for (int i = 0; i < 6; i++)
        {
            Pointers[i] = GameObject.Find("Tutorial").transform.GetChild(i).gameObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
        ZPos = Player.transform.position.z;
        if (ZPos > 20 && ZPos < 40 && !Pointers[1].activeSelf)
        {
            Pointers[1].SetActive(true);
            LeanTween.scale(Pointers[1], new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setLoopPingPong();
        }
        else if (ZPos > 40) {
            Pointers[1].SetActive(false);
        }
	}
}
