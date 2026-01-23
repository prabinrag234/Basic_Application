using System.Text.Json.Serialization;

namespace EShopNative.DataTransferObject
{
    public class AuthResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        public UserDTO ?User { get; set; }
    }


    public class RegisterRequest
    {
        public string ?Email { get; set; }
        public string ?Username { get; set; }
        public string ?Password { get; set; }
    }
    public class LoginRequest
    {
        public string ?Email { get; set; }
        public string ?Password { get; set; }
    }
}