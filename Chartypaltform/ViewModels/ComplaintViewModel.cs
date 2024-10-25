using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.Models.ViewModels
{
    public class ComplaintViewModel
    {
        [Required]
        [MinLength(10)]
        [DataType(DataType.MultilineText)]
        public string ComplaintText { get; set; }



        public string Subject { get; set; }     
    }
}
