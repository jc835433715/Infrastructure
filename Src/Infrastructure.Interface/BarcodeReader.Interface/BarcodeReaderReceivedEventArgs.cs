using System;

namespace Infrastructure.BarcodeReader.Interface
{
    /// <summary>
    /// 条码接收事件参数
    /// </summary>
    public class BarcodeReaderReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public BarcodeReaderReceivedEventArgs()
        {
            this.Barcode = BarcodeConst.EmptyBarcode;
            this.ConfirmResult = ConfirmResult.OK;
            this.ConfirmMessage = string.Empty;
        }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 条码确认结果
        /// </summary>
        public ConfirmResult ConfirmResult { get; set; }

        /// <summary>
        /// 确认信息
        /// </summary>
        public string ConfirmMessage { get; set; }
    }

    /// <summary>
    /// 条码确认结果
    /// </summary>
    public enum ConfirmResult
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// OK
        /// </summary>
        OK = 1,
        /// <summary>
        /// NG
        /// </summary>
        NG = 2
    }
}