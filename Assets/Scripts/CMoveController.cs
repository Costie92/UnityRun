using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveController : MonoBehaviour {

    public Transform Trans;
    public float speedMovements = 100f;
    private float height;
    private float width;
    private float MousePosX;
    private float MousePosY;
    private int TapCount;
    private Vector2 ButtonDownMousePos = new Vector2(0,0);
    public float MaxDubbleTapTime;

    // Use this for initialization
    void Start () {
        TapCount = 0;
        Screen.SetResolution(1920, 1080, false);
        height = Screen.height;
        width = Screen.width;
        Trans = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //마우스 클릭시 포지션 저장
        if (Input.GetMouseButtonDown(0)) {
            if (Input.mousePosition.y > (height / 2))
            {
                ButtonDownMousePos = Input.mousePosition;
                TapCount = 1;
            }
        }
        // 마우스 클릭 유지시
        if (Input.GetMouseButton(0))
        {
            MousePosX = Input.mousePosition.x;
            MousePosY = Input.mousePosition.y;
            //하단 부분 클릭 했을 경우
            if (TapCount == 0)
            {
                if (MousePosY < (height / 2))
                {
                    //좌측으로 이동
                    if (MousePosX < (width / 2))
                    {
                        Trans.position -= transform.right * Time.deltaTime * speedMovements;
                    }
                    //우측으로 이동
                    else
                    {
                        Trans.position += transform.right * Time.deltaTime * speedMovements;
                    }
                }
            }
        }
        //마우스 클릭 종료시
        else if (Input.GetMouseButtonUp(0)) {
            //좌로 스와이프
            if (ButtonDownMousePos.x - MousePosX > (width / 5))
            {
                print("Swipe Left");
            }
            //우로 스와이프
            else if (MousePosX - ButtonDownMousePos.x > (width / 5))
            {
                print("Swipe Right");
            }
            //위로 스와이프
            else if (MousePosY - ButtonDownMousePos.y > (height / 10))
            {
                print("Swipe Up");
                
            }
            //아래로 스와이프
            else if (ButtonDownMousePos.y - MousePosY > (height / 10))
            {
                print("Swipe Down");
            }
            TapCount = 0;
        }
    }
}
