using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVSApp
{   

    public class Startup
    {

        private static IWebHostEnvironment env;

        /// <summary>
        ///  ���������� ��� �������� About
        /// </summary>
        private static void About(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"{env.ApplicationName} - ASP.Net Core tutorial project");
            });
        }

        /// <summary>
        ///  ���������� ��� ������� ��������
        /// </summary>
        private static void Config(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"App name: {env.ApplicationName}. App running configuration: {env.EnvironmentName}");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {            
            // �������������� ����, �������� ��� �������� ����������� ���������
            Startup.env = env;            

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //��������� ��������� ��� ����������� �������� � �������������� ������ Use.
            app.Use(async (context, next) =>
            {
                // ��� ����������� ������ � ������� ���������� �������� ������� HttpContext
                Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
                await next.Invoke();
            });

            //��������� ��������� � ���������� ��������� ��� ������� ��������
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
                });
            });

            // ��� ������ �������� ����� ��������� �����������
            app.Map("/about", About);
            app.Map("/config", Config);

            // ���������� ��� ������ "�������� �� �������"
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Page not found");
            });
        }
    }
}
