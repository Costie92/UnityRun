using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SwipeScreen(float tPoint, hcp.E_WhichTurn eTurn);

public class TouchControl : MonoBehaviour {
    
    private static TouchControl _instance = null;
    public static TouchControl instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("TouchControl is NULL");
            return _instance;
        }
    }
    
    public float turningpoint;
    private CharacterMove cMove;
    private CharacterAnimation cAnim;
    public float speedMovements = 100f;
    private float height;
    private float width;
    private float MousePosX;
    private float MousePosY;
    private int TapCount;
    private Vector2 ButtonDownMousePos = new Vector2(0,0);
    public float MaxDubbleTapTime;
    public static event SwipeScreen swipeScreen;
    // Use this for initialization
    public float GetTpoint() { return turningpoint; }
    public void SetTpoint(float Tpoint) { turningpoint = Tpoint; }
    void Start () {
        _instance = this;
        cMove = GameObject.Find("unitychan").GetComponent<CharacterMove>();

        TapCount = 1;
        Screen.SetResolution(1920, 1080, false);
        height = Screen.height;
        width = Screen.width;
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
                        swipeScreen(turningpoint, hcp.E_WhichTurn.LEFT);
                        cMove.turningPoint = false;
                        cMove.Move(true);
                    }
                    //우측으로 이동
                    else
                    {
                        swipeScreen(turningpoint, hcp.E_WhichTurn.RIGHT);
                        cMove.turningPoint = false;
                        cMove.Move(false);
                    }
                }
            }
        }
        //마우스 클릭 종료시
        else if (Input.GetMouseButtonUp(0)) {
            //좌로 스와이프
            if (TapCount == 1) {
                if (ButtonDownMousePos.x - MousePosX > (width / 5))
                {
                    cMove.Slide(true);
                    print("Swipe Left");
                }
                //우로 스와이프
                else if (MousePosX - ButtonDownMousePos.x > (width / 5))
                {
                    cMove.Slide(false);
                    print("Swipe Right");
                }
                //위로 스와이프
                else if (MousePosY - ButtonDownMousePos.y > (height / 10))
                {
                    cMove.Jump();
                    print("Swipe Up");

                }
                //아래로 스와이프
                else if (ButtonDownMousePos.y - MousePosY > (height / 10))
                {
                    cMove.SlideDown();
                    print("Swipe Down");
                }
                TapCount = 0;
            }
        }
    }
}
