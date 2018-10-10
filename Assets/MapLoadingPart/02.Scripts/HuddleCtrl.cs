using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuddleCtrl : MonoBehaviour ,IChildModel{
    public Material[] materials;
    private GameObject childModel; //구형 장애물 모델링 자식 추출

    public void FromChildOnCollisionEnter(Collision coll)
    {
        
    }

    public void FromChildOnTriggerEnter(Collider other)
    {
       
    }

    // Use this for initialization
    void Awake () {
        childModel = transform.Find("childModel").gameObject; //구형 장애물 모델링 자식 추출  (transform.find로 자식 중 내에서 검색)
    }

    private void OnEnable()
    {
        childModel.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
    }

    // Update is called once per frame
    void Update () {
		
	}
}
