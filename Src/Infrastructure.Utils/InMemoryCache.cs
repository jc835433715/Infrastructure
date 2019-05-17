using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Utils
{
    /// <summary>
    /// 内存缓存
    /// </summary>
    public static class InMemoryCache
    {
        private class Cache
        {
            public string Key { get; set; }

            public object Value { get; set; }

            public DateTime ExpirationDateTime { get; set; }
        }


        static InMemoryCache()
        {
            caches = new Dictionary<string, Cache>();

            ThreadHelper.StartThread(delegate ()
            {
                while (true)
                {
                    var query = from cache in caches
                                where DateTime.Now > cache.Value.ExpirationDateTime
                                select cache.Key;

                    lock (caches)
                    {
                        query.ToList().ForEach(e => caches.Remove(e));
                    }

                    ThreadHelper.Sleep(100);
                }
            });
        }

        /// <summary>
        /// 获取或添加
        /// </summary>
        /// <typeparam name="TValue">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="valueFactory">值工厂</param>
        /// <param name="timeSpan">缓存时间</param>
        /// <returns>值</returns>
        public static TValue GetOrAdd<TValue>(string key, Func<string, TValue> valueFactory, TimeSpan? timeSpan = null)
        {
            Cache result;

            lock (caches)
            {
                if (!caches.TryGetValue(key, out result))
                {
                    caches[key] = new Cache()
                    {
                        Key = key,
                        Value = valueFactory(key),
                        ExpirationDateTime = timeSpan == null ? DateTime.MaxValue : DateTime.Now.Add(timeSpan.Value)
                    };
                }

                result = caches[key];
            }

            return (TValue)result.Value;
        }

        private static Dictionary<string, Cache> caches;
    }
}
