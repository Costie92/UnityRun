using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemTemp : MonoBehaviour {
    Transform childModel;
	// Use this for initialization
	void Start () {
        childModel = transform.Find("childModel").transform;
	}
	
	// Update is called once per frame
	void Update () {
        childModel.Rotate(new Vector3(0, 10f, 0));
	}
}
