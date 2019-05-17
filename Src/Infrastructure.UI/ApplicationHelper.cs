using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Ms = System.Windows.Forms;

namespace Infrastructure.UI
{
    /// <summary>
    /// 应用帮助者
    /// </summary>
    public static class ApplicationHelper
    {
        ///<summary>
        /// 该函数设置由不同线程产生的窗口的显示状态
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        /// <summary>
        ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
        ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。 
        /// </summary>
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int SW_SHOWNOMAL = 1;

        /// <summary>
        /// 应用是否在运行中
        /// </summary>
        public static bool IsRunning
        {
            get
            {
                return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1;
            }
        }

        /// <summary>
        /// 显示已运行程序主窗体
        /// </summary>
        public static void ShowWindow()
        {
            var instance = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).FirstOrDefault();

            if (instance != null)
            {
                ShowWindowAsync(instance.MainWindowHandle, SW_SHOWNOMAL);//显示
                SetForegroundWindow(instance.MainWindowHandle);//当到最前端
            }
        }

        /// <summary>
        /// 运行应用
        /// </summary>
        /// <param name="tryAction"></param>
        /// <param name="finallyAction"></param>
        /// <param name="logAction"></param>
        public static void Run(Action tryAction, Action finallyAction, Action<Exception> logAction)
        {
            if (IsRunning)
            {
                ShowWindow();

                return;
            }

            System.Windows.Application app = new System.Windows.Application();

            app.DispatcherUnhandledException += (sender, e) => Write(e.Exception, logAction);
            Ms.Application.SetUnhandledExceptionMode(Ms.UnhandledExceptionMode.CatchException);
            Ms.Application.ThreadException += (sender, e) => Write(e.Exception, logAction);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => Write(e.ExceptionObject as Exception, logAction);

            Ms.Application.EnableVisualStyles();
            Ms.Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                tryAction();
            }
            catch (Exception e)
            {
                Write(e, logAction);
            }
            finally
            {
                finallyAction();
            }
        }

        private static void Write(Exception e, Action<Exception> logAction)
        {
            MessageBoxWrapper.ShowDialog(null, $"启动错误，错误信息：“{e}”，请排查！", MessageCaption.Warning, Ms.MessageBoxButtons.OK);

            logAction(e);
        }
    }
}
