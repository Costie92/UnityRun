using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp {
    public class ItemCoinStraightAdjust : MonoBehaviour {
        [SerializeField]
        public GameObject[] coinObjs;
        private void Awake()
        {
            coinObjs = new GameObject[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                coinObjs[i] = transform.GetChild(i).gameObject;
            }
        }
        private void OnDisable()
        {
            float sp = 4.5f;
            for (int i = 0; i < coinObjs.Length; i++)
            {
                coinObjs[i].transform.position
                    = transform.position - (Vector3.forward * sp);
                sp -= 1;
            }
        }
    }
}