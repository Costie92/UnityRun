using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveController : MonoBehaviour {

    public SimpleTouchController Controller;
    public Transform Trans;
    public float speedMovements = 100f;
    // Use this for initialization
    void Start () {
        Trans = GetComponent<RectTransform>();
        print(Trans.position.x);
        print(Trans.position.y);
    }
	
	// Update is called once per frame
	void Update () {
        Trans.position += (transform.up * Controller.GetTouchPosition.y * Time.deltaTime * speedMovements) +
            (transform.right * Controller.GetTouchPosition.x * Time.deltaTime * speedMovements);
          //Trans.MovePosition(transform.position + (transform.forward * Controller.GetTouchPosition.y * Time.deltaTime * speedMovements) +
          //  (transform.right * Controller.GetTouchPosition.x * Time.deltaTime * speedMovements));
    }
}
