using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartGetEffect : MonoBehaviour {

    GameObject UnityChan;

    // Use this for initialization
    void Start()
    {
        this.UnityChan = GameObject.Find("MainPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        EffectCreat();
        Destroy(this, 3.0f);
    }

    void EffectCreat()
    {
            transform.position = new Vector3(this.UnityChan.transform.position.x, this.UnityChan.transform.position.y + 1.5f, this.UnityChan.transform.position.z - 0.5f);
    }
}
