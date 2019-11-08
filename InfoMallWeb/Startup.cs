using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InfoMallWeb.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InfoMallWeb.Models;
using InfoMallWeb.Services;
using InfoMallWeb.Repository;
using Microsoft.Net.Http.Headers;

namespace InfoMallWeb
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
       
        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseMySql(
            //        Configuration.GetConnectionString("DefaultConnection")));
            //services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            //{
            //    options.Password.RequireDigit = false;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = false;
            //})
            //    .AddEntityFrameworkStores<ApplicationDbContext>();


            //services.AddMvc()

            //services for repositories
            services.AddTransient<IBannerRepository, BannerInfoRepository>();
            services.AddTransient<ICategoryForInfoRepository, CategoryForInfoRepository>();
            services.AddTransient<ICategoryForTabRepository, CategoryForTabRepository>();
            services.AddTransient<IClienteleRepository, ClienteleRepository>();
            services.AddTransient<IContactInfoRepository, ContactInfoRepository>();
            services.AddTransient<IContentForMallRepository, ContentForMallRepository>();
            services.AddTransient<IContentForTabRepository, ContentForTabRepository>();
            services.AddTransient<IContentImageRepository, ContentImageRepository>();
            services.AddTransient<ICustomerProductRepository, CustomerProductRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IPromotionCustomerRepository, PromotionCustomerRepository>();
            services.AddTransient<IPromotionInfoRepository, PromotionInfoRepository>();
            services.AddTransient<IPromotionRepository, PromotionRepository>();

            //Services for email and images
            //Services for email and images
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IImageService, ImageService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });
            //app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
