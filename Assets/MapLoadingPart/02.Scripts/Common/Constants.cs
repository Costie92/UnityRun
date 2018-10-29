using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public static class Constants
    {
        public const int wantToShowNumOfChunks = 10;
        public const int wantToShowNumOfChunksInBehind = 1;
        public const int frontShowChunks = wantToShowNumOfChunks - wantToShowNumOfChunksInBehind;
        public const int turningTerm = 2 + frontShowChunks + 4 + 1;
        //턴청크 생성지점과 터닝포인트 차이 2(턴청크 종속적)
        //+ 프론트 청크 수
        //+ 전 턴청크 삭제 텀이 4 (턴청크 종속적 3 + 1(어색하지 않게))
        //삭제 후 다시 받아올때 +1   =16

        public const int firstObjSpawn = 2;

        public const string stageSelectSceneName = "StageSelect";
    }
}