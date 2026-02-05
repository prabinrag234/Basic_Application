using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EShop;
using EShopNative.BaseLibrary;
using EShopNative.Interfaces;
using EShopNative.Pages;
using EShopNative.Services;

namespace EShopNative.ViewModels
{
    public partial class HomePageViewModel : BaseViewModel
    {
        private readonly AuthService _auth;
        private readonly IServiceProvider _services;
        private readonly IAlertService _alert;
        private readonly ISessionManager _session;

        [ObservableProperty]
        private string userName;

        public HomePageViewModel(
            AuthService auth,
            IServiceProvider services,
            IAlertService alert,
            ISessionManager session)
        {
            _auth = auth;
            _services = services;
            _alert = alert;
            _session = session;

            userName = session.CurrentUser?.Email ?? "User";
        }

        [RelayCommand]
        public async Task Logout()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                // Call API logout
                await _auth.LogoutAsync();

                // Clear local session
                await _session.ClearSessionAsync();

                // Reset root navigation
                var loginPage = _services.GetRequiredService<UserRoleEntry>();
                App.Current.MainPage = new NavigationPage(loginPage);

                await _alert.ShowSuccess("Logged out successfully");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}