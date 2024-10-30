using Chartypaltform.Data;
using Chartypaltform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;

namespace Chartypaltform.Controllers
{
    public class DonationController : Controller
    {
        private string PaypalClientId { get; set; } = "";
        private string PaypalSecret { get; set; } = "";
        private string PaypalUrl { get; set; } = "";
        private readonly ApplicationDbContext _context;

        public DonationController(IConfiguration configuration, ApplicationDbContext context)
        {
            PaypalClientId = configuration["PayPalSettings:ClientId"]!;
            PaypalSecret = configuration["PayPalSettings:Secret"]!;
            PaypalUrl = configuration["PayPalSettings:Url"]!;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.PaypalClientId = PaypalClientId;
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var campaign = await _context.Campaigns.FirstOrDefaultAsync(o => o.CampaignId == id);
            if (campaign == null)
            {
                return NotFound();
            }
            ViewBag.PaypalClientId = PaypalClientId;
            return View(campaign);
        }

        private async Task<string> GetPaypalAccessToken()
        {
            using (var client = new HttpClient())
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{PaypalClientId}:{PaypalSecret}"));
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials64}");
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{PaypalUrl}/v1/oauth2/token");
                requestMessage.Content = new StringContent("grant_type=client_credentials", null, "application/x-www-form-urlencoded");
                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    return jsonResponse?["access_token"]?.ToString() ?? string.Empty;
                }
            }
            return string.Empty;
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrder([FromBody] JsonObject data)
        {
            var totalAmount = data?["amount"]?.ToString();
            if (string.IsNullOrEmpty(totalAmount))
            {
                return new JsonResult(new { Id = string.Empty });
            }

            var createOrderRequest = new JsonObject
            {
                ["intent"] = "CAPTURE",
                ["purchase_units"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["amount"] = new JsonObject
                        {
                            ["currency_code"] = "USD",
                            ["value"] = totalAmount
                        }
                    }
                }
            };

            string accessToken = await GetPaypalAccessToken();
            var url = $"{PaypalUrl}/v2/checkout/orders";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(createOrderRequest.ToString(), null, "application/json")
                };

                var httpResponse = await client.SendAsync(requestMessage);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    string paypalOrderId = jsonResponse?["id"]?.ToString() ?? string.Empty;
                    return new JsonResult(new { Id = paypalOrderId });
                }
            }
            return new JsonResult(new { Id = string.Empty });
        }

          [HttpPost]
          public async Task<JsonResult> CompleteOrder([FromBody] JsonObject data)
          {
              var orderId = data?["orderID"]?.ToString();
              if (string.IsNullOrEmpty(orderId))
              {
                  return new JsonResult("error");
              }

              string accessToken = await GetPaypalAccessToken();
              var url = $"{PaypalUrl}/v2/checkout/orders/{orderId}/capture";

              using (var client = new HttpClient())
              {
                  client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                  var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                  {
                      Content = new StringContent("", null, "application/json")
                  };

                  var httpResponse = await client.SendAsync(requestMessage);
                  if (httpResponse.IsSuccessStatusCode)
                  {
                      var strResponse = await httpResponse.Content.ReadAsStringAsync();
                      var jsonResponse = JsonNode.Parse(strResponse);
                      string paypalOrderStatus = jsonResponse?["status"]?.ToString() ?? string.Empty;

                      if (paypalOrderStatus == "COMPLETED")
                      {
                          // Assume you pass the CampaignId and DonorId with the request
                          var campaignId = data["campaignId"]?.ToString();
                          var donorId = data["donorId"]?.ToString();
                          var amount = decimal.Parse(data["amount"]?.ToString() ?? "0");

                          // Create a donation record
                          Donation donation = new Donation
                          {
                              Amount = amount,
                              DonorId = donorId,
                              CampaignId = int.Parse(campaignId ?? "0")
                          };

                          await _context.donations.AddAsync(donation);
                          var campaign = await _context.Campaigns.FindAsync(donation.CampaignId);
                          if (campaign != null)
                          {
                              campaign.CurrentAmountRaised += donation.Amount;
                              _context.Campaigns.Update(campaign);
                          }

                          await _context.SaveChangesAsync();
                          return new JsonResult("success");
                      }
                  }
                  else if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
                  {
                      return new JsonResult("Error: Invalid order ID or other request data issue.");
                  }
                  else
                  {
                      return new JsonResult($"Error: Unexpected response from PayPal. Status code: {httpResponse.StatusCode}");
                  }
              }
              return new JsonResult("error");
          }


    

      


    }
}
