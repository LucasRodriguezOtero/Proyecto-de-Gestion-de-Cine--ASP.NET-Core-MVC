using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TP_FINAL_GRUPO_C.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TP_FINAL_GRUPO_C
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Context"));
            });

            //Cookie
            builder.Services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(options =>
                { options.LoginPath = "/login";
                  options.Cookie.Name = "LaCookieDeLosPibes"; 
                });
            
            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim("EsAdmin", "True");
                });
            });
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
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
                    name: "miperfil",
                    pattern: "/MiPerfil",
                    defaults: new {controller = "MiPerfil", action = "Index"}
                    );

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}