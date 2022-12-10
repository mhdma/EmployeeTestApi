using EmployeeTestApi.Infrastructure;
using System.Text.Json.Serialization;

namespace EmployeeTestApi.Contract
{
    public class JwtAuthResult
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public RefreshToken RefreshToken { get; set; }
    }
}
