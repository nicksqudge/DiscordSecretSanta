using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.FluentValidation;
using Blazorise.Icons.FontAwesome;
using DiscordAuthProvider;
using DiscordSecretSanta.Blazor.Implementations;
using DiscordSecretSanta.Core;
using DiscordSecretSanta.MongoDb;
using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = DiscordDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddDiscord(options =>
    {
        options.AppId = builder.Configuration["Discord:AppId"];
        options.AppSecret = builder.Configuration["Discord:AppSecret"];

        options.SaveTokens = true;
        options.Events.OnCreatingTicket = context =>
        {
            Console.WriteLine("Signed in");
            return Task.CompletedTask;
        };
    });
builder.Services.AddLocalization();

// Blazorise
builder.Services.AddBlazorise(options => { options.Immediate = true; })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons()
    .AddBlazoriseFluentValidation();

// Add Domain core
builder.Services.AddCore()
    .AddUserService<AuthProviderService>()
    .AddSetupService<SetupService>();

// Add Database
builder.Services.AddMongoDb(
    new MongoClient(
            "mongodb://localhost:27017")
        .GetDatabase("DiscordSecretSanta"));

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapDefaultControllerRoute();

var supportedCultures = new[] { "en-GB" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.Run();