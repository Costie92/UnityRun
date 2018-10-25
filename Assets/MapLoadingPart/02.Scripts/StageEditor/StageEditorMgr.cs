using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp {
    public class StageEditorMgr : SingletonTemplate<StageEditorMgr> {

        int[] candidates = {  -1, 0, 1, 2, 3, 4, 5 };    //7개 보여줌. 하나는 뒤에꺼.

        int position;
        float chunkMargin;
        public GameObject chunk;
        GameObject startingBlock;

        bool canWork;

        List<StageEditorST> EditSTList = new List<StageEditorST>();
        

        void Start() {
            MapObjPoolingGeneration();
            canWork = false;
            startingBlock = GameObject.Find("StartingBlock");
            chunkMargin = chunk.GetComponentInChildren<Renderer>().bounds.size.z;
            position = Constants.firstObjSpawn;
            StartCoroutine( BringToWorkspace());
        }

        // Update is called once per frame
        void Update() {
            if (!canWork) return;

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
                //전 터닝텀 구간에 회전 청크가 있으면 바꾸지 못하게 하기!
                if (CanItBeTurnChunks()) 
                    ChangeFloorChunk(E_WhichTurn.LEFT);

                else print("회전 청크가 올 수 없음!");
            }
            if (Input.GetKeyDown("d"))
            {
                ChangeObjThisChunk(1, E_SPAWN_OBJ_TYPE.BALL);
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

            int tp = this.position;

            this.position = IsPositionInTurnChunks(); //포지션이 턴 청크 구간에 포함되면 포지션을 보완!(턴청크 pos 위치로.)
            //포워드 백워드 맞춰서 처리해줄 필요 있음.
            //백워드 면 이렇게 바로 포지션을 조정해주는 게 맞는데
            //포워드 경우면 회전 청크 구간을 쩜프해줘야할 필요가 있어.
            if (tp > this.position) //포워드로 불려진 구간임!
            {
                position = tp + 5;  //쩜프 (턴 청크 구간 다음의 포지션)
                //쩜프했을때 -1 구간의 청크가 안보여야만함 *****
                for (int i = 1; i < candidates.Length; i++)
                {
                    StageEditorST sest =
                        MakeChunk(candidates[i], position);
                    if (sest.IsTurnChunks())                     //회전 청크가 나오면 그만만듦
                    {
                       
                        break;
                    }
                }
            }
            

            for (int i = 0; i < candidates.Length; i++)
            {
                StageEditorST sest = 
                    MakeChunk(candidates[i], position);
                if (sest.IsTurnChunks())                     //회전 청크가 나오면 그만만듦
                {
                    ModerateListForTurnChunks(sest.pos);
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
            print(pos + "에 메이크 청크");
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

        int IsPositionInTurnChunks()
        {
            StageEditorST sest = FindEditSTByPos(position);
            if (sest != null) return position;  //뭐라도 있으니까 상관없음.

            print("일단 현재 포지션 청크는 널임.!");
            //널인 경우라면.  회전 청크 보간 청크라서 널인지를 파악
            //4는 굉장히 회전 청크의 상태에 의존한 값임!
            for (int i = -1; i >= -4; i--)
            {
                print(position + i);
                sest = FindEditSTByPos(position+i);
                if (sest != null &&sest.IsTurnChunks())
                {
                    print("전 단계의 회전 포인트 - "+ sest.pos);
                    return sest.pos;
                }
            }
            return position;
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
            StageEditorST sest = FindEditSTByPos(position);
            if (sest == null) ErrorManager.SpurtError("절대 있을 수 없는 , 호출을 잘못한 듯.");

            sest.ChangeFloor(whichTurn);

            if (whichTurn != E_WhichTurn.NOT_TURN) ModerateListForTurnChunks(position);

            ClearAndShowChunks();   //청크를 바꿨으면 빈공간 나가리가 쭉 생기게 되니까.
        }

        public bool CanItBeTurnChunks()
        {
            if (position < Constants.turningTerm)
            {
                print("전 터닝텀 구간에 회전 청크가 있어서 불가능함.");
                return false;
            }
            for (int i = -1; i >= -Constants.turningTerm; i--)    //전 터닝텀 구간 까지 터닝 청크가 있는지 체크.
            {
                StageEditorST sest = FindEditSTByPos(position + i);
                if (sest!=null &&  sest.IsTurnChunks())
                    return false;
            }
            return true;
        }

        public void MoveForward()
        {
            position++;
            ClearAndShowChunks();
        }
        public void MoveBackward()
        {
            if (position == Constants.firstObjSpawn) return;

            position--;
            ClearAndShowChunks();
        }







    }
}