using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationClient
{
    /// <summary>
    /// 自定义管理信息
    /// </summary>
    public class InitConfig
    {
        /// <summary>
        /// 定义ApiResource
        /// 这里的资源(Resource)指的就是管理的Api
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("UserApi","用户获取Api")
            };
        }

        /// <summary>
        /// 定义验证条件的Client
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
               new Client
               {
                   ClientId="authentication",//客户端唯一标识
                   ClientSecrets=new[]{new Secret("auth123456".Sha256()) },//客户端密码进行加密
                   //AllowedGrantTypes=(ICollection<string>)GrantType.ClientCredentials.AsEnumerable(),//验证模式
                   AllowedGrantTypes={GrantType.ClientCredentials },//验证模式
                   AllowedScopes=new []{ "UserApi"}//运行访问的资源，该用户可访问哪些Api
               }
            };
        }
    }
}
