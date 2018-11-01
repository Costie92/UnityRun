using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace hcp {
    public class IsThisStageClear{

        static IsThisStageClear instance=null;

        IsThisStageClear() { }

        public static IsThisStageClear GetInstance()
        {
            if (instance == null)
            {
                instance = new IsThisStageClear();
            }

            return instance;
        }

        /*** 스테이지 클리어 여부 코인(임시로)
         * 파싱은 1-Clear,2-notClear 이런식으로
         * 
         * /
         */
        public void SaveClearData(E_STAGE stage, bool isCleared, int coins)
        {
            bool firstTime = false;
            string stageSaveString = StageClearDataST.GetForSavingParsingString(stage, isCleared, coins);
            if (!Directory.Exists(Constants.isThisStageClearDataPath))
            {
                Directory.CreateDirectory(Constants.isThisStageClearDataPath);
                firstTime = true;
            }
            if (!File.Exists(Constants.isThisStageClearDataPath + "/" + Constants.isStageClearFileName))
            {
                File.Create(Constants.isThisStageClearDataPath + "/" + Constants.isStageClearFileName).Dispose();//dispose해줘야함.
                firstTime = true;
            }
            if (firstTime)
            {
                StreamWriter sw1 = new StreamWriter(Constants.isThisStageClearDataPath + "/" + Constants.isStageClearFileName);
                sw1.Write(stageSaveString+",");
                sw1.Close();
                return;
            }


            List<StageClearDataST> sclist = MakeListFromClearDataFile();

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
            FileStream fs = new FileStream(Constants.isThisStageClearDataPath + "/" + Constants.isStageClearFileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
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


            List<StageClearDataST> sclist = MakeListFromClearDataFile();

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
        public bool IsRecordExists(string fileContext, E_STAGE stage)
        {
            List<StageClearDataST> sclist =MakeListFromClearDataFile();

            for (int i = 0; i < sclist.Count; i++)
            {
                if (sclist[i].stage == stage)
                    return true;
            }
            
            return false;
        }

        List<StageClearDataST> MakeListFromClearDataFile()
        {
            string[] priparse = ParsingClearedDataPrimary(ReadStageClearFile());
            List<StageClearDataST> sclist = new List<StageClearDataST>();
            for (int i = 0; i < priparse.Length; i++)
            {
                if (priparse[i] == "") continue;
                StageClearDataST sc = new StageClearDataST(priparse[i]);
                sclist.Add(sc);
            }
            return sclist;
        }

        string ReadStageClearFile()
        {
            StreamReader sr = new StreamReader(Constants.isThisStageClearDataPath + "/" + Constants.isStageClearFileName);
            string fileContext = sr.ReadToEnd();
            sr.Close();
            return fileContext;
        }
       

        string[] ParsingClearedDataPrimary(string fileContexts)
        {
            return fileContexts.Split(',');
        }

    }
}