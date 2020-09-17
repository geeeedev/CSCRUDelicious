using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSCRUDelicious.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CSCRUDelicious
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // Here we will "inject" the default IConfiguration service, which will bind to appsettings.json by default
            // and then assign it to the Configuration property.  This happens at the startup of our application.
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This public getter will be how you access the data from appsettings.json
        // To access the connection string itself, you would use the following:
        // Configuration["DBInfo:ConnectionString"]

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.Configure<CookiePolicyOptions>(options =>
            // {
            //     // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //     options.CheckConsentNeeded = context => true;
            //     options.MinimumSameSitePolicy = SameSiteMode.None;
            // });

            
            // Now we may use the connection string, bound to Configuration, to pass the required connection
            // credentials to MySQL
            services.AddDbContext<dbCRUDeliciousContext>(options => options.UseMySql(Configuration["DBInfo:ConnectionString"]));        //pulling connection string to connect to MySQL db from "appsetting.json" file 
            services.AddSession();
            // services.AddHttpContextAccessor();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            // app.UseCookiePolicy();
            app.UseSession();

            app.UseMvc(routes =>                //app.UseMvc - last last!
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
