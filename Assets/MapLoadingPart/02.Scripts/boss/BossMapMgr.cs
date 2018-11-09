using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public enum E_BOSSPATTERN
    {
        ONCE_FIREBALL=0,
        BREATH,
        METEOR
    }
    public enum E_SPAWNLINE
    {
        LEFT=0,
        CENTER,
        RIGHT
    }

    public class BossMapMgr : SingletonTemplate<BossMapMgr>
    {
        public GameObject chunk;
        public float moveSpeed = 20.0f;
        List<BossMapST> mapSTList = new List<BossMapST>();
        float chunkMargin;

        bool stop = false;

        protected override void Awake()
        {
            base.Awake();

            chunkMargin = chunk.GetComponentInChildren<Renderer>().bounds.size.z;

        }
        private void Start()
        {
            MapAndObjPool.GetInstance().ChunkPoolInit(Constants.wantToShowNumOfChunks);
            MapAndObjPool.GetInstance().bossObsBreathPoolInit();
            MapAndObjPool.GetInstance().bossObsFireBallPoolInit();
            MapAndObjPool.GetInstance().bossObsMeteorPoolInit();
            InitMapST();
        }

        private void Update()
        {
            if (Input.GetKeyDown("p"))
            {
                BossPatternGenerator.GetInstance().BossPatternObjGen(E_BOSSPATTERN.METEOR, 100, E_SPAWNLINE.CENTER);

                BossPatternGenerator.GetInstance().BossPatternObjGen(E_BOSSPATTERN.METEOR, 100, E_SPAWNLINE.LEFT);

                BossPatternGenerator.GetInstance().BossPatternObjGen(E_BOSSPATTERN.METEOR, 100, E_SPAWNLINE.RIGHT);


                BossPatternGenerator.GetInstance().BossPatternObjGen(E_BOSSPATTERN.BREATH, 50, E_SPAWNLINE.CENTER);

                BossPatternGenerator.GetInstance().BossPatternObjGen(E_BOSSPATTERN.BREATH, 50, E_SPAWNLINE.LEFT);

                BossPatternGenerator.GetInstance().BossPatternObjGen(E_BOSSPATTERN.BREATH, 50, E_SPAWNLINE.RIGHT);
            }
            if (Input.GetKeyDown("x"))
            {GameObject t = 
                MapAndObjPool.GetInstance().GetBossObsFireBallInPool();
                t.transform.position = new Vector3(-10, 10, 100);
                t.SetActive(true);
            }
            //패턴 따라 넣어주는 처리.

            if (!stop)
            {
                mapSTList.MoveAllInList(moveSpeed);
                BossChunkLoading();
            }
        }

        public void StopMapMove()
        {
            stop = true;
        }

        void BossChunkLoading()
        {
            BossMapST bst = mapSTList.FindChunkShouldBeRemoved(chunkMargin);
            if (bst == null) return;

            bst.TurnInObjs();
            bst.MoveChunk(mapSTList.GetNewCreatePoint(chunkMargin));

        }
        


        void InitMapST()
        {
            BossMapST bst;
            float startPoint = -1*Constants.wantToShowNumOfChunksInBehind;
            for (int i = 0; i < Constants.wantToShowNumOfChunks; i++)
            {
                bst = new BossMapST();
                
                GameObject chunk = 
                MapAndObjPool.GetInstance().GetChunkInPool();

                if (chunk == null) ErrorManager.SpurtError("청크가 널임");
                if (startPoint == 0)
                    chunk.transform.position = Vector3.zero;
                else
                    chunk.transform.position = Vector3.forward*( startPoint * chunkMargin);

                bst.chunk = chunk;
                bst.chunk.SetActive(true);
                startPoint++;

                mapSTList.Add(bst);
            }
        }
    }

    
    public static class MyExtendMethodForListBossMapST
    {
        public static void MoveAllInList(this List<BossMapST> list , float moveSpeed)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if(list[i].IsDisabled() == false)
                list[i].MoveAll(moveSpeed);
            }
        }
        public static BossMapST FindChunkShouldBeRemoved(this List<BossMapST> list, float chunkMargin)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsDisabled()==false && list[i].IsShouldBeRemoved(chunkMargin))
                    return list[i];
            }
            return null;
        }
        public static float GetNewCreatePoint(this List<BossMapST> list, float chunkMargin)
        {
            float last = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsDisabled()) continue;

                if (list[i].chunk.transform.position.z > last)
                    last = list[i].chunk.transform.position.z;
            }
            return last + chunkMargin;
        }
    }

    public class BossMapST
    {
        public GameObject chunk;
        public List<GameObject> objs = new List<GameObject>();

        public void MoveAll(float moveSpeed)
        {
            chunk.transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);

            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
            }
        }
        public bool IsShouldBeRemoved(float chunkMargin)
        {
            if (chunk.transform.position.z <= -1 * ((Constants.wantToShowNumOfChunksInBehind+1) * chunkMargin))
                return true;
            else return false;
        }
        public void TurnInObjs()
        {
            for (int i = 0; i < objs.Count; i++)
            {
                MapAndObjPool.GetInstance().TurnInPoolObj(objs[i]);
            }
            objs.Clear();
        }
        public bool IsDisabled()
        {
            if (chunk==null)
                return true;
            else return false;
        }
        public void MoveChunk(float pos)
        {
            chunk.transform.position = Vector3.forward * pos;
        }
        public void ObjIn(int spawnPointNum, GameObject obj)
        {
            if (obj==null||IsDisabled()) ErrorManager.SpurtError("업음");

            Transform spawnPoint = chunk.transform.GetChild(3) //3은 굉장히 청크의 자식순서에 종속적인 값
                .GetChild(spawnPointNum);   

            obj.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            obj.SetActive(true);
            objs.Add(obj);
        }
    }
}