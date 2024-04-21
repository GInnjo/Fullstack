using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Fullstack.Models
{
	public class PasswordEditViewModel
	{
		[Required]
		[StringLength(100, MinimumLength = 10)]
		[DataType(DataType.Password)]
		public string Password { get; set; } = "123456789";

		[Required]
		[DisplayName("Confirm Password")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required]
		[DisplayName("Current Password")]
		[StringLength(100, MinimumLength = 10)]
		[DataType(DataType.Password)]
		public string CurrentPassword { get; set; }
	}
}
