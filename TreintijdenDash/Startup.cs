using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;

namespace TreintijdenDash
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
            // Setup configuration sources
            var configuration = new Configuration();
            //configuration.AddJsonFile("config.json");
            configuration.AddEnvironmentVariables();

            // Set up application services
            app.UseServices(services =>
            {
                services.AddInstance<IConfiguration>(configuration);

                // Add MVC services to the services container
                services.AddMvc();
            });

            // Add MVC to the request pipeline
            app.UseMvc(routes =>
            {
                 routes.MapRoute(
                    name: "google",
                    template: "_ah/health",
                    defaults: new { controller = "Test", action = "Test" });
                    
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "api",
                    template: "{controller}/{id?}");
            });
        }
    }
}
