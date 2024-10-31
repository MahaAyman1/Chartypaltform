 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chartypaltform.Data;
using Chartypaltform.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using System.Text;

namespace Chartypaltform.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    //act as No catgory 
    public class CampaignsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CampaignsController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Campaigns
                .Include(c => c.User).Where(c => c.Status != CampaignStatus.Closed)
                .OrderBy(c => c.Status == CampaignStatus.Pending ? 0 : 1) // 'Pending' campaigns come first
                .ThenByDescending(c => c.CreatedAt);

            return View(await applicationDbContext.ToListAsync());
        }




        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CampaignId == id);
            if (campaign == null)
            {
                return NotFound();
            }

            return View(campaign);
        }


        private bool CampaignExists(int id)
        {
            return _context.Campaigns.Any(e => e.CampaignId == id);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int campaignId, CampaignStatus status, string reason)
        {
            var campaign = await _context.Campaigns
                                         .FirstOrDefaultAsync(c => c.CampaignId == campaignId);
            if (campaign == null)
            {
                TempData["error"] = "Failed to update status. Campaign not found.";
                return RedirectToAction(nameof(Index));
            }

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adminUser = await _context.Users
                                          .OfType<AdminUser>()
                                          .FirstOrDefaultAsync(u => u.Id == adminId);

            if (adminUser == null)
            {
                TempData["error"] = "Failed to update status. Admin user not found.";
                return RedirectToAction(nameof(Index));
            }

            var adminAction = new AdminAction
            {
                ActionType = status == CampaignStatus.Closed ? ActionType.RejectCampaign : ActionType.ApproveCampaign,
                ActionDetails = $"Campaign '{campaign.CampaignName}' status updated to '{status}'.",
                Reason = reason,
                Timestamp = DateTime.UtcNow,
                AdminId = adminId,
                AdminUser = adminUser
            };

            _context.AdminActions.Add(adminAction);

            campaign.Status = status;
            _context.Campaigns.Update(campaign);

            await _context.SaveChangesAsync();
            TempData["success"] = "Status updated successfully.";

            return RedirectToAction(nameof(Index));
        }

		public FileResult Export()
		{
			// Fetching all campaigns.
			List<object> campaigns = (from campaign in this._context.Campaigns
									  select new[] {
								  campaign.CampaignId.ToString(),
								  campaign.CampaignName,
								  campaign.CampaignDes,
								  campaign.GoalAmount.ToString(),
								  campaign.CurrentAmountRaised.ToString(),
								  campaign.Status.ToString(),
								  campaign.CreatedAt.ToString("yyyy-MM-dd")
							  }).ToList<object>();

			// Building an HTML string.
			StringBuilder sb = new StringBuilder();

			// Table start.
			sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-family: Arial; font-size: 10pt;'>");

			// Building the Header row.
			sb.Append("<tr>");
			sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Campaign ID</th>");
			sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Campaign Name</th>");
			sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Description</th>");
			sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Goal Amount</th>");
			sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Current Amount Raised</th>");
			sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Status</th>");
			sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Created At</th>");
			sb.Append("</tr>");

			// Building the Data rows.
			foreach (string[] campaign in campaigns)
			{
				sb.Append("<tr>");
				foreach (string field in campaign)
				{
					sb.Append("<td style='border: 1px solid #ccc'>");
					sb.Append(field);
					sb.Append("</td>");
				}
				sb.Append("</tr>");
			}

			// Table end.
			sb.Append("</table>");

			using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString())))
			{
				using (MemoryStream byteArrayOutputStream = new MemoryStream())
				{
					PdfWriter writer = new PdfWriter(byteArrayOutputStream);
					PdfDocument pdfDocument = new PdfDocument(writer);
					pdfDocument.SetDefaultPageSize(PageSize.A4);
					HtmlConverter.ConvertToPdf(stream, pdfDocument);
					pdfDocument.Close();
					return File(byteArrayOutputStream.ToArray(), "application/pdf", "Campaigns.pdf");
				}
			}
		}
        public FileResult ExportCharityOrganizations()
        {
            // Fetching all charity organizations.
            List<object> organizations = (from organization in this._context.CharityOrganizations
                                          select new[] {
                                      organization.OrganizationName,
                                      organization.registration_status.ToString(),
                                      organization.Address,
                                      organization.Email,
                                  }).ToList<object>();

            // Building an HTML string.
            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc; font-family: Arial; font-size: 10pt;'>");

            // Building the Header row.
            sb.Append("<tr>");
            sb.Append("<th style='background-color: #B8DBFD; border: 1px solid #ccc'>Organization Name</th>");
            sb.Append("<th style='background-color: #B8DBFD; border: 1px solid #ccc'>Registration Status</th>");
            sb.Append("<th style='background-color: #B8DBFD; border: 1px solid #ccc'>Address</th>");
            sb.Append("<th style='background-color: #B8DBFD; border: 1px solid #ccc'>Email</th>");
            sb.Append("</tr>");

            // Building the Data rows.
            foreach (string[] organization in organizations)
            {
                sb.Append("<tr>");
                foreach (string field in organization)
                {
                    sb.Append("<td style='border: 1px solid #ccc'>");
                    sb.Append(field);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString())))
            {
                using (MemoryStream byteArrayOutputStream = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(byteArrayOutputStream);
                    PdfDocument pdfDocument = new PdfDocument(writer);
                    pdfDocument.SetDefaultPageSize(PageSize.A4);

                    // Convert HTML to PDF, this handles multi-page content.
                    HtmlConverter.ConvertToPdf(stream, pdfDocument);

                    pdfDocument.Close();
                    return File(byteArrayOutputStream.ToArray(), "application/pdf", "CharityOrganizations.pdf");
                }
            }
        }

    }
}