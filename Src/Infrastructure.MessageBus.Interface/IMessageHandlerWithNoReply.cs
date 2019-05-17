namespace Infrastructure.MessageBus.Interface
{
	/// <summary>
	/// 无返回的消息处理者接口
	/// </summary>
	/// <typeparam name="TRequest">请求消息类型</typeparam>
	public interface IMessageHandlerWithNoReply<TRequest> where TRequest : IMessage
    {
		/// <summary>
		/// 处理请求消息
		/// </summary>
		/// <param name="message">请求消息对象</param>
		void Handle(TRequest message);
	}
}
