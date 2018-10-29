using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{

    GameObject UnityChan;

    // Use this for initialization
    void Start()
    {
        this.UnityChan = GameObject.FindGameObjectWithTag("PLAYER");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(this.UnityChan.transform.position.x, 7.5f, this.UnityChan.transform.position.z - 5f);
    }
}
