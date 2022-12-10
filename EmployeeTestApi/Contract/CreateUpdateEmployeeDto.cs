using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeTestApi.Contract
{

    public class CreateUpdateEmployeeDto
    {

        
        [Required]
        [JsonPropertyName("Name")]
        public virtual string Name { get; set; }
        [JsonPropertyName("Email")]
        public virtual string Email { get; set; }=String.Empty;
        [JsonPropertyName("Phone")]
        public virtual string Phone { get; set; }=String.Empty;
    }

}
