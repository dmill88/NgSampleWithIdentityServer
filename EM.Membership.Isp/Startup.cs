using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using EM.MembershipDb;
using EM.Membership.Isp.Account;

namespace EM.Membership.Isp
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //string key = Configuration["BearerToken:Key"];
            //SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            //string tokenIssuer = Configuration["BearerToken:Issuer"];
            //string tokenAudience = Configuration["BearerToken:Audience"];

            string dbConnString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MembershipDbContext>(options => options.UseSqlServer(dbConnString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MembershipDbContext>()
                .AddDefaultTokenProviders();

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

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc();
            services.AddTransient<IProfileService, CustomProfileService>();

            string[] oidcApiUris = GetUrisFromConfig("OIDC:ApiUris");
            string[] mvcApiUris = GetUrisFromConfig("OIDC:MvcUris");

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Authentication.CookieLifetime = TimeSpan.FromMinutes(45); // The cookie timeout needs to be longer than the token timeout, so virtual sliding timeout works. 
            })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients(oidcApiUris, mvcApiUris))
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<CustomProfileService>();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("Need to add signing credential");
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts. 
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();
            app.UseCors(env.EnvironmentName);

            app.UseAuthentication();
            app.UseIdentityServer();    // Adds the cookie authentication. The cookie will be used by IdentityServer to issue a token. 
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

        }
    }
}
