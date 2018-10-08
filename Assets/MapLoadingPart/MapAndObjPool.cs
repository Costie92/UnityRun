using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAndObjPool : MonoBehaviour{
    public GameObject chunk;
    public GameObject obstacleBall;

    public List<GameObject> chunkPool = new List<GameObject>();
    public List<GameObject> obsBallPool = new List<GameObject>();
    public static MapAndObjPool instance=null;

    private int chunkPoolCapacity = 10;
    private int obsBallPoolCapacity = 5;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        Debug.Log("풀링 셋업어웨이크 종료.");
    }
    public void ChunkPoolInit(int capacity1=10)
    {
        GameObject chunkPoolParent = new GameObject("chunkPoolParent");

        this.chunkPoolCapacity = capacity1;
        if (!chunk)
        {
            Debug.Log("풀링 청크에서 오류.");
            return;
        }
        Debug.Log("청크풀링이닛중.");
    
        for(int i=0;i< chunkPoolCapacity; i++)
        {
            Debug.Log("청크 생성중.");
            var temp = Instantiate<GameObject>(chunk, chunkPoolParent.transform);
            temp.name = "CHUNK_" + i.ToString("00");
            Debug.Log(temp.name+"풀에 생성");
            temp.SetActive(false);  //활성화 여부로 풀링 제공 
            chunkPool.Add(temp);
            Debug.Log(chunkPool[i].name + "풀에서 확인"); 
        }
    }


    public void obsBallPoolInit(int capacity1 = 5)
    {
        GameObject obsBallPoolParent = new GameObject("obsBallPoolParent");
        obsBallPoolParent.transform.position += Vector3.up * 2f;
        this.obsBallPoolCapacity = capacity1;
        if (!obstacleBall)
        {
            return;
        }

        for (int i = 0; i < obsBallPoolCapacity; i++)
        {
            var temp = Instantiate<GameObject>(obstacleBall, obsBallPoolParent.transform);
            temp.name = "OBSBALL_" + i.ToString("00");
            Debug.Log(temp.name + "풀에 생성");
            temp.SetActive(false);  //활성화 여부로 풀링 제공 
            obsBallPool.Add(temp);
            Debug.Log(obsBallPool[i].name + "풀에서 확인");
        }
    }



    public GameObject GetChunkInPool()
    {
        Debug.Log("겟청크하러왔음. 현재 풀에는"+chunkPool.Count+"갯수가 있음");
        for (int i=0;i<chunkPool.Count;i++)
        {
            Debug.Log("청크포문돌리는중.");
            if (chunkPool[i].activeSelf == false)
            { Debug.Log("청크받아감.");
                return chunkPool[i];
            }
        }
        return null;
    }

    public GameObject GetObsBallInPool()
    {
        Debug.Log("겟장애물볼하러왔음. 현재 풀에는" + obsBallPool.Count + "갯수가 있음");
        for (int i = 0; i < obsBallPool.Count; i++)
        {
            Debug.Log("청크포문돌리는중.");
            if (obsBallPool[i].activeSelf == false)
            {
                Debug.Log("청크받아감.");
                return obsBallPool[i];
            }
        }
        return null;
    }

    public void TurnInPoolObj(GameObject temp)
    {
        temp.transform.position = Vector3.zero;
        temp.transform.rotation = Quaternion.identity;
        //오브젝트 풀링 경우
        temp.SetActive(false);//비활성화로 풀링에 반납
    }
}
