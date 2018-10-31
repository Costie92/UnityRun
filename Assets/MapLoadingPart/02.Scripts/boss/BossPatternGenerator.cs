using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp {
    public class BossPatternGenerator : SingletonTemplate<BossPatternGenerator>,IBossPattern {

        List<GameObject> bossAttackObj = new List<GameObject>();
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
        {
            for (int i = 0; i < bossAttackObj.Count; i++)
            {
                if(bossAttackObj[i].activeSelf==true)
                bossAttackObj[i].transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
            }
        }

        void DelBossObs()
        {
            for (int i = 0; i < bossAttackObj.Count; i++)
            {
                if (bossAttackObj[i].activeSelf == true && bossAttackObj[i].transform.position.z + 10f <= playerTr.position.z)
                    bossAttackObj[i].SetActive(false);
            }
        }

        public void BossPatternObjGen(E_BOSSPATTERN pattern, float disFromPlayer, int line)
        {
            if ((false == (pattern == E_BOSSPATTERN.BREATH || pattern == E_BOSSPATTERN.METEOR))
                || (line < 0 || line > 2))
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
                    obj.transform.SetPositionAndRotation(pos, Quaternion.identity);
                    obj.SetActive(true);
                    break;

                case E_BOSSPATTERN.METEOR:

                    //메테오 프리팹 풀링해오기
                    obj = MapAndObjPool.GetInstance().GetBossObsMeteorInPool();
                    if (obj == null) return;
                    obj.transform.SetPositionAndRotation(pos, Quaternion.identity);
                    obj.SetActive(true);

                    break;
            }
        }

        float GetLineXPos(int  line)
        {
            switch (line)
            {
                case 0:
                    return 0.7f;
                case 1:
                    return 3.8f;
                case 2:
                    return 6.9f;
                default: return -1;
            }
        }


    }
}