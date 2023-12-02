using DiscordSecretSanta.Configure;

namespace DiscordSecretSanta;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDiscordSecretSantaServices();

        services.AddControllersWithViews();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.AddDiscordSecretSanta();

        // Configure the HTTP request pipeline.
        if (!env.IsDevelopment())
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller}/{action=Index}/{id?}");

            endpoints.MapFallbackToFile("index.html");
        });
    }
}