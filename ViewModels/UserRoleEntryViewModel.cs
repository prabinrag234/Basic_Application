using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EShopNative.BaseLibrary;
using EShopNative.DataTransferObject;
using EShopNative.Helper;
using EShopNative.Pages;
using EShopNative.Services;

namespace EShopNative.ViewModels
{
    public partial class UserRoleEntryViewModel : BaseViewModel
    {
        private readonly AuthService _auth;
        private readonly IServiceProvider _services;

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public UserRoleEntryViewModel(AuthService auth, IServiceProvider services)
        {
            _auth = auth;
            _services = services;
        }

        [RelayCommand]
        public async Task Login()
        {
            var result = await _auth.Login(new LoginRequest
            {
                Email = _email,
                Password = _password
            });

            if (result == null)
            {
                await WindowHelper.GetCurrentPage().DisplayAlertAsync("Error", "Invalid login", "OK");
                return;
            }

            if (!string.IsNullOrEmpty(result.AccessToken))
            {
                await SecureStorage.SetAsync("AccessToken", result.AccessToken);
            }

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                await SecureStorage.SetAsync("RefreshToken", result.RefreshToken);
            }


            var homePage = _services.GetRequiredService<HomePage>();
            await WindowHelper.GetCurrentPage().Navigation.PushAsync(homePage);
        }

        [RelayCommand]
        public async Task GoToRegister()
        {
            var registerPage = _services.GetRequiredService<RegisterPage>();
            await WindowHelper.GetCurrentPage().Navigation.PushAsync(registerPage);
        }
    }
}           