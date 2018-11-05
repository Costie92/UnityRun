using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectS : MonoBehaviour {

    GameObject UnityChan;

    // Use this for initialization
    void Start () {
        this.UnityChan = GameObject.Find("MainPlayer");
    }
	
	// Update is called once per frame
	void Update () {
        EffectCreat();
    }

    void EffectCreat()
    {
        if(ObjEat.Shield == true)
        {
            transform.position = new Vector3(this.UnityChan.transform.position.x, this.UnityChan.transform.position.y, this.UnityChan.transform.position.z+1.0f);
        }
        else if(ObjEat.Shield == false)
        {
            transform.position = new Vector3(0,0,0);
        }
    }
}
