using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ServicesInstances.Uility;
using Microsoft.CodeAnalysis.Options;
using Microsoft.AspNetCore.Identity;

namespace ServicesInstances
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            //services.AddAuthentication("Bearer")
            //        .AddIdentityServerAuthentication(m =>
            //        {
            //            m.Authority = "http://localhost:7200";//Ids的地址，获取公钥
            //            m.ApiName = "GetCustomerUser";
            //            m.RequireHttpsMetadata = false;
            //        });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assemblysFrameWork = Assembly.Load("FrameWork");
            builder.RegisterAssemblyTypes(assemblysFrameWork);

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var businessDllFile = Path.Combine(basePath, "Services.dll");
            var assemblysBusiness = Assembly.LoadFrom(businessDllFile);
            builder.RegisterAssemblyTypes(assemblysBusiness)
            .AsImplementedInterfaces()
            .InstancePerDependency();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();//鉴权,没有鉴权,授权是没有意义的
            app.UseAuthorization();//授权

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //启动时注册,且注册一次
            this.Configuration.ConsulExtend();
        }
    }
}
