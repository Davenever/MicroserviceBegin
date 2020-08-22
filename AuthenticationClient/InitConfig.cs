using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                new ApiResource("GetCustomerUser","用户获取Api")
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
                   //AllowedGrantTypes=GrantType.ClientCredentials,//验证模式
                   AllowedGrantTypes={GrantType.ClientCredentials },//验证模式
                   AllowedScopes=new []{ "GetCustomerUser"},//作用域,可以访问的资源，该用户可访问哪些Api
                   Claims=new List<Claim>()
                   {
                       new Claim(IdentityModel.JwtClaimTypes.Role,"admin"),
                        new Claim(IdentityModel.JwtClaimTypes.NickName,"江北"),
                         new Claim("Email","18872046079@163.com"),
                   }
               }
            };
        }
    }
}
