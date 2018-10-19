using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour // 캐릭터의 애니메이션 + 충돌판정을 담당
{

    public static Animator animator; // 애니메이션을 구현하기위해 쓴거
    public Rigidbody rigidbody;

    private float attacked = 20.0f; // 적과 부딪힌뒤에 밀려나는 정도

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>(); // 애니메이션을 구현하기위해 쓴거
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* ObjEat으로 옮김
    void OnCollisionEnter(Collision collision) // 오브젝트와 충돌시 발생하는 이벤트
    {
        Debug.Log("캐릭터와 오브젝트가 닿음");
        if (collision.gameObject.tag == "Enemy") // Enemy태그(이름밑의 Tag)의 오브젝트와 충돌시 발생하는 이벤트
        {
            CharacterMove.shield--;
            if (CharacterMove.speedUpItem == false)
            {
                CharacterMove.enemyAttack = true; // 적에게 닿았다는것을 확인
                Debug.Log("적과 닿음");
                this.transform.Translate(Vector3.back * attacked * Time.deltaTime); // 적에게 닿은후 캐릭터의 위치가 뒤로 밀림 attacked값을 바꾸면 밀린정도를 바꿀수있음
                DieAnimation(); // 죽는 애니메이션 실행
            }
        }
    }
    */
    public static void DamageAnimation() // 캐릭터가 죽는모습을 보여주고 게임을 정지시킴
    {
        animator.Play("DAMAGED00", -1, 0); // 뒤로 쓰러지는 애니메이션 실행
    }

    public static void DieAnimation() // 캐릭터가 죽는모습을 보여주고 게임을 정지시킴
    {
        animator.Play("DAMAGED01", -1, 0); // 뒤로 쓰러지는 애니메이션 실행
        //Invoke("GameOver", 2.0f); // 쓰러진뒤 2초뒤에 게임오버(게임이 정지되도록 만들어줌) 시켜주는 함수
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
        Debug.Log("점프하기");
        animator.Play("JUMP00", -1, 0); // 점프하는 애니메이션 실행
    }

    public void resetCollider() // 캐릭터 콜라이더 되돌리기
    {
        this.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.75f, 0); // 캐릭터 콜라이더 중심 옮기기
        this.GetComponent<CapsuleCollider>().height = 1.5f; // 캐릭터 콜라이더 높이 바꾸기
    }

    public void GameOver() // 캐릭터가 쓰러진상태로 유지시켜주는 함수
    {
        Time.timeScale = 0; // 캐릭터 시간멈춤 (캐릭터가 쓰러진 모습 상태로 정지함)
    }

}
