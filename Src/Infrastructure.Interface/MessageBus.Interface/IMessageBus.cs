using System.Collections.Generic;

namespace Infrastructure.MessageBus.Interface
{
    /// <summary>
    /// 消息总线接口
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// 发送请求消息
        /// </summary>
        /// <typeparam name="TRequest">请求消息类型</typeparam>
        /// <typeparam name="TReply">回复消息类型</typeparam>
        /// <param name="message">请求消息对象</param>
        /// <returns>回复消息对象</returns>
        TReply Send<TRequest, TReply>(TRequest message)
            where TRequest : IMessageRequest
            where TReply : IMessageReply;

        /// <summary>
        /// 发送请求消息
        /// </summary>
        /// <typeparam name="TRequest">请求消息类型</typeparam>
        /// <param name="message">请求消息对象</param>
        void Send<TRequest>(TRequest message)
            where TRequest : IMessageRequest;
    }
}
