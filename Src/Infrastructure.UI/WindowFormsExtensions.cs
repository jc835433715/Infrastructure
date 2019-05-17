using System;
using System.Windows.Forms;

namespace Infrastructure.UI
{
    /// <summary>
    /// WindowForms扩展
    /// </summary>
    public static class WindowFormsExtensions
    {
        /// <summary>
        /// 在拥有控件的基础窗口句柄的线程上，用指定的参数列表执行指定委托
        /// </summary>
        /// <param name="form">Form</param>
        /// <param name="method">委托</param>
        /// <param name="args">参数</param>
        public static void InvokeEx(this Form form, Delegate method, params object[] args)
        {
            InvokeEx(form as Control, method, args);
        }

        /// <summary>
        /// 在拥有控件的基础窗口句柄的线程上，用指定的参数列表执行指定委托
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="method">委托</param>
        /// <param name="args">参数</param>
        public static void InvokeEx(this Control control, Delegate method, params object[] args)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(method, args);
            }
            else
            {
                method.DynamicInvoke(args);
            }
        }
    }
}
