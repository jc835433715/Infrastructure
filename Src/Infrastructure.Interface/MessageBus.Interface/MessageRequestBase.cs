using System;

namespace Infrastructure.MessageBus.Interface
{
    /// <summary>
    /// 消息请求抽象类
    /// </summary>
    [Serializable]
    public abstract class MessageRequestBase : IMessageRequest
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public string Id
        {
            get;
            set;
        }
    }
}
