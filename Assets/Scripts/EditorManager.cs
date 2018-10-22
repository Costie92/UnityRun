using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class SpriteArray
{
    public Sprite[] Data = new Sprite[3];
}
public class EditorManager : MonoBehaviour
{
    private static EditorManager _instance = null;
    public static EditorManager instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("EditorManager is NULL");
            return _instance;
        }
    }
    Camera cam;
    GameObject ChunkSelect;
    GameObject ObjSelect;
    Button btn_LeftObj;
    Button btn_CenterObj;
    Button btn_RightObj;
    public SpriteArray[] spriteArrays;
    public GameObject ClickedObj;
    public Sprite SelectImage;
    
    public int spriteindex;
    // Use this for initialization
    private void Awake()
    {
        spriteindex = 0;
        spriteArrays = new SpriteArray[100];
        ChunkSelect = GameObject.Find("ChunkSelect");
        ObjSelect = GameObject.Find("ObjSelect");
        btn_LeftObj = GameObject.Find("Btn_LeftObj").GetComponent<Button>();
        btn_CenterObj = GameObject.Find("Btn_CenterObj").GetComponent<Button>();
        btn_RightObj = GameObject.Find("Btn_RightObj").GetComponent<Button>();
    }
    void Start()
    {
        cam = Camera.main;
        ChunkSelect.SetActive(false);
        ObjSelect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickForward()
    {
        spriteArrays[spriteindex].Data[0] = btn_LeftObj.image.sprite;
        spriteArrays[spriteindex].Data[1] = btn_CenterObj.image.sprite;
        spriteArrays[spriteindex].Data[2] = btn_RightObj.image.sprite;
        btn_LeftObj.image.sprite = btn_CenterObj.image.sprite = btn_RightObj.image.sprite = SelectImage;
        cam.transform.position += new Vector3(0, 0, 10);
        spriteindex++;
    }
    public void OnClickBackward()
    {
        if (spriteindex <= 0)
            Debug.LogError("첫 부분입니다");
        spriteindex--;
        btn_LeftObj.image.sprite = spriteArrays[spriteindex].Data[0];
        btn_CenterObj.image.sprite = spriteArrays[spriteindex].Data[1];
        btn_RightObj.image.sprite = spriteArrays[spriteindex].Data[2];
        cam.transform.position -= new Vector3(0, 0, 10);
    }
    public void OnClickObj(Button btn)
    {
        ClickedObj = btn.gameObject;
        if (ChunkSelect)
        {
            ChunkSelect.SetActive(false);
        }
        ObjSelect.SetActive(true);
    }
    public void OnClickNextChunk()
    {
        if (ObjSelect)
        {
            ObjSelect.SetActive(false);
        }
        ChunkSelect.SetActive(true);
    }
}