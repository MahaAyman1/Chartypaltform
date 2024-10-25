// Models/AdminAction.cs
using System;

namespace Chartypaltform.Models
{

    public class AdminAction
    {
        public int Id { get; set; }
        public ActionType ActionType { get; set; }
        public string ActionDetails { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string AdminId { get; set; }
   

        public AdminUser AdminUser { get; set; } = null!;
    }

    public enum ActionType
    {
        ApproveCampaign,
        RejectCampaign,
        AcceptOrganization,
        RejectOrganization,
        AccessComplaints,
        CreateAdmin,
        PendingComplaints,    // Default
        ResolvedComplaints,
        DismissedComplaints
    }

}
