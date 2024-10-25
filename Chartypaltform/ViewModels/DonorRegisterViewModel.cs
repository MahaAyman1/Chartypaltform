using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.ViewModels
{
    public class DonorRegisterViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Confirm Email")]
        [EmailAddress]
        [Compare("Email", ErrorMessage = ("Not match"))]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Enter pass")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = ("Password Not match"))]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile? formFile { get; set; }
        public string? Img { get; set; }
        public string FullName { get; set; } = string.Empty;    

        public int Age { get; set; }
        public string Gender { get; set; }
    }
}
