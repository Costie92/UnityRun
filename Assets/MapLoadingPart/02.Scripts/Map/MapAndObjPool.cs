using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace hcp
{
    public class MapAndObjPool : SingletonTemplate<MapAndObjPool>
    {
        public GameObject chunk;

        public GameObject obstacleBall;
        public GameObject obstacleHuddle;
        public GameObject obstacleUpperHuddle_1;
        public GameObject obstacleUpperHuddle_2;
        public GameObject obstacleUpperHuddle_3;
        public GameObject obstacleFire;

        public GameObject item_HPPLUS;
        public GameObject item_INVINCIBLE;
        public GameObject item_SHIELD;
        public GameObject item_COIN;
        public GameObject item_COIN_Parabola;
        public GameObject item_COIN_Straight;
        public GameObject item_MAGNET;

        public List<GameObject> chunkPool = new List<GameObject>();

        public List<GameObject> obsBallPool = new List<GameObject>();
        public List<GameObject> obsHuddlePool = new List<GameObject>();
        public List<GameObject> obsUpperHuddle_1_Pool = new List<GameObject>();
        public List<GameObject> obsUpperHuddle_2_Pool = new List<GameObject>();
        public List<GameObject> obsUpperHuddle_3_Pool = new List<GameObject>();
        public List<GameObject> obsFirePool = new List<GameObject>();

        public List<GameObject> itemHPPlusPool = new List<GameObject>();
        public List<GameObject> itemInvinciblePool = new List<GameObject>();
        public List<GameObject> itemShieldPool = new List<GameObject>();
        public List<GameObject> itemCoinPool = new List<GameObject>();
        public List<GameObject> itemCoin_StraightLine_Pool = new List<GameObject>();
        public List<GameObject> itemCoin_Parabola_Pool = new List<GameObject>();
        public List<GameObject> itemMagnetPool = new List<GameObject>();


        private int chunkPoolCapacity = 10;

        private int obsBallPoolCapacity = 5;
        private int obsHuddlePoolCapacity = 5;
        private int obsUpperHuddle_1_PoolCapacity = 5;
        private int obsUpperHuddle_2_PoolCapacity = 5;
        private int obsUpperHuddle_3_PoolCapacity = 5;
        private int obsFirePoolCapacity = 5;

        private int itemHPPlusPoolCapacity = 5;
        private int itemInvinciblePoolCapacity = 5;
        private int itemShieldPoolCapacity = 5;
        private int itemCoinPoolCapacity = 5;
        private int itemCoin_StraightLine_Capacity = 5;
        private int itemCoin_Parabola_Capacity = 5;
        private int itemMagnetPoolCapacity = 5;


        protected override void Awake()
        {
            /*
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(this.gameObject);

            DontDestroyOnLoad(this.gameObject);

            Debug.Log("풀링 셋업어웨이크 종료.");
            */
            base.Awake();
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
                if (ChunkLoading.debugLog) Debug.Log(list[i].name + "풀에서 확인");
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

        public void obsUpperHuddle_1_PoolInit(int capacity1 = 5)
        {
            obsUpperHuddle_1_PoolCapacity = capacity1;
            PoolInit(obstacleUpperHuddle_1, obsUpperHuddle_1_Pool, obsUpperHuddle_1_PoolCapacity, "obsupperHuddle_1_PoolParent", "OBSUPHUDDLE_1_");
        }

        public void obsUpperHuddle_2_PoolInit(int capacity1 = 5)
        {
            obsUpperHuddle_2_PoolCapacity = capacity1;
            PoolInit(obstacleUpperHuddle_2, obsUpperHuddle_2_Pool, obsUpperHuddle_2_PoolCapacity, "obsupperHuddle_2_PoolParent", "OBSUPHUDDLE_2_");
        }

        public void obsUpperHuddle_3_PoolInit(int capacity1 = 5)
        {
            obsUpperHuddle_3_PoolCapacity = capacity1;
            PoolInit(obstacleUpperHuddle_3, obsUpperHuddle_3_Pool, obsUpperHuddle_3_PoolCapacity, "obsupperHuddle_3_PoolParent", "OBSUPHUDDLE_3_");
        }

        public void obsFirePoolInit(int capacity1 = 5)
        {
            obsFirePoolCapacity = capacity1;
            PoolInit(obstacleFire, obsFirePool, obsFirePoolCapacity, "obsFirePoolParent", "OBSFIRE_");
        }

        public void itemHPPlusPoolInit(int capacity1 = 5)
        {
            itemHPPlusPoolCapacity = capacity1;
            PoolInit(item_HPPLUS, itemHPPlusPool, itemHPPlusPoolCapacity, "itemHPPlusPoolParent", "ITEMHPPLUS_");
        }

        public void itemInvinciblePoolInit(int capacity1 = 5)
        {
            itemInvinciblePoolCapacity = capacity1;
            PoolInit(item_INVINCIBLE, itemInvinciblePool, itemInvinciblePoolCapacity, "itemInvinciblePoolParent", "ITEMINVINCIBLE_");
        }

        public void itemShieldPoolInit(int capacity1 = 5)
        {
            itemShieldPoolCapacity = capacity1;
            PoolInit(item_SHIELD, itemShieldPool, itemShieldPoolCapacity, "itemShieldPoolParent", "ITEMSHIELD_");
        }

        public void itemCoinPoolInit(int capacity1 = 5)
        {
            itemCoinPoolCapacity = capacity1;
            PoolInit(item_COIN, itemCoinPool, itemCoinPoolCapacity, "itemCoinPoolParent", "ITEMCOIN_");
        }

        public void itemCoin_StraightLine_PoolInit(int capacity1 = 5)
        {
            itemCoin_StraightLine_Capacity = capacity1;
            PoolInit(item_COIN_Straight, itemCoin_StraightLine_Pool, itemCoin_StraightLine_Capacity, "itemCoin_StraightLine_PoolParent", "ITEMCOINSTRAIGHT_");
        }

        public void itemCoin_Parabola_PoolInit(int capacity1 = 5)
        {
            itemCoin_Parabola_Capacity = capacity1;
            PoolInit(item_COIN_Parabola, itemCoin_Parabola_Pool, itemCoin_Parabola_Capacity, "itemCoin_Parabola_PoolParent", "ITEMCOINPARABOLA_");
        }

        public void itemMagnetPoolInit(int capacity1 = 5)
        {
            itemMagnetPoolCapacity = capacity1;
            PoolInit(item_MAGNET, itemMagnetPool, itemMagnetPoolCapacity, "itemMagnetPoolParent", "ITEMMAGNET_");
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

        public GameObject GetObsUpperHuddle_1_InPool()
        {
            return GetInSthPool(obsUpperHuddle_1_Pool);
        }

        public GameObject GetObsUpperHuddle_2_InPool()
        {
            return GetInSthPool(obsUpperHuddle_2_Pool);
        }

        public GameObject GetObsUpperHuddle_3_InPool()
        {
            return GetInSthPool(obsUpperHuddle_3_Pool);
        }

        public GameObject GetObsFireInPool()
        {
            return GetInSthPool(obsFirePool);
        }

        public GameObject GetItemHPPlusInPool()
        {
            return GetInSthPool(itemHPPlusPool);
        }

        public GameObject GetItemInvincibleInPool()
        {
            return GetInSthPool(itemInvinciblePool);
        }

        public GameObject GetItemShieldInPool()
        {
            return GetInSthPool(itemShieldPool);
        }

        public GameObject GetItemCoinInPool()
        {
            return GetInSthPool(itemCoinPool);
        }

        public GameObject GetItemCoin_StraightLine_InPool()
        {
            return GetInSthPool(itemCoin_StraightLine_Pool);
        }

        public GameObject GetItemCoin_Parabola_InPool()
        {
            return GetInSthPool(itemCoin_Parabola_Pool);
        }


        public GameObject GetItemMagnetInPool()
        {
            return GetInSthPool(itemMagnetPool);
        }

        public void TurnInPoolObj(GameObject temp)
        {
            //temp.transform.position = Vector3.zero;
            //temp.transform.rotation = Quaternion.identity;
            //오브젝트 풀링 경우
            temp.SetActive(false);//비활성화로 풀링에 반납
        }

    }
};