using CommunityToolkit.Mvvm.ComponentModel;

namespace EShopNative.BaseLibrary
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;
        public bool IsNotBusy => !IsBusy;
    }

    public class AppConstants
    {
        public const string BaseApiUrl = "https://eshopapi-a6haa8b6azg4bqc7.centralindia-01.azurewebsites.net";
    }
    public class ApiEndpoints
    {
        public const string Login = "auth/login";
        public const string Registration = "auth/register";
        public const string RefreshToken = "auth/refresh";
        public const string Logout = "/auth/logout";
    }

}