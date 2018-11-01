using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp {
    public class BossPatternGenerator : SingletonTemplate<BossPatternGenerator>,IBossPattern {

        public List<GameObject> bossAttackObj = new List<GameObject>();
        Vector3 pos = new Vector3();
        public float moveSpeed;
        Transform playerTr;

        private void Start()
        {
            moveSpeed = GetComponent<BossMapMgr>().moveSpeed;
            playerTr = GameObject.FindGameObjectWithTag("PLAYER").transform;
        }

        private void Update()
        {
            DelBossObs();
            MoveBossAttackObj();
        }

        void MoveBossAttackObj()
        {//땅 2배  메테오 2배 브레스 4배
            for (int i = 0; i < bossAttackObj.Count; i++)
            {
                if (bossAttackObj[i].activeSelf == true)
                {
                    if(bossAttackObj[i].GetComponent<IOnlyBossPatternMove>()!=null)
                    bossAttackObj[i].GetComponent<IOnlyBossPatternMove>().MoveAtOwnSpeed();
                }
            }
        }

        void DelBossObs()
        {
            for (int i = 0; i < bossAttackObj.Count; i++)
            {
                if (bossAttackObj[i].activeSelf == true && bossAttackObj[i].transform.position.z + 10f <= playerTr.position.z)
                {
                    bossAttackObj[i].SetActive(false);
                    bossAttackObj.RemoveAt(i);
                }
            }
        }

        public void BossPatternObjGen(E_BOSSPATTERN pattern, float disFromPlayer, E_SPAWNLINE line)
        {
            if ((false == (pattern == E_BOSSPATTERN.BREATH || pattern == E_BOSSPATTERN.METEOR)))
                return;

            pos.x = GetLineXPos(line);
            pos.y = 0f;
            pos.z = disFromPlayer;
            GameObject obj;

            switch (pattern)
            { 
               
                case E_BOSSPATTERN.BREATH:
                   
                    //브레스 프리팹 풀링해오기.
                    obj = MapAndObjPool.GetInstance().GetBossObsBreathInPool();
                    if (obj == null) return;
                    obj.transform.SetPositionAndRotation(pos, Quaternion.Euler(0,180,0) );
                    obj.SetActive(true);
                    bossAttackObj.Add(obj);
                    break;

                case E_BOSSPATTERN.METEOR:

                    //메테오 프리팹 풀링해오기
                    obj = MapAndObjPool.GetInstance().GetBossObsMeteorInPool();
                    if (obj == null) return;
                    obj.transform.SetPositionAndRotation(pos, Quaternion.Euler(0, 180, 0));
                    obj.SetActive(true);
                    bossAttackObj.Add(obj);

                    break;
            }
        }

        float GetLineXPos(E_SPAWNLINE  line)
        {
            switch (line)
            {
                case E_SPAWNLINE.LEFT:
                    return 0.7f;
                case E_SPAWNLINE.CENTER:
                    return 3.8f;
                case E_SPAWNLINE.RIGHT:
                    return 6.9f;
                default: return -1;
            }
        }

        public void BossPatternFireBallShoot(Vector3 pos)
        {
            GameObject temp =
                  MapAndObjPool.GetInstance().GetBossObsFireBallInPool();
            if (temp == null)
                return;
            temp.transform.position = pos;
            temp.SetActive(true);
            bossAttackObj.Add(temp);
        }
    }
}