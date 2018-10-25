using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemTemp : MonoBehaviour {
    Transform childModel;
    private float speed;
	// Use this for initialization
	void Start () {
        speed = 50.0f;
        childModel = transform.Find("childModel").transform;
	}
	
	// Update is called once per frame
	void Update () {
        childModel.Rotate(new Vector3(0, 10f, 0) * speed * Time.deltaTime);
	}
}
