using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hcp
{
    public class MapAndObjPool : MonoBehaviour
    {
        public GameObject chunk;
        public GameObject obstacleBall;
        public GameObject obstacleHuddle;

        public GameObject item_HPPLUS;
        public GameObject item_INVINCIBLE;
        public GameObject item_SHIELD;
        public GameObject item_COIN;
        public GameObject item_MAGNET;

        public List<GameObject> chunkPool = new List<GameObject>();
        public List<GameObject> obsBallPool = new List<GameObject>();
        public List<GameObject> obsHuddlePool = new List<GameObject>();
        /*
        public List<GameObject> obsHuddlePool = new List<GameObject>();
        public List<GameObject> obsHuddlePool = new List<GameObject>();
        public List<GameObject> obsHuddlePool = new List<GameObject>();
        public List<GameObject> obsHuddlePool = new List<GameObject>();
        public List<GameObject> obsHuddlePool = new List<GameObject>();
        */
        public static MapAndObjPool instance = null;

        private int chunkPoolCapacity = 10;
        private int obsBallPoolCapacity = 5;
        private int obsHuddlePoolCapacity = 5;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(this.gameObject);

            DontDestroyOnLoad(this.gameObject);

            Debug.Log("풀링 셋업어웨이크 종료.");
        }

        void PoolInit(GameObject prefab, List<GameObject> list, int capacity, string parentName, string poolobjname)
        {
            GameObject PoolParent = new GameObject(parentName);


            if (!prefab)
            {
                return;
            }
            for (int i = 0; i < capacity; i++)
            {
                var temp = Instantiate<GameObject>(prefab, PoolParent.transform);
                temp.name = poolobjname + i.ToString("00");
                temp.SetActive(false);  //활성화 여부로 풀링 제공 
                list.Add(temp);
                Debug.Log(list[i].name + "풀에서 확인");
            }
        }
        GameObject GetInSthPool(List<GameObject> list)
        {
          //  Debug.Log("겟풀 하러왔음. 현재 풀에는" + list.Count + "갯수가 있음");
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].activeSelf == false)
                {
            //        Debug.Log(list[i].name + "받아감.");
                    return list[i];
                }
            }
            return null;
        }

        public void ChunkPoolInit(int capacity1 = 10)
        {
            chunkPoolCapacity = capacity1;
            PoolInit(chunk, chunkPool, chunkPoolCapacity, "chunkPoolParent", "CHUNK_");
        }


        public void obsBallPoolInit(int capacity1 = 5)
        {
            obsBallPoolCapacity = capacity1;
            PoolInit(obstacleBall, obsBallPool, obsBallPoolCapacity, "obsBallPoolParent", "OBSBALL_");
        }

        public void obsHuddlePoolInit(int capacity1 = 5)
        {
            obsHuddlePoolCapacity = capacity1;
            PoolInit(obstacleHuddle, obsHuddlePool, obsHuddlePoolCapacity, "obsHuddlePoolParent", "OBSHUDDLE_");
        }



        public GameObject GetChunkInPool()
        {
            return GetInSthPool(chunkPool);
        }

        public GameObject GetObsBallInPool()
        {
            return GetInSthPool(obsBallPool);
        }

        public GameObject GetObsHuddleInPool()
        {
            return GetInSthPool(obsHuddlePool);
        }
        public void TurnInPoolObj(GameObject temp)
        {
            temp.transform.position = Vector3.zero;
            temp.transform.rotation = Quaternion.identity;
            //오브젝트 풀링 경우
            temp.SetActive(false);//비활성화로 풀링에 반납
        }

    }
};