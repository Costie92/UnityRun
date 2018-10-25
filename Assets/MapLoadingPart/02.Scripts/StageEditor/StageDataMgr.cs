using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

using System;
using System.IO;
using hcp;
public class StageDataMgr {
    public static string dataPath = Application.persistentDataPath + "/StageData";

    DirectoryInfo myDif = new DirectoryInfo(dataPath);

    public void SaveData( List<StageEditorST> list )
    {
        Debug.Log(dataPath);
        if (!myDif.Exists)
        {
            myDif = new DirectoryInfo(dataPath);
        }
        myDif.Create();
        
        FileStream fs = File.Create(dataPath+"/1qwe.dat");
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, list);
        //StreamWriter sw = new StreamWriter(fs);

        // sw.Write(list[0]);

        // sw.Close();
        fs.Close();
    }
    public void LoadData()
    {
        //바이너리 직렬화는 끝났고
        //xml 포맷으로 만들어서 네트워크로 쏘는 거 생각해보기.
        FileStream fs = File.Open(dataPath + "/1qwe.dat",FileMode.Open);

        BinaryFormatter bf = new BinaryFormatter();
        List<StageEditorST> list = 
        bf.Deserialize(fs) as List<StageEditorST>;

        foreach (var n in list)
        {
            Debug.Log("포지션은"+n.pos +" 회전타입은"+ n.whichTurn+" 오브젝트 타입은"+n.soa.spawnObjType[0] + n.soa.spawnObjType[1]+ n.soa.spawnObjType[2]);
        }
        fs.Close();
    }
}
