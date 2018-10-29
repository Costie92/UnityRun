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

        public const string editStage_1_fileName = "editStage_1.dat";
        public const string editStage_2_fileName = "editStage_2.dat";
        public const string editStage_3_fileName = "editStage_3.dat";
        public const string editStage_4_fileName = "editStage_4.dat";
        public const string editStage_5_fileName = "editStage_5.dat";
        public const string editStage_6_fileName = "editStage_6.dat";
        public const string editStage_7_fileName = "editStage_7.dat";
        public const string editStage_8_fileName = "editStage_8.dat";
        public const string editStage_9_fileName = "editStage_9.dat";
        public const string editStage_10_fileName = "editStage_10.dat";
        public const string editStage_11_fileName = "editStage_11.dat";
        public const string editStage_12_fileName = "editStage_12.dat";
        public const string editStage_13_fileName = "editStage_13.dat";
        public const string editStage_14_fileName = "editStage_14.dat";
        public const string editStage_15_fileName = "editStage_15.dat";
        public const string editStage_16_fileName = "editStage_16.dat";

    }
}