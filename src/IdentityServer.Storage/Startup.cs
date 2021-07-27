using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Storage
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
            var connectionString = Configuration.GetConnectionString("id4mssql");
            var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;


            Action<DbContextOptionsBuilder> dbContextBuilder = builder =>
             {
                 builder.UseSqlServer(connectionString, sqlOptions =>
                 {
                     sqlOptions.MigrationsAssembly(migrationsAssembly);
                 });
             };

            services.AddIdentityServer()//注册服务
                .AddDeveloperSigningCredential() // 添加临时秘钥
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = dbContextBuilder;
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = dbContextBuilder;
                })
                ;

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
