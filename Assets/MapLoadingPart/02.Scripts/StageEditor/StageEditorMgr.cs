﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp {
    public class StageEditorMgr : SingletonTemplate<StageEditorMgr> {

        int[] candidates = {  -1, 0, 1, 2, 3, 4, 5 };    //7개 보여줌. 하나는 뒤에꺼.
        

        private int position=0;
        public int Position
        {
            get
            {
                return position;
            }
        }

        float chunkMargin;
        public GameObject chunk;
        GameObject startingBlock;

        bool canWork;

        public List<StageEditorST> EditSTList = new List<StageEditorST>();
        

        void Start() {
            MapObjPoolingGeneration();
            canWork = false;
            startingBlock = GameObject.Find("StartingBlock");
            chunkMargin = chunk.GetComponentInChildren<Renderer>().bounds.size.z;
            position = Constants.firstObjSpawn;
            StartCoroutine( BringToWorkspace());
        }
        

        void SortEditSTList()   //pos로 정렬
        {
            EditSTList.Sort(
                delegate(StageEditorST x , StageEditorST y)
                {
                    int compareDate = x.pos.CompareTo(y.pos); 
                    return compareDate;
                }
                );
        }

        // Update is called once per frame
        void Update() {
            if (!canWork) return;
            
            //ui단에서 호출 후 맨 마지막에 position 값 읽어서 텍스트 처리 할것!

            if (Input.GetKeyDown ("w"))
            {
                MoveForward();
            }
            if (Input.GetKeyDown("s"))
            {
                MoveBackward();
            }
            if (Input.GetKeyDown("a"))
            {
                ChangeFloorChunk(E_WhichTurn.LEFT);
            }
            if (Input.GetKeyDown("d"))
            {
                ChangeFloorChunk(E_WhichTurn.NOT_TURN);
            }
            if (Input.GetKeyDown("r"))
            {
                ChangeFloorChunk(E_WhichTurn.RIGHT);
            }
            if (Input.GetKeyDown("1"))
            {
                ChangeObjThisChunk(0, E_SPAWN_OBJ_TYPE.BALL);
            }
            if (Input.GetKeyDown("9"))
            {
                ChangeObjThisChunk(0, E_SPAWN_OBJ_TYPE.FIRE);
            }
            if (Input.GetKeyDown("2"))
            {
                ChangeObjThisChunk(1, E_SPAWN_OBJ_TYPE.COIN);
            }
            if (Input.GetKeyDown("3"))
            {
                ChangeObjThisChunk(2, E_SPAWN_OBJ_TYPE.HUDDLE);
            }
            if (Input.GetKeyDown("m"))
            {
                StageDataMgr sdm = new StageDataMgr();
                sdm.SaveData(EditSTList);
            }
            if (Input.GetKeyDown("l"))
            {
                StageDataMgr sdm = new StageDataMgr();
                sdm.LoadData();
            }


            //   ShowChunks();
            //  DisappearAllChunks();
            //이제 실제 에디팅 로직

        }
        void ClearAndShowChunks()
        {
            DisappearAllChunks();
            //받은 포지션 값의 청크가 회전 청크인지, 그 범위에 포함인지, 등을 알아내어 적절한 처리를 꼭 해줄것!!

            if (position < Constants.firstObjSpawn) ErrorManager.SpurtError("포지션이 2보다 작음!!");

            if (position == Constants.firstObjSpawn)
            {
                ClearAndStartingShowChunks();
                return;
            }

            if (startingBlock.activeSelf == true)
            {
                startingBlock.SetActive(false);
            }
            

            for (int i = 0; i < candidates.Length; i++)
            {
                //-1 경우에만 전에 턴청크 때문에 안그려줘야하는지 체크해줄 필요가 있어.
                if (i == 0 && IsPositionInTurnChunksNullArea(position + candidates[0])) continue;

                StageEditorST sest = 
                    MakeChunk(candidates[i], position);
                if (sest.IsTurnChunks())                     //회전 청크가 나오면 그만만듦
                {
                    break;
                }
            }
        }

        void ClearAndStartingShowChunks()   //pos가 2일때는 이걸 불러야함.
        {
            if (startingBlock.activeSelf == false)
            {
                startingBlock.transform.position = new Vector3(0, 0, -Constants.firstObjSpawn * chunkMargin);
                startingBlock.SetActive(true);
            }

            for (int i = 1; i < candidates.Length; i++)
            {
                MakeChunk(candidates[i], position);//스타팅이니까 회전청크는 신경쓸 필요 없을듯.
            }
        }

        StageEditorST MakeChunk(int candidateNum, int standardPos)
        {
            int pos = candidateNum + standardPos;
           // print(pos + "에 메이크 청크");
            for (int i = 0; i < EditSTList.Count; i++)
            {
                if (EditSTList[i].pos == pos)
                {
                    EditSTList[i].showThisToEditor(standardPos, chunkMargin);
                    return EditSTList[i];
                }
            }

            StageEditorST sest = new StageEditorST(pos, E_WhichTurn.NOT_TURN);
            EditSTList.Add(sest);
            sest.showThisToEditor(standardPos,chunkMargin);
            return sest;
        }

        void DisappearAllChunks()
        {
            for (int i = 0; i < EditSTList.Count; i++)
            {
                if (EditSTList[i].isNowShowed)
                {
                    EditSTList[i].DisappearFromEditor();
                }
            }
        }
        
        IEnumerator BringToWorkspace() //에디터 시작할때 스무스하게 이동하는 느낌 유도
        {
            float backPoint = -Constants.firstObjSpawn * chunkMargin;
            while (startingBlock.transform.position.z - 0.1f > backPoint)
            {
                startingBlock.transform.position = Vector3.Lerp(startingBlock.transform.position, new Vector3(0, 0, backPoint), Time.deltaTime * 2.0f);
                yield return new WaitForSeconds(0.01f);
            }
            startingBlock.transform.position = new Vector3(0,0, backPoint);


            ClearAndShowChunks();

            canWork = true;
        }

        void MapObjPoolingGeneration()
        {
            MapAndObjPool.GetInstance().ChunkPoolInit(10);

            MapAndObjPool.GetInstance().TurnLeftChunksPoolInit (3);
            MapAndObjPool.GetInstance().TurnRightChunksPoolInit(3);

            MapAndObjPool.GetInstance().obsBallPoolInit(30);
            MapAndObjPool.GetInstance().obsHuddlePoolInit(30);
            MapAndObjPool.GetInstance().obsUpperHuddle_1_PoolInit(30);
            MapAndObjPool.GetInstance().obsUpperHuddle_2_PoolInit(30);
            MapAndObjPool.GetInstance().obsUpperHuddle_3_PoolInit(30);
            MapAndObjPool.GetInstance().obsFirePoolInit(30);

            MapAndObjPool.GetInstance().itemHPPlusPoolInit(30);
            MapAndObjPool.GetInstance().itemInvinciblePoolInit(30);
            MapAndObjPool.GetInstance().itemShieldPoolInit(30);
            MapAndObjPool.GetInstance().itemCoinPoolInit(30);
            MapAndObjPool.GetInstance().itemMagnetPoolInit(30);
            MapAndObjPool.GetInstance().itemCoin_Parabola_PoolInit(30);
            MapAndObjPool.GetInstance().itemCoin_StraightLine_PoolInit(30);
        }

        StageEditorST FindEditSTByPos(int pos)
        {
            for (int i = 0; i < EditSTList.Count; i++)
            {
                if (EditSTList[i].pos == pos)
                {
                    return EditSTList[i];
                }
            }
            return null;
        }

        void ModerateListForTurnChunks(int pos)//pos는 회전 청크가 생긴 지점.
        {
            StageEditorST sest = FindEditSTByPos(pos);
            if (sest.whichTurn == E_WhichTurn.NOT_TURN) ErrorManager.SpurtError("회전 청크도 아닌데 지우겠다고");

            for (int i = 1; i < Constants.turningTerm; i++)
            {
                if (i < 5)  //새로 생긴 회전 청크를 위해 비워두는 구간.
                {
                    sest = FindEditSTByPos(pos + i);
                    if (sest != null)   //여기 뭔가 있으면
                    {
                        sest.ReadyForGrave();
                        EditSTList.Remove(sest);
                    }
                }
                //이후 애들은 터닝텀에 걸리므로 턴청크일때 삭제해줘야함.
                else
                {
                    sest = FindEditSTByPos(pos + i);
                    if (sest!=null && sest.IsTurnChunks())
                    {
                        sest.ReadyForGrave();
                        EditSTList.Remove(sest);
                    }
                }
            }
        }

        bool IsPositionInTurnChunksNullArea(int pos, ref int posIfItWasNull)   
        {
            if (pos < Constants.turningTerm) return false;

            StageEditorST sest = FindEditSTByPos(pos);
            if (sest != null) return false;  //뭐라도 있으니까 상관없음.
            
            //널인 경우라면.  회전 청크 보간 청크라서 널인지를 파악
            //4는 굉장히 회전 청크의 상태에 의존한 값임!
            for (int i = -1; i >= -4; i--)
            {
                sest = FindEditSTByPos(pos + i);
                if (sest != null &&sest.IsTurnChunks())
                {
                    posIfItWasNull = sest.pos;
                    return true;
                }
            }
            return false;
        }
        bool IsPositionInTurnChunksNullArea(int pos)    
        {
            if (pos < Constants.turningTerm) return false;

            StageEditorST sest = FindEditSTByPos(pos);
            if (sest != null) return false;  //뭐라도 있으니까 상관없음.

            //널인 경우라면.  회전 청크 보간 청크라서 널인지를 파악
            //4는 굉장히 회전 청크의 상태에 의존한 값임!
            for (int i = -1; i >= -4; i--)
            {
                sest = FindEditSTByPos(pos + i);
                if (sest != null && sest.IsTurnChunks())
                {
                    return true;
                }
            }
            return false;
        }


        void PosAdjustByTurnChunkInFront()
        {
            if (position < Constants.turningTerm - 1)
            {
                position++;
                return;
            }

            StageEditorST sest = FindEditSTByPos(position); //현재 포지션이 턴청크라면!
            if (sest != null && sest.IsTurnChunks())
                position += 5;    //턴청크 구간 점프
            else position++;
        }

        void PosAdjustByTurnChunkInBack()
        {
            if (position <= Constants.turningTerm)
            {
                position--;
                return;
            }
            int posBeforeTurn=0;
            if (IsPositionInTurnChunksNullArea(position - 1, ref posBeforeTurn))
            {
                position = posBeforeTurn;
            }
            else position--;
        }

        int GetLastPosInList()
        {
            SortEditSTList();
            return EditSTList[EditSTList.Count - 1].pos;
        }

        //얘들 나중에 인터페이스로 뺼까 말까 한번 고민해보자.
        //아니면 콜백을 걸어놓든지.
        public void ChangeObjThisChunk(int spawnPointNum, E_SPAWN_OBJ_TYPE objType)
        {
            StageEditorST sest = FindEditSTByPos(position);
            if (sest == null) ErrorManager.SpurtError("절대 있을 수 없는 , 호출을 잘못한 듯.");

            sest.ChangeObj(spawnPointNum, objType);
        }

        public void ChangeFloorChunk(E_WhichTurn whichTurn)
        {
            if (CanItBeTurnChunks() == false)
            {
                Debug.Log("회전 조건 충족 못함");
                return;
            }

            StageEditorST sest = FindEditSTByPos(position);
            if (sest == null) ErrorManager.SpurtError("절대 있을 수 없는 , 호출을 잘못한 듯.");

            sest.ChangeFloor(whichTurn);

            if(whichTurn != E_WhichTurn.NOT_TURN)   //회전 청크로 바꿨다면 조정
            ModerateListForTurnChunks(position);

            ClearAndShowChunks();   //청크를 바꿨으면 빈공간 나가리가 쭉 생기게 되니까.
        }

        public bool CanItBeTurnChunks()
        {
            if (position < Constants.turningTerm)
            {
                return false;
            }
            for (int i = -1; i > -Constants.turningTerm; i--)    //전 터닝텀 구간 까지 터닝 청크가 있는지 체크.
            {
                StageEditorST sest = FindEditSTByPos(position + i);
                if (sest!=null &&  sest.IsTurnChunks())
                    return false;
            }
            return true;
        }

        public void MoveForward()
        {
            PosAdjustByTurnChunkInFront();
            ClearAndShowChunks();
        }
        public void MoveBackward()
        {
            if (position == Constants.firstObjSpawn) return;

            PosAdjustByTurnChunkInBack();
            ClearAndShowChunks();
        }
        public void MoveByNumber(int number)//position을 키보드로 바꿔서 요청했을때.
        {
            if (number < Constants.firstObjSpawn)
            {
                position = Constants.firstObjSpawn;
                ClearAndShowChunks();
                return;
            }
            int last = GetLastPosInList();
            if (last > number)
            {
                position = last;
                ClearAndShowChunks();
                return;
            }
            int beforeTurn = 0;
            if (IsPositionInTurnChunksNullArea(number, ref beforeTurn))
            {
                position = beforeTurn;
                ClearAndShowChunks();
                return;
            }

            position = number;
            ClearAndShowChunks();
            return;
        }
    }
}