using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.Models

{
    public class AdminUser : ApplicationUser
    {


        [Required]
        public string AdminFullName { get; set; } = string.Empty;
            }

}
