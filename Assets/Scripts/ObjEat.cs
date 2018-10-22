using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using hcp;
public class ObjEat : MonoBehaviour, IObjToCharactor
{ // ,IObjToCharactor

    Collision collision;
    public static int Coin = 0; // 동전수
    public static int HP = 3; // 체력 //처음3개 최대5개
    public static bool Invincible = false;
    public static float invincibleTime = 0.0f;
    public static bool Shield = false;
    public static float shieldTime = 0.0f;
    public static bool Magnet = false;
    public static float magnetTime = 0.0f;
    private UIManager UIMgr;
    bool iOverlap, sOverlap, mOverlap = false; // 방어막 겹쳤을때 확인 mOverlap(자석)은 문제없으므로 안쓰임

    float shieldCount, magnetCount = 0;
    float invincibleCount = 0;
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("SCount", 0, 1.0f);
        InvokeRepeating("MCount", 0, 1.0f);
        InvokeRepeating("ICount", 0, 1.0f);
        UIMgr = GameObject.Find("GameMgr").GetComponent<UIManager>();
        HP = 3;
        Invincible = false;
        invincibleTime = 0.0f;
        Shield = false;
        shieldTime = 0.0f;
        Magnet = false;
        magnetTime = 0.0f;
        shieldCount = 0; magnetCount = 0; invincibleCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP < 0)
        {
            HP = 0;
        }

        ShieldEvent();
        MagnetEvent();
        InvicibleEvent();
        InvicibleEventOn();
        InvicibleEventOff();
    }

public void GetItem(ItemST itemST) //아이템얻었을때
    {
        switch (itemST.itemType)
        {
            case E_ITEM.HPPLUS: //HP업먹음
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
                    Invoke("ShieldEvent3", 1.0f);
                }
                if (Shield == false)
                {
                    ShieldEvent();
                }
                Debug.Log("SHIELD 먹음");
                break;
            case E_ITEM.COIN: //동전먹음
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
            case E_OBSTACLE.EOBSMAX:
                Debug.Log("옵스타클 충돌에서 오류");
                return;
        }
    }

    void ShieldEvent()
    {
        if (shieldTime > 0)
        {
            Shield = true;
        }
        if (shieldTime <= 0)
        {
            Shield = false;
        }
    }

    void ShieldEvent2()
    {
        shieldTime = 0;
    }

    void ShieldEvent3()
    {
        shieldTime = 9;
    }

    void MagnetEvent()
    {
        if (magnetTime > 0)
        {
            Magnet = true;
        }
        if (magnetTime <= 0)
        {
            Magnet = false;
        }
    }

    void InvicibleEvent()
    {
        if (invincibleTime > 0)
        {
            Invincible = true;
        }
        if (invincibleTime <= 0)
        {
            Invincible = false;
        }
    }

    void InvicibleEventOn() // 무적상태
    {
        if (Invincible == true)
        {
            CharacterMove.runSpeed = 32.5f;
            this.GetComponent<CapsuleCollider>().isTrigger = true; // 오브젝트 뚫고가기
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY; // Rigidbody Y포지션 고정
        }
    }

    void InvicibleEventOff() // 무적아닌상태
    {
        if (Invincible == false)
        {
            CharacterMove.runSpeed = 6.5f;
            this.GetComponent<CapsuleCollider>().isTrigger = false; // 오브젝트 뚫기 해제
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation; // Rigidbody 포지션 초기화
        }
    }

    void DamagedEvent() // 장애물과 닿을때 이벤트
    {
        if (Invincible == false && Shield == true)
        {
            Invoke("ShieldEvent2", 1.0f);
        }
        if (Invincible == false && Shield == false)
        {
            DamagedEvent2();
        }
    }

    void DamagedEvent2() // 적과 충돌시 발생하는 애니메이션
    {
        if (HP > 1)
        {
            //this.transform.Translate(Vector3.back * attacked * Time.deltaTime); // 적에게 닿은후 캐릭터의 위치가 뒤로 밀림 attacked값을 바꾸면 밀린정도를 바꿀수있음
            CharacterAnimation.DamageAnimation(); // 체력깎임 애니메이션 실행
            HP--;
            CharacterMove.runSpeed = CharacterMove.runSpeed / 2.0f;
            Invoke("DamagedEvent3", 1.5f);
        }
        else
        {
            if (HP == 1) HP--;
            CharacterAnimation.DieAnimation(); //죽은 애니메이션
            Invoke("GameOver", 1.0f); // 쓰러진뒤 2초뒤에 게임오버(게임이 정지되도록 만들어줌) 시켜주는 함수
            CharacterMove.runSpeed = 0;
        }
    }

    void DamagedEvent3() // 데미지 닳았을때 이벤트
    {
        Debug.Log("체력닳음");
        Debug.Log("HP " + HP + " 개");
        CharacterMove.runSpeed = CharacterMove.runSpeed * 2.0f;
    }

    public void GameOver() // 캐릭터가 쓰러진상태로 유지시켜주는 함수
    {
        Time.timeScale = 0; // 캐릭터 시간멈춤 (캐릭터가 쓰러진 모습 상태로 정지함)
    }

    public bool GetMagnetState()
    {
        return false;
    }

    void ICount()
    {
        if (Invincible == true && invincibleCount <= 5)
        {
            invincibleTime--;
            print("무적시간 : " + invincibleTime + " 초 남음");
            invincibleCount++;
        }
    }

    void SCount()
    {
        if (Shield == true && shieldCount <= 10)
        {
            shieldTime--;
            print("방어막시간 : " + shieldTime + " 초 남음");
            shieldCount++;
        }
    }

    void MCount()
    {
        if (Magnet == true && magnetCount <= 10)
        {
            magnetTime--;
            print("자석시간 : " + magnetTime + " 초 남음");
            magnetCount++;
        }
    }

}


