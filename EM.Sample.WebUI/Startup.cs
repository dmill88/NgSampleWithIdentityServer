using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EM.Sample.WebUI
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
            string[] localHostOriginsDev = GetUrisFromConfig("CORS:Development");
            string[] localHostOriginsStaging = GetUrisFromConfig("CORS:Staging");
            string[] localHostOriginsProd = GetUrisFromConfig("CORS:Production");

            services.AddCors(options => {
                options.AddPolicy("Development", config =>
                {
                    config.WithOrigins(localHostOriginsDev).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
                options.AddPolicy("Staging", config =>
                {
                    config.WithOrigins(localHostOriginsStaging).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
                options.AddPolicy("Production", config =>
                {
                    config.WithOrigins(localHostOriginsProd).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        private string[] GetUrisFromConfig(string key)
        {
            IEnumerable<string> stringArray = Configuration.GetSection(key).GetChildren().Select(i => {
                string uriBase = i.Value;
                if (uriBase.EndsWith("/"))
                    uriBase = uriBase.Substring(0, uriBase.LastIndexOf("/"));
                return uriBase;
            });
            return stringArray.ToArray();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(env.EnvironmentName);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.Options.StartupTimeout = TimeSpan.FromSeconds(200); // <-- add this line
                    //spa.UseAngularCliServer(npmScript: "start");  // Use this to have ng build run each time there is a change to a file. Comment out when using ng build --watch
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200/");   // Use this when running ng serve manually. 
                }
            });
        }
    }
}
