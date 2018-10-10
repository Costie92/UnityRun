using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCtrl : MonoBehaviour, IChildModel
{
    public Transform playerTr;
    private GameObject childModel; //구형 장애물 모델링 자식 추출
    public Material[] materials;
    

    public float moveSpeed = 0.1f;
    Vector3 n = new Vector3(15,0,0);

	// Use this for initialization
	void Awake () {
        childModel = transform. Find("childModel").gameObject ; //구형 장애물 모델링 자식 추출  (transform.find로 자식 중 내에서 검색)
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
        //childModel.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
	}
    private void OnEnable()
    {
        transform.LookAt(playerTr, Vector3.up);
        childModel.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
    }
    // Update is called once per frame
    void Update () {
       childModel.transform.Rotate(n, Space.Self);
        transform.Translate(Vector3.forward * moveSpeed, Space.Self);
        if (transform.position.z < playerTr.position.z - 10)
            MapAndObjPool.instance.TurnInPoolObj(this.gameObject);
	}


    public void FromChildOnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PLAYER"))
        {
            print(gameObject.name + "트리거 콜리전!!!!!!!!!!!!");
}
    }

    public void FromChildOnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PLAYER"))
        {
            print(gameObject.name + "콜리전!!!!!!!!!!!!");
        }
    }
}
