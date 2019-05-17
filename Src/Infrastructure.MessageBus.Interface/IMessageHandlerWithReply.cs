namespace Infrastructure.MessageBus.Interface
{
    /// <summary>
    /// 有返回的消息处理者接口
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TReply"></typeparam>
    public interface IMessageHandlerWithReply<TRequest, TReply>
        where TRequest : IMessageRequest
        where TReply : IMessageReply
    {
        /// <summary>
        /// 处理请求消息
        /// </summary>
        /// <param name="message">请求消息对象</param>
        /// <returns>回复消息对象</returns>
        TReply Handle(TRequest message);
    }
}
