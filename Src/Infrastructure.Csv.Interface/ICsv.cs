using System.Collections.Generic;
using System.Collections;

namespace Infrastructure.Csv.Interface
{
    /// <summary>
    /// Csv文件读写接口
    /// </summary>
    public interface ICsv
    {
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="head">抬头</param>
        /// <param name="records">记录</param>
        void Write(string path, IEnumerable<string> head, IEnumerable<string> records);

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>记录</returns>
        IEnumerable<string[]> Read(string path);
    }


    /// <summary>
    /// Csv文件读写接口
    /// </summary>
    /// <typeparam name="TRecord">记录</typeparam>
    public interface ICsv<TRecord>
    {
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="records">记录</param>
        /// <param name="hasHeaderRecord">是否有列头</param>
        void Write(string path, IEnumerable<TRecord> records, bool hasHeaderRecord = true);

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>记录</returns>
        IEnumerable<TRecord> Read(string path);
    }
}
