using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Systems.AppStorage
{
    public static class Storage
    {
        public static string AppDataPath => Application.persistentDataPath;

        public static bool SaveData<T>(string path, T data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var directory = Path.GetDirectoryName(path);
            Debug.Log(json);

            try
            {
                if (directory == null)
                    return false;
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            
                Debug.Log("Saving json to :" + path);
                File.WriteAllText(path, json);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        public static T LoadData<T>(string path, T defaultValue = default)
        {
            try
            {
                if (!File.Exists(path))
                    return defaultValue;
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return defaultValue;
            }
        }
    }
}