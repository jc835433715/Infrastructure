using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Infrastructure.Utils
{
    /// <summary>
    /// 文件帮助者
    /// </summary>
    public static class FileSystemHelper
    {
        /// <summary>
        /// 获取子目录
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>返回子目录</returns>
        public static string[] GetDirectories(string directoryPath)
        {
            return Directory.Exists(directoryPath) ? Directory.GetDirectories(directoryPath) : new string[] { };
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fullPath">文件路径</param>
        /// <returns>存在，返回true</returns>
        public static bool IsFileExists(string fullPath)
        {
            return File.Exists(fullPath);
        }

        /// <summary>
        /// 判断文件目录是否存在
        /// </summary>
        /// <param name="fullPath">文件路径</param>
        /// <returns>存在，返回true</returns>
        public static bool IsDirectoryExists(string fullPath)
        {
            return Directory.Exists(Path.GetDirectoryName(fullPath));
        }

        /// <summary>
        /// 创建文件目录
        /// </summary>
        /// <param name="fullPath">文件路径</param>
        public static void CreateDirectory(string fullPath)
        {
            if (!IsDirectoryExists(fullPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }
        }

        /// <summary>
        ///创建目录
        /// </summary>
        /// <param name="directoryPath">目录</param>
        public static void CreateDirectoryIfDirectoryNotExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
