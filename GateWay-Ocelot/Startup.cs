using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.Cache;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace GateWay_Ocelot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Identity4
            var authenticationProviderKey = "Gatewaykey";
            services.AddAuthentication("Bearer")
                   .AddIdentityServerAuthentication(authenticationProviderKey, m =>
                    {
                        m.Authority = "http://localhost:7200";//Ids�ĵ�ַ����ȡ��Կ
                        m.ApiName = "GetCustomerUser";
                        m.RequireHttpsMetadata = false;
                        m.SupportedTokens = SupportedTokens.Both;
                    });
            #endregion

            services.AddOcelot().AddConsul()
                .AddCacheManager(m =>
                {
                    m.WithDictionaryHandle();//Ĭ���ֵ�洢
                })
                .AddPolly();
            //services.AddControllers();

            //�����IOcelotCache<CachedResponse>��Ĭ�ϻ����Լ��--׼���滻���Զ����
            services.AddSingleton<IOcelotCache<CachedResponse>, CustomeCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //��Ĭ�ϵ�����ܵ�ȫ������
            app.UseOcelot();
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseHttpsRedirection();

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
