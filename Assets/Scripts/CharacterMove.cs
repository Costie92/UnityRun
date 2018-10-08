using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour {

    public bool turningPoint = true;
    public int horizontalLocation = 0;
    public int runSpeed = 1;

    public bool Loop = true;
    public float rotateY = 0;
    public float positionX = 0;
    public int rotateLeftMax = 0;
    public int rotateRightMax = 0;
    public int turnLeftControl = 0;
    public int turnRightControl = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();

    }

    void Move()
    {
        if (turningPoint == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && rotateLeftMax == 0)
            {
                if (turnLeftControl == 1)
                {
                    rotateLeftMax = 0;
                    turnLeftControl = 0;
                }
                StartCoroutine(LeftSlide());
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && rotateRightMax == 0)
            {
                if (turnRightControl == 1)
                {
                    rotateRightMax = 0;
                    turnRightControl = 0;
                }
                StartCoroutine(RightSlide());
            }
            else
            {
                this.transform.Translate(Vector3.forward * runSpeed * Time.deltaTime * 5); // 앞으로 오브젝트가 이동함 runSpeed로 속도를 변경하면됨
            }
        }
        if (turningPoint == false)
        {
            this.transform.Translate(Vector3.forward * runSpeed * Time.deltaTime * 5); // 앞으로 오브젝트가 이동함 runSpeed로 속도를 변경하면됨
            if (Input.GetKeyDown(KeyCode.LeftArrow) && rotateLeftMax == 0)
            {
                if (turnLeftControl == 1)
                {
                    rotateLeftMax = 0;
                    turnLeftControl = 0;
                }
                StartCoroutine(LeftSlide());
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && rotateRightMax == 0)
            {
                if (turnRightControl == 1)
                {
                    rotateRightMax = 0;
                    turnRightControl = 0;
                }
                StartCoroutine(RightSlide());
            }
        }
    }

    IEnumerator LeftSlide()
    {
        while (Loop == true && rotateLeftMax < 15)
        {
            rotateY = +6.0f;
            positionX = +0.1f;
            if (turningPoint == true)
            {
                this.transform.Rotate(0.0f, -rotateY, 0.0f);
            }
            if (turningPoint == false)
            {
                this.transform.Translate(-positionX, 0.0f, 0.0f);
            }
            yield return new WaitForSeconds(0.0001f);
            rotateLeftMax++;
        }
        if (rotateLeftMax == 15)
        {
            rotateLeftMax = 0;
            turnLeftControl = 1;
        }
    }

    IEnumerator RightSlide()
    {
        while (Loop == true && rotateRightMax < 15)
        {
            rotateY = +6.0f;
            positionX = +0.1f;
            if (turningPoint == true)
            {
                this.transform.Rotate(0.0f, rotateY, 0.0f);
            }
            if (turningPoint == false)
            {
                this.transform.Translate(positionX, 0.0f, 0.0f);
            }
            yield return new WaitForSeconds(0.0001f);
            rotateRightMax++;
        }
        if (rotateRightMax == 15)
        {
            rotateRightMax = 0;
            turnRightControl = 1;
        }
    }

}
