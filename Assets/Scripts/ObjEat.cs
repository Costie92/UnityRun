using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using hcp;
public class ObjEat : MonoBehaviour,IObjToCharactor { // ,IObjToCharactor

    Collision collision;
    public static int Coin = 0; // 동전수
    public static int HP = 3; // 체력 //처음3개 최대5개
    bool speedUpItem = false;
    private float attacked = 20.0f;
    public static bool Invincible = false;
    public static bool Shield = false;
    public static bool Magnet = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(HP < 0)
        {
            HP = 0;
        }
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
                Debug.Log("INVINCIBLE 먹음");
                if (Invincible == false)
                {
                    Invincible = true;
                    INVINCIBLEItem();
                }
                break;
            case E_ITEM.SHIELD: //방어막먹음
                ShieldEvent();
                Debug.Log("SHIELD 먹음");
                break;
            case E_ITEM.COIN: //동전먹음
                Coin++; // 동전수 +1
                Debug.Log("COIN " + Coin +" 개");
                break;
            case E_ITEM.MAGNET: //자석먹음
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
                if(Invincible == false)
                {
                    HP = 0;
                    DamagedEvent();
                    Debug.Log("FIRE");
                }
                break;
            case E_OBSTACLE.EOBSMAX:
                Debug.Log("옵스타클 충돌에서 오류");
                return;
        }
    }

    void MagnetEvent()
    {
        Magnet = true;
        Invoke("MagentDestroy", 10.0f);
    }

    void MagentDestroy()
    {
        print("자석 끝");
        Magnet = false;
    }

    void ShieldEvent()
    {
        Shield = true;
        Invoke("ShieldDestroy", 10.0f);
    }

    void ShieldDestroy()
    {
        print("실드 자연파괴");
        Shield = false;
    }

    void INVINCIBLEItem() // 스피드업 아이템 기능구현 함수
    {
        Debug.Log("스피드업 아이템 먹음");
        CharacterMove.runSpeed *= 5f; // 달리기 5배
        Invoke("INVINCIBLE", 3.5f); // 스피드업 기능을 3.5f시간만큼 지속
    }

    void INVINCIBLE() // 스피드업 기능
    {
       this.GetComponent<CapsuleCollider>().isTrigger = true; // 오브젝트 뚫고가기
       this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY; // Rigidbody Y포지션 고정
        CharacterMove.runSpeed = CharacterMove.runSpeed / 5f;
        Invoke("INVINCIBLEOff", 0.1f);
    }

    void INVINCIBLEOff() // 스피드업 기능
    {
        this.GetComponent<CapsuleCollider>().isTrigger = false; // 오브젝트 뚫기 해제
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // Rigidbody 포지션 초기화
        Invincible = false;
    }

    void DamagedEvent()
    {
        if(Invincible == false && Shield == true)
        {
            print("실드 없어짐");
            Invoke("ShieldDestroy", 1.0f);
        }
        if(Invincible == false && Shield == false)
        {
            DamagedEvent2();
        }
    }

    void DamagedEvent2() // 적과 충돌시 발생하는 애니메이션
    {
        if(HP > 1)
        {
            //this.transform.Translate(Vector3.back * attacked * Time.deltaTime); // 적에게 닿은후 캐릭터의 위치가 뒤로 밀림 attacked값을 바꾸면 밀린정도를 바꿀수있음
            CharacterAnimation.DamageAnimation(); // 체력깎임 애니메이션 실행
            HP--;
            CharacterMove.runSpeed = CharacterMove.runSpeed /2.0f;
            Invoke("DamagedEvent3", 1.5f);
        }
        else
        {
            CharacterAnimation.DieAnimation(); //죽은 애니메이션
            Invoke("GameOver", 1.0f); // 쓰러진뒤 2초뒤에 게임오버(게임이 정지되도록 만들어줌) 시켜주는 함수
            CharacterMove.runSpeed = 0;
        }
    }

    void DamagedEvent3() // 데미지 닳았을때 이벤트
    {
        Debug.Log("체력닳음");
        Debug.Log("HP " + HP + " 개");
        CharacterMove.runSpeed = CharacterMove.runSpeed *2.0f;
    }

    public void GameOver() // 캐릭터가 쓰러진상태로 유지시켜주는 함수
    {
        Time.timeScale = 0; // 캐릭터 시간멈춤 (캐릭터가 쓰러진 모습 상태로 정지함)
    }

    public bool GetMagnetState()
    {
        return false;
    }
}
