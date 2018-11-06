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

    private CharacterAnimation cAnim;

    public static bool bossStage = false; // BossStage스크립트에서 true됨
    public static int shield = 0;
    public static float runSpeed = 6.5f; // 캐릭터가 앞으로 달려가는 스피드
    public static float speedUpdate = 1.2f;
    public bool turningPoint = true; // 방향전환해야할 시점일경우 true 아닐경우에는 false
    private int rotateLeftMax, rotateRightMax, rotateUpMax, rotateDownMax, turnLeftControl, turnRightControl = 0; // 왼쪽, 오른쪽으로 반복해서 움직이도록 할때 이용하려고 만든 변수 // 높이를 제어하기 위한 변수 // 왼쪽, 오른쪽 컨트롤을 제어하기 위해 만든 변수
    private float rotateY, temp = 0; // 캐릭터가 회전하는 각도 // 달리기 속도를 잠시 저장해주기 위해 필요한것

    Rigidbody rigidbody;
    Collision collision;
    GameObject Character;

    void Start()
    {
        bossStage = false;
        runSpeed = 6.5f;
        Character = GameObject.FindWithTag("PLAYER");
        cAnim = GetComponent<CharacterAnimation>();
        _instance = this;
        rigidbody = GetComponent<Rigidbody>();

        speedUpdate = 1.2f;
        temp = 0;
    }

    void Update()
    {
        if(bossStage == false)
        {
            if (ObjEat.HP != 0) // 적에게 부딪히지 않았을경우
            {
                Run();
            }
            InvokeRepeating("GoFast", 60.0f, 60.0f);
        }
        else if(bossStage == true)
        {
            runSpeed = 0;
        }
        JumpDown();
        CharacterStop();
    }

    public void Run() // 캐릭터가 앞을향해 달리는 함수 (속도조절포함)
    {
        this.transform.Translate(Vector3.forward * runSpeed * speedUpdate * Time.deltaTime); // runSpeed는 일반달리기 속도, speedUpdate는 가속
    }

    public void GoFast() // 시간이 지날때마다 이동속도 증가
    {
        if(speedUpdate < 2.0f && CharacterAnimation.Win == false) speedUpdate = speedUpdate + 0.05f * Time.deltaTime; // 가속도 설정 최대 2배속이 넘으면 중지
    }

    public void Move(bool isLeftDirection) // 캐릭터 이동
    {
        if (this.Character.transform.position.x > 2f) // 왼쪽이동 한계치 설정
        {
            if (isLeftDirection && rotateLeftMax == 0) this.transform.Translate(-0.1f * Time.deltaTime * 75, 0.0f, 0.0f); // 왼쪽키 눌렀을때 // 누른만큼 이동
        }
        if (this.Character.transform.position.x < 8f) // 오른쪽이동 한계치 설정
        {
            if (!isLeftDirection && rotateRightMax == 0) this.transform.Translate(0.1f * Time.deltaTime * 75, 0.0f, 0.0f); // 오른쪽키 눌렀을때 // 누른만큼 이동
        }
    }

    public void SlideDown() // 캐릭터가 장애물 아래로 지나가기
    {
        if (ObjEat.Invincible == false) cAnim.SlideAnimation(); // 무적상태가 아닐때 애니메이션 실행
    }

    public void Jump() // 점프
    {
        if (!CharacterAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("JUMP00") && ObjEat.Invincible == false) // 점프상태가 아닐때, 무적상태가 아닐때
        {
            cAnim.JumpAnimation(); // 점프애니메이션
            rigidbody.velocity = new Vector3(0, 6.0f * speedUpdate, 0); // 캐릭터가 위로 올라감
        }
    }

    public void JumpDown() // 점프취소 (강제로 점프를 취소함)
    {
        if (Character.transform.position.y > 2.5f) // 캐릭터가 2.5위치 위로 올라갈경우
        {
            rigidbody.AddForce(Vector3.down * 40 * speedUpdate, ForceMode.Impulse); // 캐릭터에게 아래로 힘을 가해줌
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) // 아래 방향키 (1회)누르면
        {
            if (CharacterAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("JUMP00") && ObjEat.Invincible == false)
            {
                rigidbody.AddForce(Vector3.down * 120 * speedUpdate, ForceMode.Impulse); // 캐릭터에게 아래로 힘을 가해줌
                this.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.75f, 0); // 캐릭터 콜라이더 중심 옮기기
                this.GetComponent<CapsuleCollider>().height = 1.5f; // 캐릭터 콜라이더 높이 바꾸기
            }
        }
    }
    
    void OnCollisionEnter(Collision collision) // 땅과 닿았을때 걷기(점프 취소 구현을 위해 만든 함수)
    {
        this.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.75f, 0); // 캐릭터 콜라이더 중심 옮기기
        this.GetComponent<CapsuleCollider>().height = 1.5f; // 캐릭터 콜라이더 높이 바꾸기
        if (!CharacterAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("RUN00_F") && !CharacterAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("SLIDE00")) // 다른 애니메이션과 겹쳐서 실행되지 않도록 해주는 조건
        {
            cAnim.RunAnimation(); // 캐릭터가 걷는 애니메이션을함
        }
    }

    void CharacterStop() // 피격시 잠시 캐릭터멈춤
    {
        if (ObjEat.HitInvincible == true) // 피격시
        {
            temp = runSpeed;
            runSpeed = 0; // 캐릭터가 멈춤
            Invoke("runStart",1.0f); // 1.0f시간동안 멈춤
        }
    }

    void runStart() // 피격시 줄어들었던 달리기 스피드를 복구시켜주는 함수
    {
        runSpeed = temp;
    }

}
