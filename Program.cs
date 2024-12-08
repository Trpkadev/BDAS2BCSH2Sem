using BCSH2BDAS2.Helpers;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BCSH2BDAS2;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        ConfigureMiddleware(app);

        app.UseRequestLocalization("en-UK");

        app.Run();
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseResponseCompression();
        app.UseSession();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}");
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews().AddCookieTempDataProvider();
        services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddDbContext<TransportationContext>(options =>
#if DEBUG
            options.UseOracle(configuration.GetConnectionString("DebugConnection")));
#elif RELEASE
            options.UseOracle(configuration.GetConnectionString("DefaultConnection")));
#endif
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.AddHttpContextAccessor();
    }
}