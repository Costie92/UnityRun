using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hcp;

public class BossAttack : MonoBehaviour {

    public Transform dragonFace;
    Animator dragon;
    string attackName = "Idle01";
    public int bossHP = 0;
    public bool bossHurt = false;
    private float dragonPosition, dragonDash = 0;
    private float i,j,a,b = 0; // 반복문쓸때 쓰는 변수
    IBossPattern iboss;
    GameObject UnityChan;

    void Start() {
        dragonFace = GameObject.Find("BossFace").transform;
        iboss = GameObject.Find("BossMapManager").GetComponent<IBossPattern>();
        dragonPosition = 0;
        dragonDash = 0;
        attackName = "Idle01";
        bossHP = 5;
        bossHurt = false;
        dragon = GetComponent<Animator>();
        attackName = GetRandomAttack();
        InvokeRepeating("GetRandomAttack", 6.0f, 6.0f); // 공격 패턴 랜덤으로 바꿔주기
        Time.timeScale = 1;
        UnityChan = GameObject.FindGameObjectWithTag("PLAYER");
    }

    void Update() {
        DragonDamaged(); // 용 얻어맞았을때 이벤트
        DragonMove(); // 용 공격 이벤트
    }

    void DragonMove() // 용 공격 이벤트
    {
        if(bossHurt == false) // 얻어맞지 않았을때
        {
            if (bossHP > 0) // 살아있을때
                AttackStart();
            if (bossHP <= 0) // 죽었을때
                Die();
        }
    }

    string GetRandomAttack() // 공격종류 랜덤으로 지정
    {
        int randomAttack = Random.Range(1, 5);
        attackName = "Idle01";
        switch (randomAttack)
        {
            case 1:
                attackName = "Basic Attack"; // 한발씩쏘기
                BasicAttack();
                Scream();
                break;
            case 2:
                attackName = "Flame Attack"; // 길게쏘기
                FlameAttack();
                Scream();
                break;
            case 3:
                attackName = "Scream"; // 위로쏘기
                Scream();
                Invoke("Scream", 0.5f);
                break;
            case 4:
                attackName = "Fly Forward"; // 날아와서 때리기
                FlyForward();
                break;
        }
        return attackName;
    }
    
    void BasicAttack()
    {
        Invoke("DragonBall", 0.25f - 0.1f);
        Invoke("DragonBall", 1.25f - 0.1f);
        Invoke("DragonBall", 2.25f - 0.1f);
        Invoke("DragonBall", 3.25f - 0.1f);
        Invoke("DragonBall", 4.25f - 0.1f);
        Invoke("DragonBall", 5.25f - 0.1f);
    }

    void FlameAttack()
    {
        Invoke("DragonBreath", 0.5f - 0.1f);
        Invoke("DragonBreath", 1.5f - 0.1f);
        Invoke("DragonBreath", 3.5f - 0.1f);
        Invoke("DragonBreath", 4.5f - 0.1f);
    }

    void Scream()
    {
        Invoke("DragonMeteor", 1.0f - 0.1f); Invoke("DragonMeteor", 1.0f - 0.1f);
        Invoke("DragonMeteor", 2.0f - 0.1f);
        Invoke("DragonMeteor", 2.8f - 0.1f); Invoke("DragonMeteor", 2.8f - 0.1f);
        Invoke("DragonMeteor", 4.0f - 0.1f);
        Invoke("DragonMeteor", 4.8f - 0.1f); Invoke("DragonMeteor", 4.8f - 0.1f);
        Invoke("DragonMeteor", 5.8f - 0.1f);
    }

    void FlyForward()
    {
        Invoke("Claw", 2.5f);
        Invoke("ClawAttack", 3.0f);
        Invoke("Fly", 3.5f);
        DragonFront();
        Invoke("DragonJump", 0.75f);
        Invoke("DragonRecovery", 2.0f);
        Invoke("DragonJump", 3.5f);
        Invoke("DragonRecovery", 5.0f);
    }

    void AttackStart() // 공격 애니메이션(attackName에 따라 다른것이 실행됨)
    {
        if (!dragon.GetCurrentAnimatorStateInfo(0).IsName(attackName))
        {
            dragon.Play(attackName, -1, 0);
        }
    }

