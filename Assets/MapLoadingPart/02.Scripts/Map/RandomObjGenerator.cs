using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hcp
{
    public class RandomObjGenerator : SingletonTemplate<RandomObjGenerator>
    {
        /* 코인 포물선 공식에 쓰였던
        float coinLineHighPointY;
        float jumpStartPointZ;
        float a;
        coinLineHighPointY = 3;
            jumpStartPointZ = 3;    //점프 시작위치에 맞춰서 설정

            a = -coinLineHighPointY / (jumpStartPointZ * jumpStartPointZ);
        */
        /*
        float CoinLineGenerator(float highPointZ, float spawnZ)
        {
            //2차 함수그래프
            //y=a*(z-p)^2 + q
            float y = a * ((spawnZ - highPointZ)* (spawnZ - highPointZ)) + coinLineHighPointY;
            return (y < 0) ? y = 0 : y;
        }
        */
        [SerializeField]
        int
        HPPLUS,
        INVINCIBLE,
        SHIELD,
        COIN,
        COIN_STRAIGHT,
        COIN_PARABOLA,
        MAGNET,

        BALL,
        HUDDLE,
        UPPER_HUDDLE_1,
            UPPER_HUDDLE_2,
            UPPER_HUDDLE_3,
        FIRE,

        NOTHING;

        protected override void Awake()
        {
            base.Awake();
            MakeProbArray();
        }
        
        
        E_SPAWN_OBJ_TYPE probArrGamble (params int [] probs)
        {
            float sum = 0;

            for(int i=0;i<probs.Length;i++)
            sum+=probs[i];

            if (sum.Equals(0)) return E_SPAWN_OBJ_TYPE.NOTHING;

            float randProb = Random.Range(0f, sum);
           // Debug.Log("백분율 확률은" +randProb);

            float formerSum = 0;

            for(int i=0;i< probs.Length; i++)
            {
                if (!i.Equals(0)) formerSum += probs[i - 1];

                if (randProb <= formerSum + probs[i])
                {
                  //  Debug.Log((E_SPAWN_OBJ_TYPE)i + "선택");
                    return (E_SPAWN_OBJ_TYPE)i;
                }
                
            }
            ErrorManager.SpurtError("확률 계산중 오류");
            return E_SPAWN_OBJ_TYPE.NOTHING;
        }
        
        void MakeProbArray()
        {
            HPPLUS = 1;
            INVINCIBLE = 1;
            SHIELD = 1;
            COIN = 1;
            COIN_STRAIGHT = 1;
            COIN_PARABOLA = 1;
            MAGNET = 1;

            BALL = 1;
            HUDDLE = 1;
            UPPER_HUDDLE_1 = 1;
            UPPER_HUDDLE_2 = 1;
            UPPER_HUDDLE_3 = 1;
            FIRE = 1;

            NOTHING = 1;
        }
        
        E_SPAWN_OBJ_TYPE GambleObjType(int spawnPointNum,bool  uppered,bool ballSpawned)
        {
            E_SPAWN_OBJ_TYPE temp;
            int tempUH1 = UPPER_HUDDLE_1;
            int tempUH2 = UPPER_HUDDLE_2;
            int tempUH3 = UPPER_HUDDLE_3;
            int tempB = BALL;

            if (uppered)
            {
                UPPER_HUDDLE_1 = 0;
                UPPER_HUDDLE_2 = 0;
                UPPER_HUDDLE_3 = 0;
            }
            if (ballSpawned)
            {
                BALL = 0;
            }
            if (spawnPointNum == 1)
            {
                UPPER_HUDDLE_3 = 0;
            }
            if (spawnPointNum == 2)
            {
                UPPER_HUDDLE_2 = 0;
                UPPER_HUDDLE_3 = 0;
            }

            temp=
            probArrGamble(
                HPPLUS,
                INVINCIBLE,
                SHIELD,
                COIN,
                COIN_STRAIGHT,
                COIN_PARABOLA,
                MAGNET,

                BALL,
                HUDDLE,
                UPPER_HUDDLE_1,
                UPPER_HUDDLE_2,
                UPPER_HUDDLE_3,
                FIRE,
                NOTHING);

            BALL = tempB;
            UPPER_HUDDLE_1 = tempUH1;
            UPPER_HUDDLE_2 = tempUH2;
            UPPER_HUDDLE_3 = tempUH3;

            return temp;
        }

       

        //마그넷이 뜨면 코인의 확률을 더 올려준다든지,
        //!!!볼은 절대 연속으로 세번 나오거나 하면 안돼!!!!!
        public void RandomObjGen(Transform spawnPointGroup, ref List<GameObject> objs)
        {
            int upperFlag = 0;
            bool uppered = false;
            bool ballSpawned = false;

            E_SPAWN_OBJ_TYPE typeOfObj;
            GameObject spawned;
            Transform spawnPoint;
            for (int i = 0; i < spawnPointGroup.childCount; i++)
            {
                if (upperFlag-- > 0)
                    continue;

                spawnPoint = spawnPointGroup.GetChild(i);
                typeOfObj = GambleObjType(i,uppered: uppered, ballSpawned: ballSpawned);
                if (ballSpawned)
                {
                    ballSpawned = false;
                }
                if (typeOfObj.Equals(E_SPAWN_OBJ_TYPE.BALL))
                {
                    ballSpawned = true;
                }
                if(typeOfObj.Equals(E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_1))
                {
                    uppered = true;
                }
                if (typeOfObj.Equals(E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_2))
                {
                    upperFlag = 1;
                    uppered = true;
                }
                if (typeOfObj.Equals(E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_3))
                {
                    upperFlag = 2;
                    uppered = true;
                }

                spawned = ObjFactory.ObjSpawnFactory(typeOfObj, spawnPoint);
                if(spawned!=null)
                    objs.Add(spawned);
            }
        }
    }
}