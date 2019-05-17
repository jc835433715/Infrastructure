using Infrastructure.MessageBus.Interface;
using System.Collections.Generic;

namespace Infrastructure.MessageBus.Implement
{
    public class MessageBusImp : IMessageBus
    {
        public MessageBusImp(IMessageHandlerFactory factory)
        {
            this.factory = factory;
        }

        public TReply Send<TRequest, TReply>(TRequest message)
            where TRequest : IMessageRequest
            where TReply : IMessageReply
        {
            IMessageHandlerWithReply<TRequest, TReply> messageHandler = factory.Create<TRequest, TReply>();

            if (messageHandler == null)
            {
                throw new UnregisteredMessageException();
            }

            return messageHandler.Handle(message);
        }

        public void Send<TRequest>(TRequest message)
            where TRequest : IMessageRequest
        {
            IMessageHandlerWithNoReply<TRequest> messageHandler = factory.Create<TRequest>();

            if (messageHandler == null)
            {
                throw new UnregisteredMessageException();
            }

            messageHandler.Handle(message);
        }

        private IMessageHandlerFactory factory;
    }
}
