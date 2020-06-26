using Ocelot.Cache;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace GateWay_Ocelot
{
    /// <summary>
    /// 自定义Cache
    /// </summary>
    public class CustomeCache : IOcelotCache<CachedResponse>
    {
        public class CacheDataModel
        {
            public CachedResponse CachedResponse { get; set; }
            public DateTime TimeOut { get; set; }
            public string Region { get; set; }
        }

        private static Dictionary<string, CacheDataModel> keyValuePairs = new Dictionary<string, CacheDataModel>();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl"></param>
        /// <param name="region"></param>
        public void Add(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            Console.WriteLine($"调用了{nameof(CustomeCache)}--{nameof(Add)}");
            keyValuePairs[key] = new CacheDataModel()
            {
                CachedResponse = value,
                Region = region,
                TimeOut = DateTime.Now.Add(ttl)
            };
        }

        /// <summary>
        /// 覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl"></param>
        /// <param name="region"></param>
        public void AddAndDelete(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            Console.WriteLine($"调用了{nameof(CustomeCache)}--{nameof(AddAndDelete)}");
            keyValuePairs[key] = new CacheDataModel()
            {
                CachedResponse = value,
                Region = region,
                TimeOut = DateTime.Now.Add(ttl)
            };
        }

        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="region"></param>
        public void ClearRegion(string region)
        {
            Console.WriteLine($"调用了{nameof(CustomeCache)}--{nameof(ClearRegion)}");
            var keyList = keyValuePairs.Where(m => m.Value.Region.Equals(region)).Select(e => e.Key);
            foreach (var item in keyList)
            {
                keyValuePairs.Remove(item);
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public CachedResponse Get(string key, string region)
        {
            Console.WriteLine($"调用了{nameof(CustomeCache)}--{nameof(Get)}");
            if (keyValuePairs.ContainsKey(key) && keyValuePairs[key] != null && keyValuePairs[key].TimeOut > DateTime.Now && keyValuePairs[key].Region.Equals(region))
                return keyValuePairs[key].CachedResponse;
            else
                return null;
        }
    }
}
