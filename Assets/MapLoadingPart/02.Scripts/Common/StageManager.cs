using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp {
    public enum E_STAGE{
        STAGE_1=0,
        STAGE_2,

        INFINITY,
    }
    public class StageManager : SingletonTemplate<StageManager> {
        E_STAGE stageNum;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
            //저 스테이지 넘버로 맵 로딩하는 걸 분기
            //
        }
    }
}