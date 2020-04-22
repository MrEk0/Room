using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataSaver
{
    private const string FILENAME = "player.data";

    public static void Save(string name, float stat1, float stat2, float stat3)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = GetFilePath();

        FileStream stream = new FileStream(path, FileMode.Create);
        Data data = new Data(name, stat1, stat2, stat3);

        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static Data Load()
    {
        string path = GetFilePath();

        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Data data = binaryFormatter.Deserialize(stream) as Data;
            return data;
        }
        else
        {
            return null;
        }
    }

    private static string GetFilePath()
    {
        string filePath = Path.Combine(Application.persistentDataPath, FILENAME);
        return filePath;
    }
}
