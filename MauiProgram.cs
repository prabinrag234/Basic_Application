using CommunityToolkit.Maui;
using EShop;
using EShopNative.BaseLibrary;
using EShopNative.Pages;
using EShopNative.Services;
using EShopNative.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System.Net.Http.Headers;

namespace EShopNative
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Core MAUI setup
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcon");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register HttpClient
            builder.Services.AddHttpClient<AuthService>(async client =>
            {
                client.BaseAddress = new Uri(AppConstants.BaseApiUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("AccessToken"));
            });


            // Register ViewModels
            builder.Services.AddTransient<UserRoleEntryViewModel>();
            builder.Services.AddTransient<HomePageViewModel>();

            // Register Pages
            builder.Services.AddTransient<UserRoleEntry>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<RegisterPage>();

            // Custom UI Handlers
            ConfigureCustomHandlers();

            // Register your services
            builder.Services.AddSingleton<App>();
            builder.Services.AddSingleton<AuthService>();

#if DEBUG
            builder.Logging.AddDebug();
            builder.Logging.AddConsole();
#endif
            // Build the app FIRST
            var app = builder.Build();
            return builder.Build();
        }

        private static void ConfigureCustomHandlers()
        {
            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.Background = null;
#endif
#if IOS || MACCATALYST
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
#if WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });

            PickerHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.Background = null;
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
#endif
#if IOS || MACCATALYST
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
#if WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });
        }
    }
}