using EShopNative.DataTransferObject;
using EShopNative.Interfaces;
using System.Text.Json;

namespace EShopNative.Services
{
    public class SessionManager : ISessionManager
    {
        public bool IsLoggedIn => !string.IsNullOrEmpty(AccessToken);
        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }
        public UserDTO? CurrentUser { get; private set; }

        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_token";
        private const string UserKey = "user";

        public async Task LoadSessionAsync()
        {
            AccessToken = await SecureStorage.GetAsync(AccessTokenKey);
            RefreshToken = await SecureStorage.GetAsync(RefreshTokenKey);

            var userJson = await SecureStorage.GetAsync(UserKey);
            if (!string.IsNullOrEmpty(userJson))
                CurrentUser = JsonSerializer.Deserialize<UserDTO>(userJson);
        }

        public async Task SaveSessionAsync(string accessToken, string refreshToken, UserDTO user)
        {
            await SecureStorage.SetAsync(AccessTokenKey, accessToken);
            await SecureStorage.SetAsync(RefreshTokenKey, refreshToken);
            await SecureStorage.SetAsync(UserKey, JsonSerializer.Serialize(user));

            AccessToken = accessToken;
            RefreshToken = refreshToken;
            CurrentUser = user;
        }

        public async Task ClearSessionAsync()
        {
            AccessToken = null;
            RefreshToken = null;
            CurrentUser = null;

            SecureStorage.Remove(AccessTokenKey);
            SecureStorage.Remove(RefreshTokenKey);
            SecureStorage.Remove(UserKey);

            await Task.CompletedTask;
        }
    }
}