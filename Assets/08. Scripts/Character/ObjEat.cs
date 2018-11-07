using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using hcp;
public class ObjEat : MonoBehaviour, IObjToCharactor
{ // ,IObjToCharactor

    Collision collision;

    public static int Coin; // 동전수
    public static int HP = 3; // 체력 //처음3개 최대5개
    public static bool unityChanDie, HitInvincible = false; // 캐릭터가 죽거나 장애물에 피격된 상태확인
    public static bool Invincible, Shield, Magnet = false; // 무적, 방어막, 자석 상태확인
    public static float invincibleTime, shieldTime, magnetTime = 0.0f; // 무적, 방어막, 자석 지속시간확인

    public GameObject CoinEffect;
    public GameObject HeartEffect;

    private UIManager UIMgr;
    private CharacterAnimation cAnim;
    private bool iOverlap, sOverlap = false; // 방어막 겹쳤을때 확인
    private float invincibleCount, shieldCount, magnetCount = 0;

    void Start()
    {
        Coin = 0;
        cAnim = this.GetComponent<CharacterAnimation>();
        unityChanDie = false;
        HitInvincible = false;
        Invincible = false;
        Shield = false;
        Magnet = false;
        invincibleTime = 0;
        shieldTime = 0;
        magnetTime = 0;
        UIMgr = GameObject.Find("GameMgr").GetComponent<UIManager>();
        ItemState(); // 캐릭터, 아이템 상태 초기화 함수
        CoinEffect.SetActive(false);
        HeartEffect.SetActive(false);
        if (StageManager.stageNum == E_STAGE.BOSS) {
            HP = 1;
        }
    }

    void Update()
    {
        if (HP < 0) HP = 0;
        ShieldPower();
        MagnetPower();
        InvinciblePower();
        if(unityChanDie == false)
        {
            HPzero();
        }
    }

    void ItemState() // 캐릭터, 아이템 상태 초기화 함수
    {
        HP = 3;
        unityChanDie = false; // 캐릭터 생사 유무 초기화
        InvokeRepeating("ICount", 0, 1.0f); InvokeRepeating("SCount", 0, 1.0f); InvokeRepeating("MCount", 0, 1.0f); // 아이템 카운트 초기화
        Invincible = false; Shield = false; Magnet = false; // 아이템 상태 초기화
        invincibleTime = 0.0f; shieldTime = 0.0f; magnetTime = 0.0f; // 아이템 지속시간 초기화
        shieldCount = 0; magnetCount = 0; invincibleCount = 0; // 아이템 시간 초기화
    }

    public void GetItem(ItemST itemST) //아이템얻었을때
    {
        switch (itemST.itemType)
        {
            case E_ITEM.HPPLUS: //HP업먹음
                HeartEffect.SetActive(true);
                Invoke("EffectOff", 0.25f);
                if (HP < 5)
                {
                    HP++;
                }
                Debug.Log("HP " + HP + " 개");
                break;
            case E_ITEM.INVINCIBLE: //무적
                invincibleCount = 0;
                invincibleTime = 5;
                UIMgr.UI_Invincible.GetComponentInChildren<ProgressCoolDown>().MaxtTime = invincibleTime;
                UIMgr.UI_Invincible.GetComponentInChildren<ProgressCoolDown>().ItemTime = invincibleTime;
                Debug.Log("INVINCIBLE 먹음");
                break;
            case E_ITEM.SHIELD: //방어막먹음
                shieldCount = 0;
                shieldTime = 10;
                UIMgr.UI_Shield.GetComponentInChildren<ProgressCoolDown>().MaxtTime = shieldTime;
                UIMgr.UI_Shield.GetComponentInChildren<ProgressCoolDown>().ItemTime = shieldTime;
                if (Shield == true)
                {
                    sOverlap = true;
                    Invoke("ShieldEventOn", 1.0f);
                }
                if (Shield == false)
                {
                    ShieldEvent();
                }
                Debug.Log("SHIELD 먹음");
                break;
            case E_ITEM.COIN: //동전먹음
                CoinEffect.SetActive(true);
                Invoke("EffectOff", 0.25f);
                Coin++; // 동전수 +1
                break;
            case E_ITEM.MAGNET: //자석먹음
                magnetCount = 0;
                magnetTime = 10;
                UIMgr.UI_Magnet.GetComponentInChildren<ProgressCoolDown>().MaxtTime = magnetTime;
                UIMgr.UI_Magnet.GetComponentInChildren<ProgressCoolDown>().ItemTime = magnetTime;
                MagnetEvent();
                Debug.Log("MAGNET 먹음");
                break;
            case E_ITEM.EITEMMAX:
                Debug.Log("아이템 겟에서 오류");
                return;

        }
    }

