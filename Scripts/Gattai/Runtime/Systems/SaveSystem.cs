using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Gattai.Runtime.Systems
{
    public static class SaveSystem
    {
        private const bool UseBase64Encoding = false;
        private static GameData _current;
        public static GameData Current => _current ??= Load(0);
        private static readonly string SavesDirectory = Application.persistentDataPath + "/saves";
        private static bool _loaded;

        private static void CreateNewGameData(int saveDataId)
        {
            _current = new GameData();
            Save(saveDataId);
        }

        public static void Save(int saveDataId)
        {
            CheckSavesDirectory();
            
            var json = JsonUtility.ToJson(Current, true);

            var path = GetPath(saveDataId);

            using var streamWriter = new StreamWriter(path);
            
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (UseBase64Encoding)
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(json);
                var b64 = Convert.ToBase64String(plainTextBytes);
                streamWriter.Write(b64);
            }
            else
            {
                streamWriter.Write(json);
            }
        }
        
        public static GameData Load(int saveDataId)
        {
            if (_loaded && _current != null) return Current;
            
            CheckSavesDirectory();
            
            var path = GetPath(saveDataId);

            if (!File.Exists(path))
            {
                CreateNewGameData(saveDataId);

                return _current;
            }
            
            string json = null;
            
            using var streamReader = new StreamReader(path);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (UseBase64Encoding)
            {
                try
                {
                    var b64 = streamReader.ReadToEnd();
                    var plainTextBytes = Convert.FromBase64String(b64);
                    json = Encoding.UTF8.GetString(plainTextBytes);
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
            else
            {
                try
                {
                    json = streamReader.ReadToEnd();
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            _loaded = true;
            
            return JsonUtility.FromJson<GameData>(json);
        }

        private static void CheckSavesDirectory()
        {
            if (Directory.Exists(SavesDirectory)) return;
            
            Directory.CreateDirectory(SavesDirectory);
            Load(0);
        }

        private static string GetPath(int saveDataId)
        {
            return $"{SavesDirectory}/save{saveDataId}.json";
        }

        public static void ResetProgress()
        {
            var path = GetPath(0);

            if (!File.Exists(path)) return;
            
            File.Delete(path);
            
            _current = new GameData();
            
            Save(0);
        }
    }
}