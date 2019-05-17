using System;

namespace Infrastructure.MessageBus.Interface
{
	/// <summary>
	/// 消息回复抽象类
	/// </summary>
	[Serializable]
	public abstract class MessageReplyBase : IMessageReply
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
