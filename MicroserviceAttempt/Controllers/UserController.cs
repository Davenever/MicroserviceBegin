using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using FrameWork;
using IServices;
using Microsoft.AspNetCore.Mvc;
using Model;
using Newtonsoft.Json;

namespace MicroserviceAttempt.Controllers
{
    public class UserController : Controller
    {
        readonly IUserInfo _userInfo;
        readonly HttpSender _httpSender;
        public UserController(IUserInfo userInfo,
            HttpSender httpSender)
        {
            _userInfo = userInfo;
            _httpSender = httpSender;
        }
        //暂不考虑线程安全
        private static int index = 0;
        public async Task<IActionResult> Index()
        {
            #region nginx版 只知道nginx地址就行了
            //var str = await _httpSender.InvokeApi("http://localhost:8088/api/User/GetCustomerUser");
            #endregion

            #region consul
            //new一个consul实例
            ConsulClient client = new ConsulClient(m =>
            {
                new Uri("http://localhost:8500/");
                m.Datacenter = "dc1";
            });
            //与consul进行通信(连接),得到consul中所有的服务实例
            var response = client.Agent.Services().Result.Response;
            string url = "http://MicroserviceAttempt/api/User/GetCustomerUser";
            Uri uri = new Uri(url);
            string groupName = uri.Host;
            AgentService agentService = null;//服务实例
            var serviceDictionary = response.Where(m => m.Value.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();//找到的全部服务实例
            //{
            //    agentService = serviceDictionary[0].Value;
            //}
            {
                //轮询策略=>达到负载均衡的目的
                agentService = serviceDictionary[index++ % 3].Value;
            }
            {
                //平均策略（随机获取索引--相对平均）=>达到负载均衡的目的
                agentService = serviceDictionary[new Random(index++).Next(0, serviceDictionary.Length)].Value;
            }
            {
                //权重策略,给不同的实例分配不同的压力,注册时提供权重
                List<KeyValuePair<string, AgentService>> keyValuePairs = new List<KeyValuePair<string, AgentService>>();
                foreach (var item in keyValuePairs)
                {
                    int count = int.Parse(item.Value.Tags?[0]);//在服务注册的时候给定权重数量
                    for (int i = 0; i < count; i++)
                    {
                        keyValuePairs.Add(item);
                    }
                }
                agentService = keyValuePairs.ToArray()[new Random(index++).Next(0, keyValuePairs.Count())].Value;
            }
            url = $"{uri.Scheme}://{agentService.Address}:{agentService.Port}{uri.PathAndQuery}";
            string content = await _httpSender.InvokeApi(url);
            #endregion
            return View(JsonConvert.DeserializeObject<CustomerUser>(content));
        }
    }
}