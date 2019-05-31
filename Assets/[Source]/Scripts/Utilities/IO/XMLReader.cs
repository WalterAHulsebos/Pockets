using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Utilities.IO
{
    public static class XMLReader
    {
        public static (T, bool) Load<T>(this string path) where T : class
        {
            if (!File.Exists(path))
            {
                Debug.LogError("No Save Data has been found!");
                return (null, false);
            }
            var serializer = new XmlSerializer(typeof(T));
            var stream = new FileStream(path, FileMode.Open);
            var ret = (T)serializer.Deserialize(stream);
            stream.Close();

            Debug.Log($"{typeof(T)} class loaded!");
            return (ret, true);
        }

        public static void Save<T>(this T saveData, List<string> path, string fileName) where T : class
        {
            Save(saveData, path.GenerateFilePath(fileName));
        }

        public static void Save<T>(this T saveData, string path) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            var stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, saveData);
            stream.Close();

            Debug.Log($"{nameof(saveData)} saved!");
        }

        public static string GenerateFilePath(this List<string> folderPath, string fileName)
        {
            var s = Path.DirectorySeparatorChar;
            string path;

            #if UNITY_ANDROID || UNITY_IOS
            path = Application.persistentDataPath;
            #endif

            #if UNITY_STANDALONE || UNITY_EDITOR
            path = Application.dataPath;
            #endif

            foreach (var f in folderPath)
                path += s + f;

            path += fileName + ".xml";
            return path;
        }
    }
}