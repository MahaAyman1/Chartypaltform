using System.ComponentModel.DataAnnotations.Schema;

namespace Chartypaltform.Models
{
	public class TUI
	{
		public int TUIID { get; set; }
		public string TUIName
		{
			get; set;

		}
		public int CampaignId { get; set; }
		public Campaign Campaign { get; set; }


	}
}