    void DragonBall() // 불쏘기
    {
        iboss.BossPatternFireBallShoot(dragonFace.position);
    }

    void DragonBreath() // 브레스
    {
        int randomline = Random.Range(1, 4);
        switch (randomline)
        {
            case 1:
                iboss.BossPatternObjGen(E_BOSSPATTERN.BREATH, 60, E_SPAWNLINE.LEFT);
                break;
            case 2:
                iboss.BossPatternObjGen(E_BOSSPATTERN.BREATH, 60, E_SPAWNLINE.CENTER);
                break;
            case 3:
                iboss.BossPatternObjGen(E_BOSSPATTERN.BREATH, 60, E_SPAWNLINE.RIGHT);
                break;
        }
    }

    void DragonMeteor() // 메테오
    {
        int randomline = Random.Range(1, 4);
        switch (randomline)
        {
            case 1:
                iboss.BossPatternObjGen(E_BOSSPATTERN.METEOR, 50, E_SPAWNLINE.LEFT);
                break;
            case 2:
                iboss.BossPatternObjGen(E_BOSSPATTERN.METEOR, 50, E_SPAWNLINE.CENTER);
                break;
            case 3:
                iboss.BossPatternObjGen(E_BOSSPATTERN.METEOR, 50, E_SPAWNLINE.RIGHT);
                break;
        }
    }

    void DragonDamaged() // 용 얻어맞았을때 위로 올라감
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // 체력닳을때 조건
        {
            if (bossHP > 1)
            {
                bossHurt = true;
                bossHP--;
                Defend();
                Invoke("DragonJump", 1.0f);
            }
            else
            {
                bossHurt = true;
                bossHP--;
                Die();
            }
        }
    }

    void DragonFront()
    {
        StartCoroutine(BossFront());
        Invoke("DragonBack", 4.5f);
    }

    void DragonBack() // 용 다시 내려옴
    {
        StartCoroutine(BossBack());
    }

    IEnumerator BossFront() 
    {
        while (a < 75)
        {
            dragonDash = dragonDash + 0.021f;
            this.transform.Translate(0.0f, 0.0f, dragonDash);
            yield return new WaitForSeconds(0.01f);
            a++; // 반복카운트 +1
        }
        if (a == 75)
        {
            dragonDash = 0;
            a = 0;
        }
    }

    IEnumerator BossBack()
    {
        while (b < 75)
        {
            dragonDash = dragonDash - 0.021f;
            this.transform.Translate(0.0f, 0.0f, dragonDash);
            yield return new WaitForSeconds(0.01f);
            b++; // 반복카운트 +1
        }
        if (b == 75)
        {
            dragonDash = 0;
            b = 0;
        }
    }

    void DragonJump()
    {
        StartCoroutine(BossUp()); // 용 y위치가 위로뜸
    }

    void DragonRecovery() // 용 다시 내려옴
    {
        StartCoroutine(BossDown());
        bossHurt = false;
    }

    IEnumerator BossUp()  // 용 y위치가 위로뜸
    {
        while (i < 50) 
        {
             dragonPosition = dragonPosition + 0.01f;
             this.transform.Translate(0.0f, dragonPosition, 0.0f); 
             yield return new WaitForSeconds(0.0001f);
             i++; // 반복카운트 +1
        }
        if(i == 50)
        {
            dragonPosition = 0;
            i = 0;
        }
    }

    IEnumerator BossDown() // 용 y위치 원래대로
    {
        while (j < 50)
        {
            dragonPosition = dragonPosition - 0.01f;
            this.transform.Translate(0.0f, dragonPosition, 0.0f);
            yield return new WaitForSeconds(0.0001f);
            j++; // 반복카운트 +1
        }
        if(j == 50)
        {
            dragonPosition = 0;
            j = 0;
        }
    }

    void Defend() // 얻어맞는 애니메이션
    {
        dragon.Play("Defend", -1, 0);
    }

    void Die() // 죽는 애니메이션
    {
        dragon.Play("Die", -1, 0);
    }

    void Fly()
    {
        attackName = "Fly Forward";
    }

    void Claw()
    {
        attackName = "Claw Attack";
    }

    void ClawAttack()
    {
        if(UnityChan.transform.position.x > 3.5f && UnityChan.transform.position.x < 6.5)
        {
            ObjEat.HP = 0;
        }
    }
}
