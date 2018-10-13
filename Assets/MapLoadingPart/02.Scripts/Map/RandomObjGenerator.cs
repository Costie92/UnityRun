using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hcp
{
    public class RandomObjGenerator : SingletonTemplate<RandomObjGenerator>
    {

        //흠 코인 생성 경우 굳이 여러 청크 거치면서 해야되나 싶기도 하고?


        //ㅐㅇ성할때 풀링 되는지 여부 꼭 체크하기.

        int huddleProb,
        ballProb,
        upperHProb,
        fireProb;

        int itemHpPlusProb,
        itemInvinclbleProb,
        itemShieldProb,
        itemCoinProb,
        itemMagnetProb;

        int obsOrItem_obsWin_Prob;

        int[] itemProbs;
        int[] obsProbs;

        bool upperHFlag = false;

        float coinLineHighPointY;
        float jumpStartPointZ;
        float a;


        protected override void Awake()
        {
            base.Awake();
            coinLineHighPointY = 3;
            jumpStartPointZ = 2;    //점프 시작위치에 맞춰서 설정

            a = -coinLineHighPointY / (jumpStartPointZ * jumpStartPointZ);
            probReset();
            obsOrItem_obsWin_Prob = 50;

            itemProbs = new int[(int)E_ITEM.EITEMMAX];
            obsProbs = new int[(int)E_OBSTACLE.EOBSMAX];
        }
        
        float CoinLineGenerator(float highPointZ, float spawnZ)
        {
            //2차 함수그래프
            //y=a*(z-p)^2 + q
            float y = a * ((spawnZ - highPointZ)* (spawnZ - highPointZ)) + coinLineHighPointY;
            return (y < 0) ? y = 0 : y;
        }
        
        int probHundred  (int[] probs, int max)
        {
            float sum = 0;

            for(int i=0;i<probs.Length;i++)
            sum+=probs[i];

            float sumDiv = 100f / sum;

            float[] enumArr=new float[max];

            for(int i=0;i<max;i++)
            {
                enumArr[i]=probs[i]*sumDiv;//각 백분율이 들어감
                //enum 구조체의 선언 순서 필시 확인!!
            }

                //임시 디버깅용

                float temp=0;

                for(int i=0;i<max;i++)
                {
                temp +=enumArr[i];//각 백분율이 들어감
                //enum 구조체의 선언 순서 필시 확인!!
                }
                Debug.Log("백분율 총합 " +temp);
                //~임시디버깅용


            float randProb = Random.Range(0f, 100f);

            float formerSum = 0;

            for(int i=0;i<max;i++)
            {
                for(int j=0;j<i;j++)
                {
                    formerSum+= enumArr[j];
                }
            if(randProb<=formerSum+ enumArr[i])    return i;

            else formerSum = 0;
            }

            Debug.Log("확률 레인지 오류!");

            return -1;
}

        E_ITEM whichItem()
        {

            int item = probHundred(itemProbs, (int)E_ITEM.EITEMMAX);
            if (item >= 0 && item < (int)E_ITEM.EITEMMAX)
            {
                return (E_ITEM)item;
            }
            else
            {
                 print("앤럼오브젝트제너레이터오류 002" + item);
                return default(E_ITEM);
            }
        }

        E_OBSTACLE whichObs()
        {

            int obs = probHundred(obsProbs, (int)E_OBSTACLE.EOBSMAX);
            if (obs >= 0 && obs < (int)E_OBSTACLE.EOBSMAX)
            {
                return (E_OBSTACLE)obs;
            }
            else
            {
                print("앤럼오브젝트제너레이터오류 002-1" + obs);
                return default(E_OBSTACLE);
            }
        }

        void MakeProbArray()
        {
            itemProbs[(int)E_ITEM.HPPLUS] = itemHpPlusProb;
            itemProbs[(int)E_ITEM.INVINCIBLE] = itemInvinclbleProb;
            itemProbs[(int)E_ITEM.SHIELD] = itemShieldProb;
            itemProbs[(int)E_ITEM.COIN] = itemCoinProb;
            itemProbs[(int)E_ITEM.MAGNET] = itemMagnetProb;
            
            obsProbs[(int)E_OBSTACLE.BALL] = ballProb;
            obsProbs[(int)E_OBSTACLE.HUDDLE] = huddleProb;
            obsProbs[(int)E_OBSTACLE.UPPER_HUDDLE] = upperHProb;
            obsProbs[(int)E_OBSTACLE.FIRE] = fireProb;
        }

        void probAdjust()
        {
        }

        void probReset()
        {
            huddleProb = 25;
            ballProb = 25;
            upperHProb = 25;
            fireProb = 100 - (huddleProb + ballProb + upperHProb);
            
            itemHpPlusProb = 20;
            itemInvinclbleProb = 10;
            itemShieldProb = 20;
            itemCoinProb = 40;
            itemMagnetProb = 100 - (
            itemHpPlusProb +
            itemInvinclbleProb +
            itemShieldProb +
            itemCoinProb);
        }

        //마그넷이 뜨면 코인의 확률을 더 올려준다든지,
        //!!!볼은 절대 연속으로 세번 나오거나 하면 안돼!!!!!
        public List<GameObject> RandomObjGen(Transform spawnPoint, int spawnPointNum)
        {
            List<GameObject> whatSpawned=new List<GameObject>();
            if (upperHFlag)
            {
                if (spawnPointNum == 2)
                {
                    upperHFlag = false;
                    return null;
                }
            return null;
            }
            probAdjust();
            MakeProbArray();

            if (Gamble(obsOrItem_obsWin_Prob, "ObsorItem"))
            {
                E_OBSTACLE eWhichObs = whichObs();
                switch (eWhichObs)
                {
                    case E_OBSTACLE.BALL:
                        //볼 생성 풀링;
                        GenByPool(MapAndObjPool.GetInstance().GetObsBallInPool(), spawnPoint, whatSpawned);
                        break;
                    case E_OBSTACLE.HUDDLE:
                        //허들생성풀링
                        GenByPool(MapAndObjPool.GetInstance().GetObsHuddleInPool(), spawnPoint, whatSpawned);
                        break;
                    case E_OBSTACLE.FIRE:
                        //불 생성풀링
                        GenByPool(MapAndObjPool.GetInstance().GetObsFireInPool(), spawnPoint, whatSpawned);
                        break;
                    case E_OBSTACLE.UPPER_HUDDLE:
                        //어퍼허들 길이 체크, 스폰 포인트 위치랑 등등 해서 잘
                        if (spawnPointNum == 0)
                        {
                            if (null    !=  GenByPool(MapAndObjPool.GetInstance().GetObsUpperHuddle_3_InPool(), spawnPoint, whatSpawned))
                                upperHFlag = true;
                        }
                        else if (spawnPointNum == 1)
                        {
                            if (null != GenByPool(MapAndObjPool.GetInstance().GetObsUpperHuddle_2_InPool(), spawnPoint, whatSpawned))
                                upperHFlag = true;
                        }
                        else if (spawnPointNum == 2)
                        {
                                GenByPool(MapAndObjPool.GetInstance().GetObsUpperHuddle_1_InPool(), spawnPoint, whatSpawned);
                         }
                        break;
                    default:
                        Debug.Log("랜던오브젝트제너레이터오류003" + eWhichObs);
                        break;
                }
            }
            else//아이템 생성의 경우
            {
                E_ITEM eWhichItem   =   whichItem();
                //eWhichItem따라 아이템 생성
                switch (eWhichItem)
                {
                    case E_ITEM.HPPLUS:
                        //회복 아이템 풀링
                        GenByPool(MapAndObjPool.GetInstance().GetItemHPPlusInPool(), spawnPoint, whatSpawned);
                        break;
                    case E_ITEM.INVINCIBLE:
                        //무적아이템풀링
                        GenByPool(MapAndObjPool.GetInstance().GetItemInvincibleInPool(), spawnPoint, whatSpawned);
                        break;
                    case E_ITEM.SHIELD:
                        //쉴드 아이템 풀링
                        GenByPool(MapAndObjPool.GetInstance().GetItemShieldInPool(), spawnPoint, whatSpawned);
                        break;
                    case E_ITEM.MAGNET:
                        //자석아이템 풀링
                        GenByPool(MapAndObjPool.GetInstance().GetItemMagnetInPool(), spawnPoint, whatSpawned);
                        break;
                    case E_ITEM.COIN:
                        if (Gamble(50, "parabola CoinLine?"))
                        {
                            for (float i = 0; i < MapObjManager.GetInstance().GetChunkMargin(); i = i + 1)
                            {
                                GameObject temp= MapAndObjPool.GetInstance().GetItemCoinInPool();
                                float y=0;
                                if (temp != null)
                                {
                                    y = CoinLineGenerator(spawnPoint.position.z, spawnPoint.position.z + (-5 + i));
                                    temp.transform.position = new Vector3(spawnPoint.position.x, y, spawnPoint.position.z + (-5 + i));
                                    temp.transform.rotation = spawnPoint.rotation;
                                    temp.SetActive(true);
                                    whatSpawned.Add(temp);
                                }
                                else break;
                                //생성(spawnPoint.p-osition.x,y,spawnPoint.p-osition.z+(-5+i))
                            }
                            if (Gamble(50, "fireorHuddle on coin Line?"))
                            {
                                if (Gamble(50, "huddle?"))
                                {
                                    //허들놓기
                                    GenByPool(MapAndObjPool.GetInstance().GetObsHuddleInPool(), spawnPoint, whatSpawned);
                                }
                                else
                                {
                                    //불 놓기
                                    GenByPool(MapAndObjPool.GetInstance().GetObsFireInPool(), spawnPoint, whatSpawned);
                                }
                            }
                        }
                        else
                        {
                            //직선 코인라인생성
                            for (int i = 0; i < MapObjManager.GetInstance().GetChunkMargin(); i++)
                            {
                                GameObject temp = MapAndObjPool.GetInstance().GetItemCoinInPool();
                                if (temp != null)
                                {
                                    temp.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z + (-5 + i));
                                    temp.transform.rotation = spawnPoint.rotation;
                                    temp.SetActive(true);
                                    whatSpawned.Add(temp);
                                }
                                else break;
                            }
                        }
                        break;
                    default: Debug.Log("아이템스폰 뭔가 오류"); break;
                }
            }
            return whatSpawned;
        }
        
        bool Gamble(int probability, string gambleTitle_win_lose)
        {
            if (Random.Range(0f, 100f) <= probability)
                return true;
            else return false;
        }

        GameObject GenByPool(GameObject objByPool, Transform spawnPoint,  List<GameObject> spawned)
        {
            if (objByPool == null)
            {
                Debug.Log("풀링 해오지 못함");
                return null;
            }
            objByPool.transform.position = spawnPoint.position;
            objByPool.transform.rotation = spawnPoint.rotation;
            objByPool.SetActive(true);
            spawned.Add(objByPool); //청크로딩 쪽에서 리턴 받을 오브젝트들 리스트 만드는 거임
            return objByPool;
        }
    }
}