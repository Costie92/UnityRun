using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace hcp
{
    public class ErrorManager {

        public static void SpurtError(string msg="")//나중에 메인화면으로 오류메시지 뿜으면서 이동하는 걸로 교체
        {
            Debug.Log(msg);
            Assert.IsTrue(true);
        }
    }
}