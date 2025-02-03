using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Save
{
    private const string extension = ".SE";
    private const string fileName = "dataSE";
    public static T LoadFile<T>(string filename = fileName) where T : class
    {
        T data = null;
        string fullpath = Application.persistentDataPath + "/" + filename + extension;
        if (IsValidFilename(filename) && File.Exists(fullpath))
        {
            FileStream file = null;
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Open(fullpath, FileMode.Open);
                data = (T)bf.Deserialize(file);
                file.Close();
            }
            catch (System.Exception e) { Debug.Log("Error Loading Data " + e); if (file != null) file.Close(); }
        }
        return data;
    }
    //Save any class to a file, make sure the class is marked with [System.Serializable]
    public static void SaveFile<T>(T data, string filename = fileName) where T : class
    {
        if (IsValidFilename(filename))
        {
            FileStream file = null;
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                string fullpath = Application.persistentDataPath + "/" + filename + extension;
                file = File.Create(fullpath);
                bf.Serialize(file, data);
                file.Close();
            }
            catch (System.Exception e) { Debug.Log("Error Saving Data " + e); if (file != null) file.Close(); }
        }
    }
#if UNITY_EDITOR
    [MenuItem("SpaceExploration/clear")]
#endif
    public static void DeleteFile()
    {
        string fullpath = Application.persistentDataPath + "/" + fileName + extension;
        if (File.Exists(fullpath))
            File.Delete(fullpath);
    }
    public static bool IsValidFilename(string filename = fileName)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return false; //Filename cant be blank

        if (filename.Contains("."))
            return false; //Dont allow dot as they are for extensions savefile

        foreach (char c in Path.GetInvalidFileNameChars())
        {
            if (filename.Contains(c.ToString()))
                return false; //Dont allow any special characters
        }
        return true;
    }
}
