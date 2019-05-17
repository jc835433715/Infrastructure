using System.Collections.Generic;
using System.Windows.Forms;

namespace Infrastructure.UI
{
    /// <summary>
    /// 消息框包装器
    /// </summary>
    public static class MessageBoxWrapper
    {
        static MessageBoxWrapper()
        {
            messageCaptions = new Dictionary<MessageCaption, string>
            {
                [MessageCaption.Information] = "提示",
                [MessageCaption.Question] = "确认",
                [MessageCaption.Warning] = "警告"
            };
        }

        /// <summary>
        /// 模态显示消息框
        /// </summary>
        /// <param name="owner">父窗体</param>
        /// <param name="messageBoxText">消息框文本</param>
        /// <param name="messageCaption">消息框标题</param>
        /// <param name="buttons">按钮</param>
        /// <returns></returns>
        public static DialogResult ShowDialog(IWin32Window owner, string messageBoxText, MessageCaption messageCaption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(owner, messageBoxText, messageCaptions[messageCaption], buttons);
        }

        private static readonly Dictionary<MessageCaption, string> messageCaptions;
    }

    /// <summary>
    /// 消息标题
    /// </summary>
    public enum MessageCaption
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 信息
        /// </summary>
        Information,
        /// <summary>
        /// 确认
        /// </summary>
        Question,
        /// <summary>
        /// 警告
        /// </summary>
        Warning
    }
}
