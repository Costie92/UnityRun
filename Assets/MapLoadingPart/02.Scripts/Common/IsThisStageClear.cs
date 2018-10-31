using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace hcp {
    public class IsThisStageClear : MonoBehaviour {
        /***
         * 파싱은 1-Clear,2-notClear 이런식으로
         * 
         * /
         */
        public bool IsThisStageFirstPlay(E_STAGE stage)
        {
            if (!Directory.Exists(Constants.isThisStageClearDataPath))
            {
                return false;
            }
            if (! File.Exists(Constants.isThisStageClearDataPath + "/" + Constants.isStageClearFileName))
            {
                return false;
            }
            

            return IsRecordExists(ReadStageClearFile(),stage);
        }
        string ReadStageClearFile()
        {
            StringReader sr = new StringReader(Constants.isThisStageClearDataPath + "/" + Constants.isStageClearFileName);
            string fileContext = sr.ReadToEnd();
            sr.Close();
            return fileContext;
        }
        public bool IsRecordExists(string fileContext, E_STAGE stage)
        {
            string[] parsed = ParsingClearedData(fileContext);
            for (int i = 0; i < parsed.Length; i++)
            {
                if (int.Parse(parsed[i].Substring(0)) == (int)stage)
                {
                    return true;
                }
            }
            return false;
        }



    }
}