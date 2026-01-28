using System.ComponentModel.DataAnnotations;

namespace GCP_Pratice.Models
{
    public class LoginModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}