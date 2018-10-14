using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("w"))
        {
            this.transform.Translate(Vector3.forward*10f*Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            this.transform.Translate(Vector3.left * 10f * Time.deltaTime);
        }
        if (Input.GetKey("d"))
        {
            this.transform.Translate(Vector3.right * 10f * Time.deltaTime);
        }
    }
}
