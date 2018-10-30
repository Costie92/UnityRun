using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour // 캐릭터의 애니메이션 + 충돌판정을 담당
{
    private static CharacterAnimation _instance = null;
    public static CharacterAnimation instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("CharacterAnimation is NULL");
            return _instance;
        }
    }
    public static Animator animator; // 애니메이션을 구현하기위해 쓴거
    private UIManager UIMgr;

    // Use this for initialization
    void Start()
    {
        UIMgr = GameObject.Find("GameMgr").GetComponent<UIManager>();
        animator = GetComponent<Animator>(); // 애니메이션을 구현하기위해 쓴거
    }

    // Update is called once per frame
    void Update()
    {
        InvincibleRunAnimation();
        animator.SetFloat("JumpSpeed", 1.2f * CharacterMove.speedUpdate); // 애니메이션 스피드 1.2f배로
    }
    
    public void DamageAnimation() // 캐릭터가 죽는모습을 보여주고 게임을 정지시킴
    {
        animator.Play("DAMAGED00", -1, 0); // 뒤로 쓰러지는 애니메이션 실행
    }

    public void DieAnimation() // 캐릭터가 죽는모습을 보여주고 게임을 정지시킴
    {
        animator.Play("DAMAGED01", -1, 0); // 뒤로 쓰러지는 애니메이션 실행
        Invoke("GameOver", animator.GetCurrentAnimatorStateInfo(0).length); // 쓰러진뒤 2초뒤에 게임오버(게임이 정지되도록 만들어줌) 시켜주는 함수
    }

    public void SlideAnimation() // 슬라이드 (아래방향키)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("SLIDE00") && !animator.GetCurrentAnimatorStateInfo(0).IsName("JUMP00")) // 슬라이드 애니메이션이 실행중이 아닐때 (중복해서 슬라이드 애니메이션이 실행하는것을 막아주는 조건)
        {
            Debug.Log("미끄러지기");
            animator.Play("SLIDE00", -1, 0); // 슬라이드하는 애니메이션 실행
            this.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.25f, 0); // 캐릭터 콜라이더 중심 옮기기
            this.GetComponent<CapsuleCollider>().height = 0.5f; // 캐릭터 콜라이더 높이 줄이기
            
        }
        Invoke("resetCollider", animator.GetCurrentAnimatorStateInfo(0).length); // 2초후 캐릭터 콜라이더를 되돌림
    }

    public void JumpAnimation() // 점프하기 (위방향키)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("JUMP00")) // 점프 애니메이션이 실행중이 아닐때 (중복해서 슬라이드 애니메이션이 실행하는것을 막아주는 조건)
        {
            Debug.Log("점프하기");
            animator.Play("JUMP00", -1, 0); // 점프하는 애니메이션 실행
            /*
            this.GetComponent<CapsuleCollider>().center = new Vector3(0, 1.5f, 0); // 캐릭터 콜라이더 중심 옮기기
            this.GetComponent<CapsuleCollider>().height = 0.5f; // 캐릭터 콜라이더 높이 줄이기
            */
        }
        Invoke("resetCollider", 1.0f); // 1초후 캐릭터 콜라이더를 되돌림
    }

    public void resetCollider() // 캐릭터 콜라이더 되돌리기
    {
        this.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.75f, 0); // 캐릭터 콜라이더 중심 옮기기
        this.GetComponent<CapsuleCollider>().height = 1.5f; // 캐릭터 콜라이더 높이 바꾸기
    }

    void InvincibleRunAnimation()
    {
        if(ObjEat.Invincible == true)
        {
            animator.SetFloat("RunSpeed", 2.0f);
        }
        else
        {
            animator.SetFloat("RunSpeed", 1.0f);
        }
    }

    public void RunAnimation()
    {
        if(ObjEat.HP != 0)
        {
            animator.Play("RUN00_F", -1, 0);
        }
    }

    public void GameOver() // 캐릭터가 쓰러진상태로 유지시켜주는 함수
    {
        UIMgr.ShowResult(); // 캐릭터 시간멈춤 (캐릭터가 쓰러진 모습 상태로 정지함)
    }

}
