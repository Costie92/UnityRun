using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDie : MonoBehaviour {

    // Use this for initialization
    void Start () {
        CharacterAnimation.animator = GetComponent<Animator>(); // 애니메이션을 구현하기위해 쓴거
    }
	
	// Update is called once per frame
	void Update () {
        FallingDie();
    }

    public void Die() // 캐릭터가 죽는모습을 보여주고 게임을 정지시킴
    {
        Invoke("GameOver", 0.0f); // 쓰러진뒤 2초뒤에 게임오버(게임이 정지되도록 만들어줌) 시켜주는 함수
    }

    public void FallingDie() // 캐릭터 낙사
    {
        if(this.transform.position.y < -10f)
        {
            Die();
            ObjEat.HP = 0;
        }
    }

    public void GameOver() // 캐릭터가 쓰러진상태로 유지시켜주는 함수
    {
        Time.timeScale = 0; // 캐릭터 시간멈춤 (캐릭터가 쓰러진 모습 상태로 정지함)
    }

}
