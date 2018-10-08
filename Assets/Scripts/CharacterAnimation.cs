using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{

    Animator animator;
    public Rigidbody rigidbody;

    private float speedZ = 80.0f;

    // Use this for initialization
    void Start()
    {
        this.animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //animator.Play("WALK00_F"); // 기본 애니메이션은 걷기
        Slide();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube")
        {
            Debug.Log("충돌됨");
            animator.Play("DAMAGE00", -1, 0);
            this.transform.Translate(Vector3.back * speedZ * Time.deltaTime);
        }
    }


    public void Slide() // 슬라이드하기
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("SLIDE00"))
            {
                Debug.Log("미끄러지기");
                animator.Play("SLIDE00", -1, 0);
            }
        }
    }
}
