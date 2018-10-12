using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    //모델링 담당하는 자식의 콜라이더로 충돌 판정 할 때 처리단인 부모에서 호출할 함수
    public interface IChildModel
    {
        void FromChildOnCollisionEnter(GameObject child, Collision coll);
        void FromChildOnTriggerEnter(GameObject child, Collider other);
    };
};