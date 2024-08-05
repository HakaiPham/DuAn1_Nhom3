using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData
{
    // Start is called before the first frame update
    public int coin;
}
public class DataManager
{
    const string fileName = "data.txt";
    public static bool SAVEDATA(GameData data)
    {
        try
        {
            var json = JsonUtility.ToJson(data);
            var fileSteam = new FileStream(fileName, FileMode.Create);
            using (var writeFile = new StreamWriter(fileSteam))
            {
                writeFile.WriteLine(json);
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.Log($"Save Data error: {e.Message}");
        }
        return false;
    }
    public static GameData ReadData()
    {
        try 
        {
            if (File.Exists(fileName))
            {
                //OPEN FILE
                var fileStream = new FileStream(fileName, FileMode.Open);
                //ĐỌC File
                using ( var readFile = new StreamReader(fileStream))
                {
                    //Read Data to End
                    var json = readFile.ReadToEnd();
                    //Chuyển dữ liệu từ Json sang class
                    var data = JsonUtility.FromJson<GameData>(json);
                    return data;
                }
            }
        }
        catch (System.Exception e) 
        {
            Debug.Log("Error Loading File: "+e.Message);
        }
        return null;
    }
}
