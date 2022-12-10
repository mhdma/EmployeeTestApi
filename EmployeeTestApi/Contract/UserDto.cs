using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeTestApi.Contract
{

    public class UserDto 
    {

        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }


    }

}
