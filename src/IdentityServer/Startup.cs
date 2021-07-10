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
            services.AddIdentityServer()//ע�����
                .AddDeveloperSigningCredential() // �����ʱ��Կ
                .AddInMemoryApiScopes(Config.ApiScopes)//������Ȩ��Χ
                .AddInMemoryApiResources(Config.ApiResources)//����API��Դ
                .AddInMemoryClients(Config.Clients)//�����ඨ�����Ȩ�ͻ���
                .AddInMemoryIdentityResources(Config.IdentityResources) // ���IdentityResources
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

            // TODO: �ƶ�Cookie����
            // �������SameSite���� https://www.jianshu.com/p/7fb032cf2a98
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
