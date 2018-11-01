using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace hcp {
    public class IsThisStageClear : MonoBehaviour {
        public class StageClearDataST
        {
            public E_STAGE stage;
            public bool clear;
            public int coins;

            public static string GetForSavingParsingString(E_STAGE stageNum, bool isCleared, int coins)
            {
                string parse = "-";
                string saveParse="";
                string stageStr = ((int)stageNum).ToString();
                string clearStr = (isCleared==true)?"1":"0";
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
                            this.stage = (E_STAGE)int.Parse( a[i]);
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

        /*** 스테이지 클리어 여부 코인(임시로)
         * 파싱은 1-Clear,2-notClear 이런식으로
         * 
         * /
         */
        public void SaveClearData(E_STAGE stage, bool isCleared, int coins)
        {
            bool firstTime = false;
            FileStream fs;
            StreamWriter sw;
            string stageSaveString = StageClearDataST.GetForSavingParsingString(stage, isCleared, coins);
            if (!Directory.Exists(Constants.isThisStageClearDataPath))
            {
                Directory.CreateDirectory(Constants.isThisStageClearDataPath);
                firstTime = true;
            }
            if (!File.Exists(Constants.isThisStageClearDataPath + "/" + Constants.isStageClearFileName))
            {
                File.Create(Constants.isThisStageClearDataPath + "/" + Constants.isStageClearFileName);
                firstTime = true;
            }
            if (firstTime)
            {
                fs = new FileStream(Constants.isThisStageClearDataPath + "/" + Constants.isStageClearFileName, FileMode.OpenOrCreate);
                sw = new StreamWriter(fs);
                sw.Write(stageSaveString);
                sw.Close();
                fs.Close();
                return;
            }
            

            string[] priparse=
            ParsingClearedDataPrimary(ReadStageClearFile());

            List<StageClearDataST> sclist = new List<StageClearDataST>();
            for (int i = 0; i < priparse.Length; i++)
            {
                StageClearDataST sc = new StageClearDataST(priparse[i]);
                sclist.Add(sc);
            }
            string lastSaving="";
            for (int i = 0; i < sclist.Count; i++)
            {
                if (sclist[i].stage == stage)
                {
                    sclist.RemoveAt(i);
                }
            }
            for (int i = 0; i < sclist.Count; i++)
            {
                lastSaving += StageClearDataST.GetForSavingParsingString(sclist[i])+",";
            }
            lastSaving += stageSaveString;

            fs = new FileStream(Constants.isThisStageClearDataPath + "/" + Constants.isStageClearFileName, FileMode.Create);
            sw = new StreamWriter(fs);
            sw.Write(lastSaving);
            sw.Close();
            fs.Close();
            return;

        }


        public StageClearDataST GetClearDataOfThisStage(E_STAGE stage)
        {
            if (!Directory.Exists(Constants.isThisStageClearDataPath))
            {
                return null;
            }
            if (!File.Exists(Constants.isThisStageClearDataPath + "/" + Constants.isStageClearFileName))
            {
                return null;
            }

            string fileContext = ReadStageClearFile();
            string[] priparse = ParsingClearedDataPrimary(fileContext);
            List<StageClearDataST> sclist = new List<StageClearDataST>();
            for (int i = 0; i < priparse.Length; i++)
            {
                StageClearDataST sc = new StageClearDataST(priparse[i]);
                sclist.Add(sc);
            }

            for (int i = 0; i < sclist.Count; i++)
            {
                if (sclist[i].stage == stage)
                    return sclist[i];
            }

            return null;
        }

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
            string[] priparse = ParsingClearedDataPrimary(fileContext);
            List<StageClearDataST> sclist = new List<StageClearDataST>();
            for (int i = 0; i < priparse.Length; i++)
            {
                StageClearDataST sc = new StageClearDataST(priparse[i]);
                sclist.Add(sc);
            }

            for (int i = 0; i < sclist.Count; i++)
            {
                if (sclist[i].stage == stage)
                    return true;
            }
            
            return false;
        }
        string[] ParsingClearedDataPrimary(string fileContexts)
        {
            return fileContexts.Split(',');
        }

    }
}