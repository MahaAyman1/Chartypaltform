using Chartypaltform.Models;

namespace Chartypaltform.ViewModels
{
    public class AdminActionViewModel
    {
        
        public int Id { get; set; }
        public ActionType ActionType { get; set; }
        public string ActionDetails { get; set; }
        public string Reason { get; set; }
        public DateTime Timestamp { get; set; }
        public string AdminFullName { get; set; } // Admin's Full Name
    }
}

