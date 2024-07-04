using Lost_And_Found.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Threading.Tasks;
using Lost_And_Found.Areas.Identity.Data;


namespace Lost_And_Found.Controllers
{
    public class HelpController : Controller
    {
        private readonly CoreProjectContext _context;

        public HelpController(CoreProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult HelpView(string userId, string userEmail)
        {
            var model = new Help
            {
                UserEmail = userEmail,
                UserId = userId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> HelpView(Help model)
        {
            Console.WriteLine("POST ReportView hit");
            if (ModelState.IsValid)
            {
                _context.Help.Add(model);
                await _context.SaveChangesAsync();

                var emailSent = await SendEmailAsync(model);
                if (emailSent)
                {
                    Console.WriteLine("Email sent successfully");
                    return RedirectToAction("LostItemList", "List");
                }
                else
                {
                    ModelState.AddModelError("", "There was an error sending the email.");
                }
            }
            else
            {
                Console.WriteLine("ModelState is not valid");
            }

            return View(model);
        }

        private async Task<bool> SendEmailAsync(Help model)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Happy News", "findyourlostitem@gmail.com"));
                message.To.Add(new MailboxAddress(model.Name, model.UserEmail));
                message.Subject = "Report Submission Confirmation";

                message.Body = new TextPart("plain")
                {
                    Text = $"Name: {model.Name}\nFindded Location: {model.Findded_Location}\nDate and Time: {model.DateTime}\nContact Information: {model.Contact_Information}"
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("findyourlostitem@gmail.com", "cxnu dxzg cnwr ktbd");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        public IActionResult Index()
        {
            // This action can return a view or perform other actions as needed
            return View();
        }
    }
}
