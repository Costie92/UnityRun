using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTap : MonoBehaviour {

    int TapCount;
    public float MaxDubbleTapTime;
    float TapTime;
    //public SimpleTouchController Controller;
    private float MousePosX;
    // Use this for initialization
    void Start() {
        TapTime = MaxDubbleTapTime;
        TapCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TapTime > 0 && TapCount == 1)
            {
                print("double Tap");
            }
            else {
                TapTime = MaxDubbleTapTime;
                TapCount += 1;
            }
        }
        if (TapTime > 0)
        {
            TapTime -= 1 * Time.deltaTime;
        }
        else {
            TapCount = 0;
        }
    }
}
