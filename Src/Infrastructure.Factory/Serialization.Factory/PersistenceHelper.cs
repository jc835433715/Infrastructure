using Infrastructure. Utils;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Infrastructure.Serialization.Factory
{
    /// <summary>
    /// 持久化帮助者
    /// </summary>
    public static class PersistenceHelper
    {
        /// <summary>
        /// 保存,相关类，需加 [Serializable]特性
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="value">持久化对象</param>
        public static void SaveBin(string filePath, object value)
        {
            FileSystemHelper.CreateDirectory(filePath);

            using (var file = File.Open(filePath, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();

                file.Seek(0, SeekOrigin.Begin);

                bf.Serialize(file, value);

                file.Flush();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="value">持久化对象</param>
        public static void SaveJson(string filePath, object value)
        {
            FileSystemHelper.CreateDirectory(filePath);

            File.WriteAllText(filePath, ConverterManager.GetConverter().Serialize(value));
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回对象</returns>
        public static T RecoveryBin<T>(string filePath)
            where T : new()
        {
            var result = new T();

            if (FileSystemHelper.IsFileExists(filePath))
            {
                using (var file = File.OpenRead(filePath))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    var value = bf.Deserialize(file);

                    result = (T)Convert.ChangeType(value, typeof(T));
                }
            }

            return result;
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回对象</returns>
        public static T RecoveryJson<T>(string filePath)
            where T : new()
        {
            var result = new T();

            if (FileSystemHelper.IsFileExists(filePath))
            {
                string value = File.ReadAllText(filePath);

                result = ConverterManager.GetConverter().Deserialize<T>(value);
            }

            return result;
        }
    }
}
