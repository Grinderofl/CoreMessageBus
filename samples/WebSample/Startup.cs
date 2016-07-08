using System;
using System.IO;
using Core;
using CoreMessageBus;
using CoreMessageBus.ServiceBus;
using CoreMessageBus.ServiceBus.Extensions;
using CoreMessageBus.ServiceBus.SqlServer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebSample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services
                .AddServiceBus(x => x
                    .SendOnly()
                    .UseSqlServer(s => s.ConnectionString("Server=.;Database=ServiceBusQueue;Trusted_Connection=True;"))
                    .Handles("Queue1", new[] {typeof (Message)})
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //try
            //{
            //    app.ApplicationServices.GetService<IServiceBus>();
            //}
            //catch (Exception e)
            //{
                
            //}
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }

        // Entry point for the application.
        public static void Main(string[] args)
            =>
                new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build().Run();
    }
}
