using Infrastructure.Ioc.Interface;
using Infrastructure.MessageBus.Implement;
using Infrastructure.MessageBus.Interface;
using System.Linq;

namespace Infrastructure.MessageBus.Imp
{
    public class IocMessageHandlerFactory : IMessageHandlerFactory
    {
        public IocMessageHandlerFactory(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        public IMessageHandlerWithReply<TRequest, TReply> Create<TRequest, TReply>()
            where TRequest : IMessageRequest
            where TReply : IMessageReply
        {
            IMessageHandlerWithReply<TRequest, TReply> result = null;

            try
            {
                result = dependencyResolver.GetServices<IMessageHandlerWithReply<TRequest, TReply>>().FirstOrDefault();
            }
            catch { }

            return result;
        }

        public IMessageHandlerWithNoReply<TRequest> Create<TRequest>() where TRequest : IMessageRequest
        {
            IMessageHandlerWithNoReply<TRequest> result = null;

            try
            {
                result = dependencyResolver.GetServices<IMessageHandlerWithNoReply<TRequest>>().FirstOrDefault();
            }
            catch { }

            return result;
        }

        private readonly IDependencyResolver dependencyResolver;
    }
}
