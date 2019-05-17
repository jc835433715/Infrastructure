using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Log.Interface
{
    /// <summary>
    /// 日志消息写入事件帮助者
    /// </summary>
    public static class MessageWritedEventHelper
    {
        /// <summary>
        /// 日志消息写入事件
        /// </summary>
        public static event EventHandler<MessageWritedEventArgs> MessageWrited;

        /// <summary>
        /// 触发日志消息写入事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">日志消息写入事件参数</param>
        public static void OnMessageWrited(object sender, EventArgs e)
        {
            if (MessageWrited != null)
            {
                foreach (var del in MessageWrited.GetInvocationList())
                {
                    try
                    {
                        del.DynamicInvoke(sender, e);
                    }
                    catch { }
                }
            }
        }
    }
}
