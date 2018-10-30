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

    public class BossMapMgr : SingletonTemplate<BossMapMgr>
    {
        public GameObject chunk;
        public float moveSpeed = 10.0f;
        List<BossMapST> mapSTList = new List<BossMapST>();
        float chunkMargin;

        protected override void Awake()
        {
            base.Awake();
            chunkMargin = chunk.GetComponent<Renderer>().bounds.size.z;
            
        }
        private void Start()
        {
            MapAndObjPool.GetInstance().ChunkPoolInit(Constants.wantToShowNumOfChunks);

            InitMapST();
        }

        private void Update()
        {
            //패턴 따라 넣어주는 처리.
            mapSTList.MoveAllInList(moveSpeed);
            BossChunkLoading();
            ServeBossPattern(); //꼭 새롭게 생긴(이동한) 청크에 오브젝트를 소환시킬 게 아니라
            //플레이어 기준으로 몇 미터 앞에 떨어져있는 청크에 소환한다든지 하는 조작이 가능한 방향이 더 나은듯
        }

        void BossChunkLoading()
        {
            BossMapST bst = mapSTList.FindChunkShouldBeRemoved(chunkMargin);
            if (bst == null) return;

            bst.TurnInObjs();
            bst.MoveChunk(mapSTList.GetNewCreatePoint(chunkMargin));

        }

        void ServeBossPattern()
        {
            //큐로 받은 패턴을 뽑아서
            //맞게 넣어줌

            //그런데 문제가 되는게 보스가 파이어볼 쏘는 건 보스 쪽에서 끝나는 시간을 알 수가 있음
            //브레스나 메테오도 보스 쪽에서 끝낼 시간을 알아야함
            //콜백 메소드로 이쪽 메소드를 보스 패턴쪽에서 호출하게?
            //플레이어 몇 미터 앞에 있는 청크의 몇 라인에 불을 생성한다든지 하는 식으로.
            //이게 좋은듯

            //그러면 보스가 패턴 실행이 끝났다는 걸 이쪽이 알아야하고
            //또 브레스나 메테오를 언제 실행할 건지를 알아내서 그에 맞게 청크에 불 오브젝트를 소환해줄 수 있어야함.

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
            if (chunk.transform.position.z < -1 * ((Constants.wantToShowNumOfChunksInBehind+1) * chunkMargin))
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