using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAndObjPool : MonoBehaviour{
    public GameObject chunk;
    public List<GameObject> chunkPool = new List<GameObject>();
    public static MapAndObjPool instance=null;

    private int capacity=10;
    

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

        this.capacity = capacity1;
        if (!chunk)
        {
            Debug.Log("풀링 청크에서 오류.");
            return;
        }
        Debug.Log("청크풀링이닛중.");
    
        for(int i=0;i<capacity;i++)
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
}
