using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace hcp
{
    
    public class ObjInCharge : SingletonTemplate<ObjInCharge>
    {
        Dictionary<float, List<GameObject>> spawnedObjDic;

        List<float> spawnObjPosList = new List<float>();
        List<float> delObjPosList = new List<float>();
        Dictionary<float, Transform> toCrtObjs = new Dictionary<float, Transform>();

        protected override void Awake()
        {
            base.Awake();
            spawnedObjDic = DataOfMapObjMgr.spawnedObjDic;
        }
        
        public void  AdjustObjSpawn()
        {
            spawnObjPosList.Clear();
            delObjPosList.Clear();
            toCrtObjs.Clear();
            List<float> chunkKeys   =   DataOfMapObjMgr.chunkOnMap.Keys.ToList();
            List<float> objKeys = DataOfMapObjMgr.spawnedObjDic.Keys.ToList();
          
            foreach (float ck in chunkKeys)
            {
                bool check = false;
                foreach (float ok in objKeys)
                {
                    if (ck == ok) check = true;
                }
                if(check==false)
                {
                    spawnObjPosList.Add(ck);
                }
            }
            foreach (float ok in objKeys)
            {
                bool check = false;
                foreach (float ck in chunkKeys)
                {
                    if (ok == ck) check = true;
                }
                if(check==false)
                {
                    delObjPosList.Add(ok);
                }
            }
           
            foreach (var n in spawnObjPosList)
                Debug.Log("생성할 스폰 위치 = " + n);
            foreach (var n in delObjPosList)
                Debug.Log("삭제할 스폰 위치" + n);

            foreach(float sp in spawnObjPosList)
            {
                Transform spg = DataOfMapObjMgr.chunkOnMap[sp].transform.Find("SpawnPointGroup");
                toCrtObjs.Add(sp, spg);
            }

            DelObjsOnPosList(delObjPosList);
            CreateObjsInChunk(toCrtObjs);


            /*링큐 도중 포기 결과가 float 리스트로 변환되지 않고
             * 웨어절에서 default 로 체크하면 0 이 체킹되지 않아 문제
             * nullable float 으로 해서 null로 체크해도 float list 로 변환 시킬 때 문제가 생김

            List<float> mocking = new List<float>();
            mocking.Add(20);
            mocking.Add(10);
            mocking.Add(-10);
            mocking.Add(0);
            foreach (var obj in mocking)
                Debug.Log("모킹 " + obj);

            var t =
                  from c in chunkKeys   //청크중 안겹치는 애들. 생성할 애들.
                            join o in mocking
                  on c equals o into ords
                  from loj in ords.DefaultIfEmpty()
                  where loj==default(float)
                            // from loj in ords.DefaultIfEmpty()
                            select new  { c };

            foreach (var obj in t)
            {
                Debug.Log("링큐 결과 " + obj);
               // spawnObjPosList.Add((float)obj);
            }
            
            
            ();
            spawnObjPosList =t.ToList<float>();
            if (spawnObjPosList == null) Debug.Log("sedfasdg");

            var ot = from o in chunkKeys
                     join c in chunkKeys
                     on o equals c into ords
                     from loj in ords.DefaultIfEmpty()
                     select new { loj };
            //delObjPosList = (List<float>)ot;
            */
        }

        public void CreateObjsInChunk(Dictionary<float, Transform>   toCrtObjs)
        {
            foreach (var dicKeyValuePair in toCrtObjs) //가져온 위치의 청크마다
            {
                List<GameObject> list = DataOfMapObjMgr.FindEmptyGList();
                if (list == null) return;

                Transform spgParent = dicKeyValuePair.Value;

                    for (int i = 0; i < spgParent.childCount; i++)
                    {
                    List<GameObject> t = 
                    RandomObjGenerator.GetInstance().RandomObjGen(spgParent.GetChild(i), i);
                        //스퐅 포인트 별로 오브젝트 생성 후 저장
                    if (null != t) list.AddRange(t);
                    }
                spawnedObjDic.Add(dicKeyValuePair.Key, list);
            }
        }

        private void Update()  
        {
            if(Input.GetMouseButtonDown(2))
            {
                AdjustObjSpawn();
                /*
                foreach (var go in spawnedObjDic)
                {
                    foreach (var g in go.Value)
                        print(go.Key + " 키에 밸류로" + g.name);
                }
                */
            }
        }
        public void DelObjsOnPosList(List<float> DelPosList)
        {
            List<float> delCandidates = DelPosList;
            foreach(var delPos in delCandidates)
            {
                if (spawnedObjDic.ContainsKey(delPos))
                {
                    foreach (var spawned in spawnedObjDic[delPos])
                    {
                        if (
                          //  spawned.transform.position.z < MapObjManager.GetInstance().GetPosByChunkMargin()&&
                          //이건 마지막 수단.
                         spawned.activeSelf == true)

                        spawned.SetActive(false);   //풀링에 반납.
                    }
                    spawnedObjDic[delPos].Clear();
                    spawnedObjDic.Remove(delPos);
                }
            }
        }
    }
}