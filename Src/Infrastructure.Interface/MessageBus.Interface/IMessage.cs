namespace Infrastructure.MessageBus.Interface
{
	/// <summary>
	/// 消息接口
	/// </summary>
	public interface IMessage
	{
		/// <summary>
		/// 消息Id
		/// </summary>
		string Id
		{
			get;
		}
	}
}
