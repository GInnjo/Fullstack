using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Fullstack.Models
{
	public class UserEditViewModel
	{
		[Required]
		[StringLength(30)]
		[DisplayName("First Name")]
		public string FirstName { get; set; }

		[Required]
		[StringLength(30)]
		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[Required]
		[StringLength(100)]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 4)]
		[RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only letters and numbers are allowed.")]
		public string? Username { get; set; }
	}
}
