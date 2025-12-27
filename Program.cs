using Kursach_CorpHubPortal.Data;
using Kursach_CorpHubPortal.Services;
using Kursach_CorpHubPortal.Services.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Kursach_CorpHubPortal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddServerSideBlazor();

            string? connectionString = builder.Configuration.GetConnectionString("Default");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new MissingFieldException("Failed to get Default connection string");

            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped(p =>
                p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options =>
                            {
                                options.LoginPath = "/Auth/Login";
                                options.LogoutPath = "/Auth/Logout";
                                options.AccessDeniedPath = "/Auth/AccessDenied";
                                options.Cookie.Name = "MyPortalAuth";
                                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                                options.SlidingExpiration = true;
                            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IAccount, Account>();
            builder.Services.AddScoped<IAvatar, Avatar>();
            builder.Services.AddScoped<IPositionService, PositionService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();

            // ВНИМАНИЕ: Если TaskService внутри использует DbContext, 
            // его тоже лучше сделать Transient или использовать фабрику внутри.
            builder.Services.AddScoped<ITaskService, TaskService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapBlazorHub();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
