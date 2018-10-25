using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public class StageEditorST
    {
        public int pos;     //회전 청크일때 나중에 직렬화 할때 꼭 터닝포인트로 넣어주는거 잊지말기!
        public GameObject floorChunk;
        public bool isNowShowed=false;
        public E_WhichTurn whichTurn;
        public List<GameObject> objs = new List<GameObject>();

        StageObjArr soa = new StageObjArr();    //스폰 타입 저장

        Vector3 bornPos = new Vector3(0, 0, 0);

        Transform spg;

        public void showThisToEditor(int standardPos, float chunkMargin)
        {
            bornPos.z = (pos - standardPos) * chunkMargin;  //이렇게 하면 기준 점기준으로 현재 pos가 가리키는 위치를 얻을 수 있어.

            Debug.Log(bornPos.ToString() + "에 생성!!");

            switch (whichTurn)
            {
                
                case E_WhichTurn.LEFT:
                    //왼쪽 으로 청크 풀링 해오기.
                    floorChunk =
                        MapAndObjPool.GetInstance().GetTurnLeftChunksInPool();
                    break;
                case E_WhichTurn.RIGHT:
                    //오른쪽으로 청크 풀링
                    floorChunk =
                        MapAndObjPool.GetInstance().GetTurnRightChunksInPool();
                    break;
                case E_WhichTurn.NOT_TURN:
                    floorChunk
                        = MapAndObjPool.GetInstance().GetChunkInPool();
                    break;
                default: ErrorManager.SpurtError("위치턴이 아무것도 아닌 심각한 오류.");
                    break;
            }
            floorChunk.transform.position = bornPos;
            floorChunk.SetActive(true);

            if (whichTurn == E_WhichTurn.NOT_TURN)
            {
                spg = floorChunk.transform.GetChild(3);
                FixedObjGenerator.FixedObjGen(spg, soa.spawnObjType, ref objs);
            }
            isNowShowed = true;
        }

        public void DisappearFromEditor()
        {
            if (floorChunk == null)
                ErrorManager.SpurtError("생성도 안됐는데 삭제를 불러와짐");
           
                //턴청크일때와 그냥 청크일때 맵 풀링 고려해서 짜기.
                MapAndObjPool.GetInstance().TurnInPoolObj(floorChunk);

                floorChunk = null;
                spg = null;

            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i] != null)
                    MapAndObjPool.GetInstance().TurnInPoolObj(objs[i]);
            }
            objs.Clear();//참조만 날라감. 괜찮음.

            isNowShowed = false;
        }

        public void ChangeObj(int objNum, E_SPAWN_OBJ_TYPE changingType)
        {
            if (floorChunk == null) ErrorManager.SpurtError("생성도 안됐는데 오브젝트를 바꾸려함.");

            if (whichTurn != E_WhichTurn.NOT_TURN) ErrorManager.SpurtError("회전 청크인데 오브젝트를 체인지 하려고함.");

            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i] != null)
                    MapAndObjPool.GetInstance().TurnInPoolObj(objs[i]);
            }
            objs.Clear();//참조만 날라감. 괜찮음.

            soa.spawnObjType[objNum] = changingType;

            spg = floorChunk.transform.GetChild(3);

            FixedObjGenerator.FixedObjGen(spg, soa.spawnObjType, ref objs);
        }

        public void ChangeFloor(E_WhichTurn whichTurn)    //그냥 청크면 낙턴으로 보내줄것.
        {
            if (floorChunk == null) ErrorManager.SpurtError("생성도 안됐는데 청크를 바꾸려함,");
            if (this.whichTurn == whichTurn) ErrorManager.SpurtError("위치턴이 같은데 체인지 플로어를 부름");

            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i] != null)
                    MapAndObjPool.GetInstance().TurnInPoolObj(objs[i]);
            }
            objs.Clear();//참조만 날라감. 괜찮음.

            soa.spawnObjType[0] = E_SPAWN_OBJ_TYPE.NOTHING;
            soa.spawnObjType[1] = E_SPAWN_OBJ_TYPE.NOTHING;
            soa.spawnObjType[2] = E_SPAWN_OBJ_TYPE.NOTHING;

            this.whichTurn = whichTurn;
            //위치턴 따라서 플로어 청크 분기해오기.
            bornPos.z = floorChunk.transform.position.z;
            MapAndObjPool.GetInstance().TurnInPoolObj(floorChunk);

            switch (whichTurn)
            {
                case E_WhichTurn.LEFT:
                    //왼쪽 으로 청크 풀링 해오기.
                    floorChunk =
                       MapAndObjPool.GetInstance().GetTurnLeftChunksInPool();
                    break;
                case E_WhichTurn.RIGHT:
                    //오른쪽으로 청크 풀링
                    floorChunk =
                     MapAndObjPool.GetInstance().GetTurnRightChunksInPool();
                    break;
                case E_WhichTurn.NOT_TURN:
                    floorChunk
                        = MapAndObjPool.GetInstance().GetChunkInPool();
                    break;
                default:
                    ErrorManager.SpurtError("위치턴이 아무것도 아닌 심각한 오류.");
                    break;
            }
            floorChunk.transform.position = bornPos;
            floorChunk.SetActive(true);
        }

        public StageEditorST(int pos, E_WhichTurn whichTurn)
        {
            this.pos = pos;
            this.whichTurn = whichTurn;
        }

        public bool IsTurnChunks()
        {
            if (whichTurn != E_WhichTurn.NOT_TURN)
                return true;
            else return false;
        }

        public void ReadyForGrave()
        {
            if (floorChunk != null)
            {
                MapAndObjPool.GetInstance().TurnInPoolObj(floorChunk);
            }
            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i] != null)
                    MapAndObjPool.GetInstance().TurnInPoolObj(objs[i]);
            }
            objs.Clear();//참조만 날라감. 괜찮음.

        }
    }
}