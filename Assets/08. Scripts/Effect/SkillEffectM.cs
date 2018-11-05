using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectM : MonoBehaviour {

    GameObject UnityChan;

    // Use this for initialization
    void Start () {
        this.UnityChan = GameObject.FindGameObjectWithTag("PLAYER");
    }
	
	// Update is called once per frame
	void Update () {
        EffectCreat();
    }

    void EffectCreat()
    {
        if(ObjEat.Magnet == true)
        {
            transform.position = new Vector3(this.UnityChan.transform.position.x, this.UnityChan.transform.position.y, this.UnityChan.transform.position.z+0.5f);
        }
        else if(ObjEat.Magnet == false)
        {
            transform.position = new Vector3(0,0,-50);
        }
    }
}
