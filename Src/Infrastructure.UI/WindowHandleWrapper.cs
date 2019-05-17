using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Infrastructure.UI
{
    /// <summary>
    /// 窗口句柄包装器
    /// </summary>
    public class WindowHandleWrapper : IWin32Window
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="handle">句柄</param>
        protected WindowHandleWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        /// <summary>
        /// 句柄
        /// </summary>
        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        /// <summary>
        /// 获取主窗体句柄
        /// </summary>
        /// <returns></returns>
        public static IWin32Window GetMainWindowHandle()
        {
            IWin32Window ower = null;

            try
            { ower = new WindowHandleWrapper(Process.GetCurrentProcess().MainWindowHandle); }
            catch
            { ower = null; }

            return ower;
        }

        private IntPtr _hwnd;
    }
}
