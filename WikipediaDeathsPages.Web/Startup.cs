using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using Wikimedia.Utilities.Interfaces;
using Wikimedia.Utilities.Services;
using WikipediaDeathsPages.Data.Interfaces;
using WikipediaDeathsPages.Data.Models;
using WikipediaDeathsPages.Data.Repositories;
using WikipediaDeathsPages.Service;
using WikipediaDeathsPages.Service.Interfaces;

namespace WikipediaDeathsPages
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
            string webApiConnectionString = Configuration.GetConnectionString("WikipediaReferencesDBConnection");

            Action<DbContextOptionsBuilder> optionActionCreator(string connectionString)
            {
                return options => options.UseSqlServer(webApiConnectionString);
            }

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddDbContext<WRContext>(optionActionCreator(webApiConnectionString));
            services.AddSingleton<HttpClient>();
            services.AddSingleton<IWikipediaWebClient, WikipediaWebClient>();
            services.AddScoped<IWikipediaReferences, WikipediaReferences>();
            services.AddScoped<IWikipediaService, WikipediaService>();
            services.AddScoped<IWikidataService, WikidataService>();
            services.AddScoped<IWikiTextService, WikiTextService>();
            services.AddScoped<IReferenceService, ReferenceService>();
            services.AddScoped<IToolforgeService, ToolforgeService>();
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

            app.UseHttpsRedirection();

            app.UseStaticFiles();
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
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
