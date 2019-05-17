using Infrastructure.MessageBus.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.MessageBus.Implement
{
    public class MessageHandlerFactory : IMessageHandlerFactory
    {
        /// <summary>
        /// 构造
        /// </summary>
        ///<param name="messageHandlerAssemblyStrings">消息处理者程序集的长格式</param>
        public MessageHandlerFactory(IEnumerable<string> messageHandlerAssemblyStrings)
        {
            this.messageHandlerAssemblyList = GetMessageHandlerAssembly(messageHandlerAssemblyStrings);
        }

        public IMessageHandlerWithNoReply<TRequest> Create<TRequest>()
            where TRequest : IMessageRequest
        {
            IMessageHandlerWithNoReply<TRequest> result = null;
            var messageHandlerInfo = (from messageHandlerAssembly in messageHandlerAssemblyList
                                      from type in messageHandlerAssembly.GetTypes()
                                      where type.GetInterfaces().Any((Type e) => e.IsGenericType && e.GetGenericTypeDefinition() == typeof(IMessageHandlerWithNoReply<>)
                                      && e.GetGenericArguments().Any((Type x) => x == typeof(TRequest)))
                                      select new
                                      {
                                          Assembly = messageHandlerAssembly,
                                          Type = type
                                      }).FirstOrDefault();

            if (messageHandlerInfo != null)
            {
                result = (messageHandlerInfo.Assembly.CreateInstance(messageHandlerInfo.Type.FullName) as IMessageHandlerWithNoReply<TRequest>);
            }

            return result;
        }

        public IMessageHandlerWithReply<TRequest, TReply> Create<TRequest, TReply>()
            where TRequest : IMessageRequest
            where TReply : IMessageReply
        {
            IMessageHandlerWithReply<TRequest, TReply> result = null;
            var messageHandlerInfo = (from messageHandlerAssembly in messageHandlerAssemblyList
                                      from type in messageHandlerAssembly.GetTypes()
                                      where type.GetInterfaces().Any((Type e) => e.IsGenericType && e.GetGenericTypeDefinition() == typeof(IMessageHandlerWithReply<,>)
                                      && e.GetGenericArguments().Any((Type x) => x == typeof(TRequest)) && e.GetGenericArguments().Any((Type x) => x == typeof(TReply)))
                                      select new
                                      {
                                          Assembly = messageHandlerAssembly,
                                          Type = type
                                      }).FirstOrDefault();

            if (messageHandlerInfo != null)
            {
                result = (messageHandlerInfo.Assembly.CreateInstance(messageHandlerInfo.Type.FullName) as IMessageHandlerWithReply<TRequest, TReply>);
            }

            return result;
        }

        private List<Assembly> GetMessageHandlerAssembly(IEnumerable<string> messageHandlerAssemblyStrings)
        {
            List<Assembly> result = new List<Assembly>();

            foreach (string messageHandlerAssemblyString in messageHandlerAssemblyStrings)
            {
                Assembly messageHandlerAssembly = Assembly.Load(messageHandlerAssemblyString);

                result.Add(messageHandlerAssembly);
            }

            return result;
        }

        private List<Assembly> messageHandlerAssemblyList;
    }
}