    public void BeenHitByObs(ObstacleST obstacleST) //공격에맞았을때
    {
        //if (!obstacleST.beenHit) return;
        switch (obstacleST.obstacleType)
        {
            case E_OBSTACLE.BALL:
                DamagedEvent();
                Debug.Log("BALL");
                break;
            case E_OBSTACLE.HUDDLE:
                DamagedEvent();
                Debug.Log("HUDDLE");
                break;
            case E_OBSTACLE.UPPER_HUDDLE:
                DamagedEvent();
                Debug.Log("UPPER_HUDDLE");
                break;
            case E_OBSTACLE.FIRE:
                if (Invincible == false)
                {
                    if (Shield == false)
                    {
                        HP = 0;
                    }
                    DamagedEvent();
                    Debug.Log("FIRE");
                }
                break;
            case E_OBSTACLE.BOSS_FIREBALL:
                if (Invincible == false)
                {
                    if (Shield == false)
                    {
                        HP = 0;
                    }
                    DamagedEvent();
                    Debug.Log("FIRE");
                }
                break;
            case E_OBSTACLE.BOSS_BREATH:
                if (Invincible == false)
                {
                    if (Shield == false)
                    {
                        HP = 0;
                    }
                    DamagedEvent();
                    Debug.Log("FIRE");
                }
                break;
            case E_OBSTACLE.BOSS_METEOR:
                if (Invincible == false)
                {
                    if (Shield == false)
                    {
                        HP = 0;
                    }
                    DamagedEvent();
                    Debug.Log("FIRE");
                }
                break;
            case E_OBSTACLE.EOBSMAX:
                Debug.Log("옵스타클 충돌에서 오류");
                return;
        }
    }

    void ShieldPower()
    {
        ShieldEvent();
    }

    void ShieldEvent()
    {
        if (shieldTime > 0) Shield = true;
        if (shieldTime <= 0) Shield = false;
    }

    void ShieldEventOff()
    {
        shieldTime = 0;
    }

    void ShieldEventOn()
    {
        shieldTime = 9;
    }

    void MagnetPower()
    {
        MagnetEvent();
    }

    void MagnetEvent()
    {
        if (magnetTime > 0) Magnet = true;
        if (magnetTime <= 0) Magnet = false;
    }

    void InvinciblePower()
    {
        InvincibleEvent();
        InvincibleEventOn();
        InvincibleEventOff();
    }

    void InvincibleEvent()
    {
        if (invincibleTime > 0) Invincible = true;
        if (invincibleTime <= 0) Invincible = false;
    }

    void InvincibleEventOn() // 무적상태
    {
        if (Invincible == true)
        {
            CharacterMove.runSpeed = 32.5f;
            this.GetComponent<CapsuleCollider>().isTrigger = true; // 오브젝트 뚫고가기
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY; // Rigidbody Y포지션 고정
        }
    }

    void InvincibleEventOff() // 무적아닌상태
    {
        if (Invincible == false && HitInvincible == false)
        {
            CharacterMove.runSpeed = 6.5f;
            this.GetComponent<CapsuleCollider>().isTrigger = false; // 오브젝트 뚫기 해제
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation; // Rigidbody 포지션 초기화
        }
    }

