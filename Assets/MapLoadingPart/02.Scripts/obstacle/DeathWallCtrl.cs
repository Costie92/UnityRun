using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class DeathWallCtrl : MonoBehaviour
    {
        IObjToCharactor objToCharactor;
        ObstacleST obsST;
        private void Awake()
        {
            obsST = new ObstacleST();
            obsST.obstacleType = E_OBSTACLE.FIRE;   //그냥 파이어로 처리 (즉사는 같은 거임./)
            obsST.beenHit = true;
            objToCharactor = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<IObjToCharactor>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PLAYER"))
            {
                Debug.Log("FIRE 장애물 트리거엔터 이벤트");
                obsST.beenHit = true;
                objToCharactor.BeenHitByObs(obsST);
            }
        }
    }
}