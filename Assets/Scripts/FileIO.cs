using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class FileIO
{
    public static void WriteString(string fileName,string text)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        StreamWriter writer = new(path, false);
        writer.Write(text);
        writer.Close();
        StreamReader reader = new(path);
        reader.Close();
    }
    public static string ReadString(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        StreamReader reader;
        try
        {
            reader = new(path);
        }
        catch (FileNotFoundException)
        {
            return "0";
        }
        string text = reader.ReadToEnd();
        reader.Close();
        return text;
    }
    public static int ReadInt(string fileName)
    {
        return int.Parse(ReadString(fileName));
    }
}