    void HitInvincibleEvent() // 장애물에 맞았을때 무적, 무적해제
    {
        HitInvincibleEventOn();
        Invoke("HitInvincibleEventOff", 1.5f); // 1.5초간 무적
    }

    void HitInvincibleEventOn() // 장애물에 맞았을때 무적
    {
        HitInvincible = true;
        this.GetComponent<CapsuleCollider>().isTrigger = true; // 오브젝트 뚫고가기
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY; // Rigidbody Y포지션 고정
    }

    void HitInvincibleEventOff() // 장애물에 맞았을때 무적해제
    {
        this.GetComponent<CapsuleCollider>().isTrigger = false; // 오브젝트 뚫기 해제
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation; // Rigidbody 포지션 초기화
        HitInvincible = false;
    }

    void DamagedEvent() // 장애물과 닿을때 이벤트
    {
        if (Invincible == false && Shield == true)
        {
            Invoke("ShieldEventOff", 1.0f); // 방어막 제거
        }
        if (Invincible == false && Shield == false)
        {
            if (HitInvincible == false) 
            DamagedEvent2(); // 데미지입음
        }
        print("무적" + HitInvincible);
    }

    void DamagedEvent2() // 장애물에 부딪힐시 발생하는 이벤트
    {
        if (HP > 1)
        {
            //this.transform.Translate(Vector3.back * attacked * Time.deltaTime); // 적에게 닿은후 캐릭터의 위치가 뒤로 밀림 attacked값을 바꾸면 밀린정도를 바꿀수있음
            cAnim.DamageAnimation(); // 체력깎임 애니메이션 실행
            HP--;
            HitInvincibleEvent(); // 일시무적상태 돌입
            CharacterMove.runSpeed = CharacterMove.runSpeed / 2.0f; // 이동속도 줄어듬
            Invoke("DamagedEvent3", 1.5f);
        }
        else
        {
            if (HP == 1)
                HP--;
            if(unityChanDie == false)
            {
                unityChanDie = true;
                cAnim.DieAnimation(); //죽은 애니메이션
                this.GetComponent<CapsuleCollider>().isTrigger = true; // 오브젝트 뚫고가기
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY; // Rigidbody Y포지션 고정
                Invoke("GameOver", 1.0f); // 쓰러진뒤 2초뒤에 게임오버(게임이 정지되도록 만들어줌) 시켜주는 함수
                CharacterMove.runSpeed = 0;
            }
        }
    }

    void DamagedEvent3() // 데미지 닳았을때 이벤트
    {
        Debug.Log("체력닳음");
        Debug.Log("HP " + HP + " 개");
        CharacterMove.runSpeed = CharacterMove.runSpeed * 2.0f;
    }

    void HPzero()
    {
        if(HP == 0)
        {
            unityChanDie = true;
            cAnim.DieAnimation(); //죽은 애니메이션
            this.GetComponent<CapsuleCollider>().isTrigger = true; // 오브젝트 뚫고가기
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY; // Rigidbody Y포지션 고정
            Invoke("GameOver", 1.0f); // 쓰러진뒤 2초뒤에 게임오버(게임이 정지되도록 만들어줌) 시켜주는 함수
            CharacterMove.runSpeed = 0;
        }
    }

    public void GameOver() // 캐릭터가 쓰러진상태로 유지시켜주는 함수
    {
        unityChanDie = true;
    }

    public bool GetMagnetState()
    {
        return Magnet||Invincible;
    }

    void ICount() // 무적시간
    {
        if (Invincible == true && invincibleCount <= 5)
        {
            invincibleTime--;
            invincibleCount++;
        }
    }

    void SCount() // 방어막시간
    {
        if (Shield == true && shieldCount <= 10)
        {
            shieldTime--;
            shieldCount++;
        }
    }

    void MCount() // 자석시간
    {
        if (Magnet == true && magnetCount <= 10)
        {
            magnetTime--;
            magnetCount++;
        }
    }

    void EffectOff()
    {
        HeartEffect.SetActive(false);
        CoinEffect.SetActive(false);
    }

}