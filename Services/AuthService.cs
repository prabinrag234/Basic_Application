using EShopNative.BaseLibrary;
using EShopNative.DataTransferObject;
using EShopNative.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace EShopNative.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient; 
        private readonly ISessionManager _session;

        public AuthService(HttpClient httpClient, ISessionManager session)
        {
            _httpClient = httpClient;
            _session = session;

            _httpClient.BaseAddress = new Uri(AppConstants.BaseApiUrl);
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

        public async Task<AuthResponse?> RefreshAsync(string refreshToken)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.RefreshToken, refreshToken); if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }
        public async Task<bool> LogoutAsync()
        {
            if (_session.RefreshToken == null)
                return true;

            var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.Logout, _session.RefreshToken);

            await _session.ClearSessionAsync();

            return response.IsSuccessStatusCode;
        }
        public void AttachToken()
        {
            if (!string.IsNullOrEmpty(_session.AccessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _session.AccessToken);
            }
        }
        public async Task<HttpResponseMessage> SendAsync(Func<Task<HttpResponseMessage>> action)
        {
            AttachToken();

            var response = await action();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshResult = await RefreshAsync(_session.RefreshToken!);

                if (refreshResult == null)
                {
                    await _session.ClearSessionAsync();
                    return response;
                }

                await _session.SaveSessionAsync(
                    refreshResult.AccessToken,
                    refreshResult.RefreshToken,
                    refreshResult.User
                );

                AttachToken();
                response = await action();
            }

            return response;
        }

    }
}
