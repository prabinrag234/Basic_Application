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
            // Temporary loading screen while we check authentication
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

            // Run async login check AFTER window is created
            _ = InitializeApp(window);

            return window;
        }

        private async Task InitializeApp(Window window)
        {
            var refreshToken = await SecureStorage.GetAsync("RefreshToken");

            Page startPage;
                
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var authService = _services.GetRequiredService<AuthService>();
                var result = await authService.Refresh(refreshToken);

                if (result != null)
                {
                    if (!string.IsNullOrEmpty(result.AccessToken))
                    {
                        await SecureStorage.SetAsync("AccessToken", result.AccessToken);
                    }
                    startPage = _services.GetRequiredService<HomePage>();
                }
                else
                {
                    startPage = _services.GetRequiredService<UserRoleEntry>();
                }
            }
            else
            {
                startPage = _services.GetRequiredService<UserRoleEntry>();
            }

            // Update the active window’s root page (correct for .NET 10)
            window.Page = new NavigationPage(startPage);
        }
    }
}