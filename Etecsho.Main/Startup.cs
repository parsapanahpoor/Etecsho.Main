using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etecsho.DataAccess.Context;
using Etecsho.DataAccess.Services.Blog;
using Etecsho.DataAccess.Services.Comment;
using Etecsho.DataAccess.Services.Employee;
using Etecsho.DataAccess.Services.Permission;
using Etecsho.DataAccess.Services.Product;
using Etecsho.DataAccess.Services.Slider;
using Etecsho.DataAccess.Services.Users;
using Etecsho.Utilities.Convertors;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Etecsho.Main
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

            #region Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);

            });

            #endregion

            #region Context
            services.AddDbContext<EtecshoContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ParsapanahpoorConnection")));
            #endregion

            #region IoC

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IViewRenderService, RenderViewToString>();
            services.AddTransient<IBlogCategoryService, BlogCategryService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ISliderService, SliderService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IProductService, ProductService>();


            #endregion

            services.AddControllersWithViews();
            services.AddControllersWithViews().AddNewtonsoftJson().AddRazorRuntimeCompilation();

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(

                    name: "MyAreas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(

                    name: "Default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
