using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hcp;
public class Temp : MonoBehaviour ,IObjToCharactor{
    public void BeenHitByObs(ObstacleST obstacleST)
    {
        print("옵스타클 이벤트 받음.");
    }

    public void GetItem(ItemST itemST)
    {
        print("아이템 이벤트 받음.");
    }

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
