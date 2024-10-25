using Chartypaltform.Models;
using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.ViewModels
{
    public class CharityOrganizationRegistrationModel
    {
        public string OrganizationName { get; set; }
        public RegistrationStatus registration_status { get; set; }
      
 
        public string InstagramLink { get; set; }
        public string FacebookLink { get; set; }
        public string GoogleLink { get; set; }
        public DateTime? CreatedAT { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Enter Your Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Confirm Email")]
        [EmailAddress]
        [Compare("Email", ErrorMessage = ("Not match"))]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter  Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = ("Password Not match"))]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public IFormFile? formFile { get; set; }
        public string ? Img { get; set; }   


    }
}
