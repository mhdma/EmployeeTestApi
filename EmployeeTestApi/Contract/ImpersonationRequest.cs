using System.Text.Json.Serialization;

namespace EmployeeTestApi.Contract
{
    public class ImpersonationRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}
