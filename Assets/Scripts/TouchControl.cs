using System.Collections;
using System.Collections.Generic;
using hcp;
using UnityEngine;

public delegate void SwipeScreen(float tPoint, hcp.E_WhichTurn eTurn);

public class TouchControl : MonoBehaviour,IMapTurnToUI {
    
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
    E_WhichTurn whichTurn;
    public float turningpoint;
    private GameObject unitychan;
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
    public event SwipeScreen swipeScreen;
    public static bool isTurn;
    // Use this for initialization
    void Awake () {
        _instance = this;
        TapCount = 0;
        //Screen.SetResolution(1920, 1080, false);
        height = Screen.height;
        width = Screen.width;
        isTurn = false;
    }
    void Start() {
        
        cMove = GameObject.FindWithTag("PLAYER").GetComponent<CharacterMove>();
        unitychan = GameObject.FindWithTag("PLAYER");
    }
    // Update is called once per frame
    void Update()
    {
        if (turningpoint == 0) {
            isTurn = false;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        if (ObjEat.Invincible)
        {
            SwipeToTurn();
        }
        //마우스 클릭시 포지션 저장
        if (!UIManager.isPause)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Input.mousePosition.y > (height / 2.5))
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
                    if (MousePosY < (height / 2.5))
                    {
                        //좌측으로 이동
                        if (MousePosX < (width / 2))
                        {
                            cMove.turningPoint = false;
                            cMove.Move(true);
                        }
                        //우측으로 이동
                        else
                        {
                            cMove.turningPoint = false;
                            cMove.Move(false);
                        }
                    }
                }
            }
            //마우스 클릭 종료시
            else if (Input.GetMouseButtonUp(0))
            {
                //좌로 스와이프
                if (TapCount == 1)
                {
                    if (ButtonDownMousePos.x - MousePosX > (width / 5))
                    {
                        SetWhichTurnToUI(E_WhichTurn.LEFT);
                        print("Swipe Left");
                        SwipeToTurn();


                    }
                    //우로 스와이프
                    else if (MousePosX - ButtonDownMousePos.x > (width / 5))
                    {
                        SetWhichTurnToUI(E_WhichTurn.RIGHT);
                        print("Swipe Right");
                        SwipeToTurn();
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
    public void SwipeToTurn() {
        //한번만부르게 바꾸기
        if (!isTurn && turningpoint != 0 && unitychan.transform.position.z > turningpoint)
        {
            isTurn = true;
            swipeScreen(turningpoint, whichTurn);
        }
    }


    public float GetTurningPointInUI()
    {
        return turningpoint;
    }

    public E_WhichTurn GetWhichTurnInUI()
    {
        return whichTurn;
    }

    public void SetTurningPointToUI(float turningPoint)
    {
        this.turningpoint = turningPoint;
    }

    public void SetWhichTurnToUI(E_WhichTurn whichTurn)
    {
        this.whichTurn = whichTurn;
    }
}
