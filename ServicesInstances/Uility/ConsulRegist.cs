using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesInstances.Uility
{
    public static class ConsulRegist
    {
        public static void ConsulExtend(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(m =>
            {
                m.Address = new Uri("http://localhost:8500/");
                m.Datacenter = "dc1";
            });
            //启动的时候在consul中注册实例服务
            //在consul中注册的ip,port
            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);
            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "service" + Guid.NewGuid(),//唯一的
                Name = "MicroserviceAttempt",//组名称
                Address = ip,
                Port = port,//不同的端口=>不同的实例
                Tags = new string[] { weight.ToString() },//标签
                Check = new AgentServiceCheck()//服务健康检查
                {
                    Interval = TimeSpan.FromSeconds(12),//间隔12s一次 检查
                    HTTP = $"http://{ip}:{port}/Api/Health/Index",
                    Timeout = TimeSpan.FromSeconds(5),//检测等待时间
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20)//失败后多久移除
                }
            });
            Console.WriteLine($"{ip}:{port}--weight:{weight}");
        }
    }
}
