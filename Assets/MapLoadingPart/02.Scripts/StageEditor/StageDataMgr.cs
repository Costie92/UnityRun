using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

using System;
using System.IO;
using hcp;

public class StageDataMgr  : SingletonTemplate<StageDataMgr>{

    public string editStageDataPath;

    DirectoryInfo myDif;
    protected override void Awake()
    {
        base.Awake();

        editStageDataPath = Constants.editStageDataPath;
        myDif = new DirectoryInfo(editStageDataPath);
    }

    public List<StageEditorST> CheckTheStage(string fileName)
    {
        if (!myDif.Exists)  myDif.Create();

        string filePath = editStageDataPath + "/" + fileName;
        FileStream fs;
        if (File.Exists(filePath))    //파일이 있으면 수정모드.
        {
            fs = File.Open(filePath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            List<StageEditorST> list = bf.Deserialize(fs) as List<StageEditorST>;
            fs.Close();
            return list;
        }

        else
        {   //파일이 없음
            return new List<StageEditorST>();
        }
    }



    public void SaveData( List<StageEditorST> list ,string stageName)//확장자까지 이름에 넣기.
    {
        Debug.Log(editStageDataPath);
        if (!myDif.Exists)
        {
            myDif.Create();
        }
       
        FileStream fs = File.Create(editStageDataPath +"/" +stageName);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, list);

        fs.Close();
    }

    public List<StageEditorST> LoadData(string stageName)
    {
        FileStream fs;
        if (File.Exists(editStageDataPath + "/" + stageName))
        {
            //바이너리 직렬화는 끝났고
            //xml 포맷으로 만들어서 네트워크로 쏘는 거 생각해보기.
            fs = File.Open(editStageDataPath + "/" + stageName, FileMode.Open);
        }
        else
        {
            ErrorManager.SpurtError("파일이 없어");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        List<StageEditorST> list =  bf.Deserialize(fs) as List<StageEditorST>;

        foreach (var n in list)
        {
            Debug.Log("포지션은"+n.pos +" 회전타입은"+ n.whichTurn+" 오브젝트 타입은"+n.soa.spawnObjType[0] + n.soa.spawnObjType[1]+ n.soa.spawnObjType[2]);
        }

        fs.Close();
        return list;
    }

    public static bool IsFileExist(string fileName)
    {
        if (! Directory.Exists(Constants.editStageDataPath)) return false;
        if (!File.Exists(Constants.editStageDataPath + "/" + fileName)) return false;
        return true;
    }
}
