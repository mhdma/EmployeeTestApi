using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeTestApi.Contract
{

    public class EmployeeDto
    {

        [Required]
        [JsonPropertyName("Id")]
        public virtual int Id { get; set; }
        [Required]
        [JsonPropertyName("Name")]
        public virtual string Name { get; set; }
        [JsonPropertyName("Email")]
        public virtual string Email { get; set; }=String.Empty;
        [JsonPropertyName("Phone")]
        public virtual string Phone { get; set; }=String.Empty;
    }

}
