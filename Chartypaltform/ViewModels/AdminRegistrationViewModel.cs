using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.ViewModels
{
    public class AdminRegistrationViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
        public string? Password { get; set; } = string.Empty;

		[DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = ("Password Not match"))]
        public string ?ConfirmPassword { get; set; } = string.Empty;

		[Required(ErrorMessage = "Full name is required.")]
        [Display(Name = "Full Name")]
        public string? AdminFullName { get; set; } = string.Empty;
		public string? Address { get; set; } = string.Empty;
		public string? PhoneNumber { get; set; } = string.Empty;

	}
}
