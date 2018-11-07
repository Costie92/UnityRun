using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnLeftCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    private bool isBtnLeftDown = false;
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
            if (isBtnLeftDown && CanMove)
            {
                cMove.turningPoint = false;
                cMove.Move(true);
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isBtnLeftDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isBtnLeftDown = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            isBtnLeftDown = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            isBtnLeftDown = false;
        }
    }
}