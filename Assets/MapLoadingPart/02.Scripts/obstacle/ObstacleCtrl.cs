using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    [System.Serializable]
    public abstract class ObstacleCtrl : MonoBehaviour,IChildModel
    {
        protected Transform playerTr;
        protected ObstacleST obsST;
        public Material[] materials;
        protected GameObject childModel;
        protected IObjToCharactor objToCharactor;

        public virtual void FromChildOnCollisionEnter(GameObject child, Collision coll)
        {
            print("옵스타클컬 콜리전");
        }

        public virtual void FromChildOnTriggerEnter(GameObject child, Collider other)
        {
            print("옵스타클컬 트리거");
        }

        protected virtual void Awake()
        {
            obsST = new ObstacleST();
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
            childModel = transform.Find("childModel").gameObject; //구형 장애물 모델링 자식 추출  (transform.find로 자식 중 내에서 검색)
            obsST.beenHit = false;
            objToCharactor = playerTr.gameObject.GetComponent<IObjToCharactor>();
        }
        protected virtual void OnEnable()
        {
            //print("옵스타클 오네이블");
            childModel.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }

        protected void OnDisable()
        {
            obsST.beenHit = false;

          //  this.gameObject.transform.SetParent(null);
            //스폰포인트와의 부모관계 삭제의도.
        }

        
    }
}