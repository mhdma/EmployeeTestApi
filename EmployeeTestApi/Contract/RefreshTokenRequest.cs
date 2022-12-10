using System.Text.Json.Serialization;

namespace EmployeeTestApi.Contract
{
   
        public class RefreshTokenRequest
        {
            [JsonPropertyName("refreshToken")]
            public string RefreshToken { get; set; }
        }
    
}
