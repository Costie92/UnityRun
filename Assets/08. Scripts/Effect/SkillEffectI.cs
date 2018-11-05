using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectI : MonoBehaviour {

    GameObject UnityChan;

    // Use this for initialization
    void Start () {
        this.UnityChan = GameObject.FindGameObjectWithTag("PLAYER");
    }
	
	// Update is called once per frame
	void Update () {

    }

    void LateUpdate()
    {
        EffectCreat();
    }
        void EffectCreat()
    {
        if(ObjEat.Invincible == true)
        {
            transform.position = new Vector3(this.UnityChan.transform.position.x, this.UnityChan.transform.position.y, this.UnityChan.transform.position.z+0.5f);
        }
        else if(ObjEat.Invincible == false)
        {
            transform.position = new Vector3(0,0,-50);
        }
    }
}
