using System;

namespace Infrastructure.MessageBus.Interface
{
	/// <summary>
	/// 未注册消息异常
	/// </summary>
	[Serializable]
	public class UnregisteredMessageException : ApplicationException
	{
	}
}
