using System.ComponentModel.DataAnnotations;

namespace Fullstack.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
