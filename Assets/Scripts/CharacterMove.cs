using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour // 캐릭터의 실제 움직임담당 (좌표및 회전값)
{
    private static CharacterMove _instance = null;
    public static CharacterMove instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("CMoveController is NULL");
            return _instance;
        }
    }

    GameObject Character;

    private CharacterAnimation cAnim;
    public bool turningPoint = true; // 방향전환해야할 시점일경우 true 아닐경우에는 false
    public int horizontalLocation = 0;
    public static float runSpeed = 6.5f; // 캐릭터가 앞으로 달려가는 스피드

    public bool Loop = true; // while문 작동하시키기 위해 만듬 (별의미는 없음)
    public float rotateY = 0; // 캐릭터가 회전하는 각도
    // public float positionX = 0; // 캐릭터가 이동하는 좌표
    public int rotateLeftMax = 0; // 왼쪽으로 반복해서 움직이도록 할때 이용하려고 만든 변수
    public int rotateRightMax = 0; // 오른쪽으로 반복해서 움직이도록 할때 이용하려고 만든 변수
    public int turnLeftControl = 0; // 왼쪽 컨트롤을 제어하기 위해 만든 변수
    public int turnRightControl = 0; // 오른쪽 컨트롤을 제어하기 위해 만든 변수

    public float jumpHeight = 100.0f;
    public static bool enemyAttack = false;
    public static bool leftWall = false;
    public static bool rightWall = false;

    public static bool speedUpItem = false;
    public static int shield = 0;

    Rigidbody rigidbody;
    Collision collision;

    // Use this for initialization
    void Start()
    {
        Character = GameObject.Find("unitychan");
        cAnim = GetComponent<CharacterAnimation>();
        _instance = this;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyAttack == false) // 적에게 부딪히지 않았을경우
        {
            Run();
        }

    }
    public void Run() {
        this.transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
    }
    public void Move(bool isLeftDirection) // 좌우로 회전하거나 움직이는 이벤트를 담당하는 함수 turningPoint가 true일 경우와 false일경우로 나뉨
    {
        if (this.Character.transform.position.x > 0.1f)
        {
            if (isLeftDirection && rotateLeftMax == 0) // 왼쪽키 눌렀을때
            {
                /*
                    if (turnLeftControl == 1) // 왼쪽 움직임을 1번에 1번씩만 하도록 제한해주는 조건
                    {
                        rotateLeftMax = 0;
                        turnLeftControl = 0;
                    }
                    StartCoroutine(LeftSlide()); // 왼쪽으로 좌표를 부드럽게 이동하도록 하는 함수
                */
                // this.transform.Translate(-positionX, 0.0f, 0.0f);  // x축으로 -positionX값 만큼 이동시켜줌
                this.transform.Translate(-0.1f, 0.0f, 0.0f);  // 누른만큼 이동
            }
        }
        if (this.Character.transform.position.x < 9.5f)
        {
            if (!isLeftDirection && rotateRightMax == 0) // 오른쪽키 눌렀을때
            {
                /*
                if (turnRightControl == 1) // 오른쪽 움직임을 1번에 1번씩만 하도록 제한해주는 조건
                {
                    rotateRightMax = 0;
                    turnRightControl = 0;
                }
                StartCoroutine(RightSlide()); // 오른쪽으로 좌표를 부드럽게 이동하도록 하는 함수
                */
                //  this.transform.Translate(positionX, 0.0f, 0.0f);  // x축으로 positionX값 만큼 이동시켜줌
                this.transform.Translate(0.1f, 0.0f, 0.0f);  // 누른만큼 이동
            }
        }
    }
    public void Slide(bool isLeftDirection)
    {
        if (isLeftDirection && rotateLeftMax == 0) // 왼쪽으로 슬라이드 눌렀을때 (갈림길)
        {
            if (turnLeftControl == 1) // 왼쪽 움직임을 1번에 1번씩만 하도록 제한해주는 조건
            {
                rotateLeftMax = 0;
                turnLeftControl = 0;
            }
            StartCoroutine(LeftSlide()); // 왼쪽으로 90도를 자연스럽게 회전하도록 하는 함수
        }
        if (!isLeftDirection && rotateRightMax == 0)  // 오른쪽으로 슬라이드 눌렀을때 (갈림길)
        {
            if (turnRightControl == 1) // 오른쪽 움직임을 1번에 1번씩만 하도록 제한해주는 조건
            {
                rotateRightMax = 0;
                turnRightControl = 0;
            }
            StartCoroutine(RightSlide()); // 왼쪽으로 90도를 자연스럽게 회전하도록 하는 함수
        }
    }
    public void SlideDown() {
        cAnim.SlideAnimation();
    }
    public void Jump() // 점프
    {
        if (!CharacterAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("JUMP00"))
        {
            cAnim.JumpAnimation();
            rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse); // * 뒤 숫자를 조절하여 뛰는높이 조정가능
        }
    }

    IEnumerator LeftSlide() // 왼쪽으로 부드럽게 움직이도록 해주는 함수 (회전하는 각도와 좌표이동할때의 움직임)
    {
        while (Loop == true && rotateLeftMax < 15) // 회전과 이동을 15번 반복해서 실행시켜줌 단 회전할때와 이동할때는 각각 분리해서 실행함
        {
            rotateY = +6.0f; // y축으로 회전하는 각도
                             // positionX = +0.1f; // x축으로 이동하는 좌표

            this.transform.Rotate(0.0f, -rotateY, 0.0f); // y축으로 -rotateY값 만큼 회전시켜줌
            /*
            if (turningPoint == false) // 회전할때가 아닐때 // 이동할때
            {
                this.transform.Translate(-positionX, 0.0f, 0.0f); // x축으로 -positionX값 만큼 이동시켜줌
            }
            */
            yield return new WaitForSeconds(0.0001f); // 자연스럽게 캐릭터가 이동하는것처럼 보이기위해 ()안의 시간만큼 정지해서 멈춰있는 모습을 보여줌
            rotateLeftMax++; // 반복카운트 +1
        }
        if (rotateLeftMax == 15) // 회전과 이동의 반복문이 끝날경우
        {
            rotateLeftMax = 0; // 왼쪽 반복카운트 0으로 초기화
            turnLeftControl = 1;
        }
    }

    IEnumerator RightSlide() // 오른쪽으로 부드럽게 움직이도록 해주는 함수 (회전하는 각도와 좌표이동할때의 움직임)
    {
        while (Loop == true && rotateRightMax < 15)  // 회전과 이동을 15번 반복해서 실행시켜줌 단 회전할때와 이동할때는 각각 분리해서 실행함
        {
            rotateY = +6.0f; // y축으로 회전하는 각도
                             // positionX = +0.1f; // x축으로 이동하는 좌표

            this.transform.Rotate(0.0f, rotateY, 0.0f); // y축으로 rotateY값 만큼 회전시켜줌

            /*
            if (turningPoint == false) // 회전할때가 아닐때 // 이동할때
            {
                this.transform.Translate(positionX, 0.0f, 0.0f);  // x축으로 -positionX값 만큼 이동시켜줌
            }
            */
            yield return new WaitForSeconds(0.0001f); // 자연스럽게 캐릭터가 이동하는것처럼 보이기위해 ()안의 시간만큼 정지해서 멈춰있는 모습을 보여줌
            rotateRightMax++; // 반복카운트 +1
        }
        if (rotateRightMax == 15) // 회전과 이동의 반복문이 끝날경우
        {
            rotateRightMax = 0; // 오른쪽 반복카운트 0으로 초기화
            turnRightControl = 1;
        }
    }

    void OnCollisionStay(Collision collision) // 벽과 충돌시 발생하는 이벤트
    {
        if (collision.gameObject.tag == "LeftWall") // LeftWall태그(이름밑의 Tag)의 오브젝트와 충돌시 발생하는 이벤트
        {
            CharacterMove.leftWall = true; // 왼쪽에게 닿았다는것을 확인
            Debug.Log("왼쪽벽과 닿음");
        }
        else
        {
            CharacterMove.leftWall = false;
        }

        if (collision.gameObject.tag == "RightWall") // RightWall태그(이름밑의 Tag)의 오브젝트와 충돌시 발생하는 이벤트
        {
            CharacterMove.rightWall = true; // 오른쪽에게 닿았다는것을 확인
            Debug.Log("오른쪽벽과 닿음");
        }
        else
        {
            CharacterMove.rightWall = false;
        }
    }

    /*
    void OnCollisionEnter(Collision collision) // 아이템과 닿았다는것을 확인
    {
         // SpeedUpItem태그(이름밑의 Tag)의 오브젝트와 충돌시 발생하는 이벤트
        {
            Destroy(collision.gameObject); // 아이템 제거
            SpeedUpItem(); // 스피드업 아이템
        }

         // SpeedUpItem태그(이름밑의 Tag)의 오브젝트와 충돌시 발생하는 이벤트
        {
            shield = 1;
            Destroy(collision.gameObject); // 아이템 제거
            ShieldItem();
            Invoke("ShieldOff", 6f);
            //SpeedUpItem(); // 스피드업 아이템
        }
    }

    void SpeedUpItem() // 스피드업 아이템 기능구현 함수
    {
        speedUpItem = true; // 스피드업 아이템에게 닿았다는것을 확인
        Debug.Log("스피드업 아이템 먹음");
        runSpeed *= 5f; // 달리기 5배
        Invoke("SpeedRun", 3.5f); // 스피드업 기능을 3.5f시간만큼 지속
    }

    void SpeedRun() // 스피드업 기능
    {
        this.GetComponent<CapsuleCollider>().isTrigger = true; // 오브젝트 뚫고가기
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY; // Rigidbody Y포지션 고정
        // this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; 
        speedUpItem = false;
        runSpeed = runSpeed / 5f;
        Invoke("SpeedRunOff", 0.1f);
    }

    void SpeedRunOff() // 스피드업 기능
    {
        this.GetComponent<CapsuleCollider>().isTrigger = false; // 오브젝트 뚫기 해제
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // Rigidbody 포지션 초기화
    }

    void ShieldItem() // 쉴드기능
    {
        Debug.Log("쉴드 아이템 먹음");
        this.GetComponent<CapsuleCollider>().isTrigger = true;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }

    void ShieldOff()
    {
        this.GetComponent<CapsuleCollider>().isTrigger = false; // 오브젝트 뚫기 해제
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // Rigidbody 포지션 초기화
    }
    */

}
