using Chartypaltform.Models;
using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.ViewModels
{
    public class RegisterViewModel
    {
        public string UserType { get; set; }
        [Required(ErrorMessage = "Enter Your Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Confirm Email")]
        [EmailAddress]
        [Compare("Email", ErrorMessage = ("Not match"))]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Enter pass")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter  Confirm pass")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = ("Pass Not match"))]
        public string ConfirmPassword { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Img { get; set; }
        // Specific fields for donors

        public string FullName { get; set; }

        public int Age { get; set; }
        public string Gender { get; set; }
        // Specific fields for organizations
        public string OrganizationName { get; set; }
        public RegistrationStatus registration_status { get; set; }
 
        public string InstagramLink { get; set; }
        public string FacebookLink { get; set; }
        public string GoogleLink { get; set; }
        public DateTime? CreatedAT { get; set; }
    }
}
