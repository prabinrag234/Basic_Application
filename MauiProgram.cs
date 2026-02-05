using CommunityToolkit.Maui;
using EShop;
using EShopNative.BaseLibrary;
using EShopNative.Interfaces;
using EShopNative.Pages;
using EShopNative.Services;
using EShopNative.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace EShopNative
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcon");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // -----------------------------
            // CORRECT HttpClient registration
            // -----------------------------
            builder.Services.AddHttpClient<AuthService>(client =>
            {
                client.BaseAddress = new Uri(AppConstants.BaseApiUrl);
            });

            // -----------------------------
            // Register Services
            // -----------------------------
            builder.Services.AddSingleton<ISessionManager, SessionManager>();
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IdleTimeoutService>();

            // -----------------------------
            // Register ViewModels
            // -----------------------------
            builder.Services.AddTransient<UserRoleEntryViewModel>();
            builder.Services.AddTransient<HomePageViewModel>();

            // -----------------------------
            // Register Pages
            // -----------------------------
            builder.Services.AddTransient<UserRoleEntry>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<RegisterPage>();

            // -----------------------------
            // Register App
            // -----------------------------
            builder.Services.AddSingleton<App>();

#if DEBUG
            builder.Logging.AddDebug();
            builder.Logging.AddConsole();
#endif

            return builder.Build();
        }
    }
}