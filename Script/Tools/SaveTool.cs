using System.IO;
using System.Text;
using UnityEngine;

namespace FPS_Manager
{
    public static class SaveTool
    {
        private const string SAVE_FILE_NAME = "save.dat";
        // private static string saveFilePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

        //保存数据
        //int coin,int maxScore,int currentCharacter, int[] characterLock
        public static void SaveData<T>(T dataToSave)
        {
            string saveFilePath = Path.Combine(Application.persistentDataPath, typeof(T).Name);

            string json = JsonUtility.ToJson(dataToSave);

            //如果没有，就创建文件目录，保证存储路径存在
            Directory.CreateDirectory(Path.GetDirectoryName(saveFilePath));
            //using 省去手动调用 Dispose 方法和编写 try-finally 语句
            using (StreamWriter writer = new StreamWriter(saveFilePath, false, Encoding.UTF8))//目标文件，覆盖，字符编码格式
            {
                writer.WriteLine(json);
            }
        }
        public static T LoadData<T>()
        {
            string saveFilePath = Path.Combine(Application.persistentDataPath, typeof(T).Name);
            if (!File.Exists(saveFilePath))//判断指定文件是否存在
            {
                Debug.Log($"没有指定文件:{typeof(T).Name}");
                return default(T);
            }
            using (StreamReader reader = new StreamReader(saveFilePath, Encoding.UTF8))
            {
                string json = reader.ReadToEnd();
                T loadedData = JsonUtility.FromJson<T>(json);
                return loadedData;
            }
        }
        // 重置数据
        /* public static void ResetData()
         {
             if (File.Exists(saveFilePath))
             {
                 File.Delete(saveFilePath);
             }
         }*/
    }

}