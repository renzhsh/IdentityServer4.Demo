using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
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
            services.AddIdentityServer()//注册服务
                .AddDeveloperSigningCredential() // 添加临时秘钥
                .AddInMemoryApiScopes(Config.ApiScopes)//配置授权范围
                .AddInMemoryApiResources(Config.ApiResources)//配置API资源
                .AddInMemoryClients(Config.Clients)//配置类定义的授权客户端
                .AddInMemoryIdentityResources(Config.IdentityResources) // 添加IdentityResources
                .AddTestUsers(Config.TestUsers)
                ;

            //services.ConfigureNonBreakingSameSiteCookies();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            //app.UseCookiePolicy();

            // TODO: 制定Cookie方案
            // 浏览器的SameSite策略 https://www.jianshu.com/p/7fb032cf2a98
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
