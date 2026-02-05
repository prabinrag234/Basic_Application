using EShopNative.Interfaces;
using EShopNative.Pages;
using EShopNative.Services;

namespace EShop
{
    public partial class App : Application
    {
        private readonly IServiceProvider _services;

        public App(IServiceProvider services)
        {
            InitializeComponent();
            _services = services;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Temporary loading screen
            var loadingPage = new ContentPage
            {
                Content = new ActivityIndicator
                {
                    IsRunning = true,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                }
            };

            var window = new Window(new NavigationPage(loadingPage));

            // Attach global input handlers once the window is ready
            window.HandlerChanged += (s, e) =>
            {
                if (window.Handler?.PlatformView is not null)
                {
                    AttachGlobalInputHandlers(window);
                }
            };

            // Run async session check AFTER window is created
            _ = InitializeApp(window);

            return window;
        }

        private async Task InitializeApp(Window window)
        {
            try
            {
                var session = _services.GetRequiredService<ISessionManager>();
                await session.LoadSessionAsync();

                var auth = _services.GetRequiredService<AuthService>();
                Page startPage;

                if (!string.IsNullOrEmpty(session.RefreshToken))
                {
                    var refreshed = await auth.RefreshAsync(session.RefreshToken);

                    if (refreshed is { User: not null })
                    {
                        await session.SaveSessionAsync(
                            refreshed.AccessToken,
                            refreshed.RefreshToken,
                            refreshed.User
                        );

                        auth.AttachToken(); // IMPORTANT

                        startPage = _services.GetRequiredService<HomePage>();
                    }
                    else
                    {
                        await session.ClearSessionAsync();
                        startPage = _services.GetRequiredService<UserRoleEntry>();
                    }
                }
                else
                {
                    auth.AttachToken(); // IMPORTANT
                    startPage = _services.GetRequiredService<UserRoleEntry>();
                }

                window.Page = new NavigationPage(startPage);
            }
            catch
            {
                var fallback = _services.GetRequiredService<UserRoleEntry>();
                window.Page = new NavigationPage(fallback);
            }
        }
        private void AttachGlobalInputHandlers(Window window)
        {
#if ANDROID
            if (window.Handler?.PlatformView is Android.Views.View view)
            {
                view.Touch += (s, e) =>
                {
                    var idle = IPlatformApplication.Current?.Services?.GetService<IdleTimeoutService>();
                    idle?.ResetTimer();
                };
            }
#endif

#if IOS
    if (window.Handler?.PlatformView is UIKit.UIView uiView)
    {
        var gesture = new UIKit.UITapGestureRecognizer(() =>
        {
            var idle = IPlatformApplication.Current?.Services?.GetRequiredService<IdleTimeoutService>();
            idle?.ResetTimer();
        });

        uiView.AddGestureRecognizer(gesture);
    }
#endif

#if WINDOWS
if (window.Handler?.PlatformView is Microsoft.UI.Xaml.Window win)
{
    var root = win.Content as Microsoft.UI.Xaml.FrameworkElement;

    if (root != null)
    {
        root.PointerPressed += (s, e) =>
        {
            var idle = IPlatformApplication.Current?.Services?.GetService<IdleTimeoutService>();
            idle?.ResetTimer();
        };

        root.PointerMoved += (s, e) =>
        {
            var idle = IPlatformApplication.Current?.Services?.GetService<IdleTimeoutService>();
            idle?.ResetTimer();
        };
    }
}
#endif
        }

    }
}