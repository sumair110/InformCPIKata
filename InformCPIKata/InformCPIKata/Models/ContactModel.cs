using System.ComponentModel.DataAnnotations;

namespace InformCPIKata.Models
{
	public class Contact
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "First name is required.")]
		[StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required.")]
		[StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Phone number is required.")]
		[RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email format.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Address is required.")]
		public string Address { get; set; }
	}
}
