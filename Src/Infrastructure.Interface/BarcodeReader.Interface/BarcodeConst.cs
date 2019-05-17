using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BarcodeReader.Interface
{
    /// <summary>
    /// 条码常量
    /// </summary>
    public static class BarcodeConst
    {
        /// <summary>
        /// 错误条码
        /// </summary>
        public readonly static string ErrorBarcode = "ERROR";

        /// <summary>
        /// 空条码
        /// </summary>
        public readonly static string EmptyBarcode = "N/A";
    }
}
