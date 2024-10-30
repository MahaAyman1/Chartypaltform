using Chartypaltform.Models;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;
using System.IO;

namespace Chartypaltform.Services
{
    public class PDFService
    {
        public byte[] GenerateCampaignsPDF(List<Campaign> campaigns)
        {
            string pdfFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PDFs");

            // Create the folder if it doesn't exist
            if (!Directory.Exists(pdfFolderPath))
            {
                Directory.CreateDirectory(pdfFolderPath);
            }

            string pdfFilePath = Path.Combine(pdfFolderPath, "Campaigns.pdf");

            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    PdfWriter writer = new PdfWriter(stream);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);

                    // Add content to the PDF
                    document.Add(new Paragraph("Campaign List")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(20));

                    // Create a table
                    Table table = new Table(new float[] { 3, 2, 2, 2, 2 });
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    // Adding header cells
                    table.AddHeaderCell("Campaign Name");
                    table.AddHeaderCell("Description");
                    table.AddHeaderCell("Goal Amount");
                    table.AddHeaderCell("Created At");

                    // Adding data rows
                    foreach (var campaign in campaigns)
                    {
                        table.AddCell(campaign.CampaignName ?? "N/A");
                        table.AddCell(campaign.CampaignDes ?? "N/A");
                        table.AddCell(campaign.GoalAmount.ToString("C"));
                        table.AddCell(campaign.CreatedAt.ToShortDateString());
                    }

                    document.Add(table);
                    document.Close(); // Close the document

                    // Reset the stream position to the beginning
                    stream.Seek(0, SeekOrigin.Begin);

                    // Save the PDF to the PDFs folder
                    using (FileStream fileStream = new FileStream(pdfFilePath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }

                    return stream.ToArray(); // Optionally return the PDF file as a byte array
                }
                catch (PdfException pdfEx)
                {
                    throw new Exception("PDF Generation Error: " + pdfEx.Message, pdfEx);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error generating PDF: " + ex.Message, ex);
                }
            }
        }
    }
}
