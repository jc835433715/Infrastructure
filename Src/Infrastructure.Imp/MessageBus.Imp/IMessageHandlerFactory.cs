using Infrastructure.MessageBus.Interface;

namespace Infrastructure.MessageBus.Implement
{
    public interface IMessageHandlerFactory
    {
        IMessageHandlerWithReply<TRequest, TReply> Create<TRequest, TReply>()
            where TRequest : IMessageRequest
            where TReply : IMessageReply;

        IMessageHandlerWithNoReply<TRequest> Create<TRequest>() where TRequest : IMessageRequest;
    }
}