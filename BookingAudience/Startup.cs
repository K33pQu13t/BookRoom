using BookingAudience.DAL;
using BookingAudience.Models;
using BookingAudience.Models.Users;
using BookingAudience.DAL;
using BookingAudience.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookingAudience.Enums;

namespace BookingAudience
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
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.Configure<RouteOptions>(options =>
            {
                //options.LowercaseUrls = true;
                //options.LowercaseQueryStrings = true;
                options.AppendTrailingSlash = true;
            });
            services.AddHttpContextAccessor();


            services.AddAuthorization(options =>
            {
                options.AddPolicy(Role.Administrator.ToString(), builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, Role.Administrator.ToString());
                });
                options.AddPolicy(Role.Moderator.ToString(), builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, Role.Moderator.ToString());
                });
                options.AddPolicy(Role.Teacher.ToString(), builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, Role.Teacher.ToString());
                });
                options.AddPolicy(Role.Student.ToString(), builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, Role.Student.ToString());
                });
            });
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDBContext>(options =>
            options.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"]))
                .AddIdentity<AppUser, UserRole>(config =>
                {
                    config.Password.RequireDigit = false;
                    config.Password.RequireUppercase = false;
                    config.Password.RequireLowercase = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequiredLength = 3;

                    config.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/User/Login";
                config.AccessDeniedPath = "/error";
            });

            services.AddScoped<IGenericRepository<AppUser>, UsersRepository>();
            services.AddScoped<IGenericRepository<Audience>, AudiencesRepository>();
            services.AddScoped<IGenericRepository<Building>, BuildingsRepository>();
            services.AddScoped<IGenericRepository<Booking>, BookingsRepository>();
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
                app.UseExceptionHandler("/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();    //подключение аутентификации
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
