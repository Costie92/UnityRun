using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnRightCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    private bool isBtnRightDown = false;
    private CharacterMove cMove;
    private bool CanMove;
    // Use this for initialization
    void Start()
    {
        cMove = GameObject.FindWithTag("PLAYER").GetComponent<CharacterMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.isPause && !CharacterAnimation.Win)
        {
            CanMove = !ObjEat.unityChanDie && !ObjEat.HitInvincible;
            if (isBtnRightDown && CanMove)
            {

                cMove.turningPoint = false;
                cMove.Move(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isBtnRightDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isBtnRightDown = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            isBtnRightDown = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            isBtnRightDown = false;
        }
    }
}