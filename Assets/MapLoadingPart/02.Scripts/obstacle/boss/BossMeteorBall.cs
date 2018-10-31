using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public class BossMeteorBall : MonoBehaviour
    {
        ObstacleST obsST;
        IObjToCharactor objToCharactor;

        private void Awake()
        {
            obsST.obstacleType = E_OBSTACLE.BOSS_METEOR;
            obsST.beenHit = true;
            objToCharactor = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<IObjToCharactor>();
        }
        private void OnTriggerEnter(Collider other)
        {
            objToCharactor.BeenHitByObs(obsST);
        }
    }
}