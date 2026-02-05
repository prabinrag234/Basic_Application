using CommunityToolkit.Mvvm.Input;
using EShop;
using EShopNative.BaseLibrary;
using EShopNative.DataTransferObject;
using EShopNative.Interfaces;
using EShopNative.Pages;
using EShopNative.Services;

namespace EShopNative.ViewModels
{
    public partial class UserRoleEntryViewModel : BaseViewModel
    {
        private readonly AuthService _auth;
        private readonly IServiceProvider _services;
        private readonly IAlertService _alert;
        private readonly ISessionManager _session;

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public UserRoleEntryViewModel(
            AuthService auth,
            IServiceProvider services,
            IAlertService alert,
            ISessionManager session)
        {
            _auth = auth;
            _services = services;
            _alert = alert;
            _session = session;
        }

        [RelayCommand]
        public async Task Login()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    await _alert.ShowError("Email and password are required");
                    return;
                }

                var result = await _auth.Login(new LoginRequest
                {
                    Email = Email,
                    Password = Password
                });

                if (result == null)
                {
                    await _alert.ShowError("Invalid login");
                    return;
                }

                if (result.User == null)
                {
                    await _alert.ShowError("Invalid user data received");
                    return;
                }

                // Save session
                await _session.SaveSessionAsync(
                    result.AccessToken,
                    result.RefreshToken,
                    result.User
                );

                // Attach token for all future API calls
                _auth.AttachToken();

                // Reset root navigation to HomePage
                var homePage = _services.GetRequiredService<HomePage>();
                App.Current.MainPage = new NavigationPage(homePage);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task GoToRegister()
        {
            var registerPage = _services.GetRequiredService<RegisterPage>();
            App.Current.MainPage = new NavigationPage(registerPage);
        }
    }
}