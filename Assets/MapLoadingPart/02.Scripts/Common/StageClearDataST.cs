using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hcp
{
    public class StageClearDataST
    {
        //파싱룰 1-1-1234,2-0-23141 
        public E_STAGE stage;
        public bool clear;
        public int coins;

        public static string GetForSavingParsingString(E_STAGE stageNum, bool isCleared, int coins)
        {
            string parse = "-";
            string saveParse = "";
            string stageStr = ((int)stageNum).ToString();
            string clearStr = (isCleared == true) ? "1" : "0";
            string coinStr = coins.ToString();

            saveParse = stageStr + parse + clearStr + parse + coinStr;
            return saveParse;
        }
        public static string GetForSavingParsingString(StageClearDataST st)
        {
            string parse = "-";
            string saveParse = "";
            string stageStr = ((int)st.stage).ToString();
            string clearStr = (st.clear == true) ? "1" : "0";
            string coinStr = st.coins.ToString();

            saveParse = stageStr + parse + clearStr + parse + coinStr;
            return saveParse;
        }

        public StageClearDataST(string primaryParsedData)
        {
            string[] a = primaryParsedData.Split('-');
            for (int i = 0; i < a.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        this.stage = (E_STAGE)(int.Parse(a[i]));
                        break;
                    case 1:
                        int isClear = int.Parse(a[i]);
                        if (isClear == 0)
                            this.clear = false;
                        else
                            this.clear = true;
                        break;
                    case 2:
                        int coins = int.Parse(a[i]);
                        this.coins = coins;
                        break;
                }
            }
        }
    }
}