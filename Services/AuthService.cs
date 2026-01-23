using EShopNative.BaseLibrary;
using EShopNative.DataTransferObject;
using System.Net.Http.Json;


namespace EShopNative.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(AppConstants.BaseApiUrl)
            };
        }

        public async Task<AuthResponse?> Login(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.Login, request);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.Registration, request);
            return response.IsSuccessStatusCode;
        }

        public async Task<AuthResponse?> Refresh(string refreshToken)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.RefreshToken, refreshToken);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }
    }
}
