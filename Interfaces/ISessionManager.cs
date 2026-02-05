using EShopNative.DataTransferObject;

namespace EShopNative.Interfaces
{
    public interface ISessionManager
    {
        bool IsLoggedIn { get; }
        string? AccessToken { get; }
        string? RefreshToken { get; }
        UserDTO? CurrentUser { get; }

        Task LoadSessionAsync();
        Task SaveSessionAsync(string accessToken, string refreshToken, UserDTO user);
        Task ClearSessionAsync();
    }

}
