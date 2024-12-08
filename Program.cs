using BCSH2BDAS2.Helpers;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);

        var app = builder.Build();
        ConfigureMiddleware(app);

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
        app.UseRequestLocalization("en-UK");
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel((context, options) => options.Configure(context.Configuration.GetSection("Kestrel")));

        builder.Services.AddControllersWithViews().AddCookieTempDataProvider();
        builder.Services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddDbContext<TransportationContext>(options =>
#if DEBUG
            options.UseOracle(builder.Configuration.GetConnectionString("DebugConnection")).EnableSensitiveDataLogging(false).EnableDetailedErrors(false););
#elif RELEASE
            options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging(false).EnableDetailedErrors(false));
#endif
        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        builder.Services.AddHttpContextAccessor();
    }
}