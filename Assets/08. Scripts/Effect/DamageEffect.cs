using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour {

    GameObject UnityChan;

    // Use this for initialization
    void Start () {
        this.UnityChan = GameObject.Find("unitychan");
    }
	
	// Update is called once per frame
	void Update () {
        EffectCreat();
    }

    void EffectCreat()
    {
        if(ObjEat.HitInvincible == true)
        {
            transform.position = new Vector3(this.UnityChan.transform.position.x, this.UnityChan.transform.position.y+1.5f, this.UnityChan.transform.position.z);
        }
        else if(ObjEat.HitInvincible == false)
        {
            transform.position = new Vector3(0,0,0);
        }
    }
}
