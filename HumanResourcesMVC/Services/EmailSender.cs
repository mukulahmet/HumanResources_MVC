
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace HumanResourcesMVC.Services
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var smtpSettings = _configuration.GetSection("Smtp");
			var host = smtpSettings["Host"];
			var port = int.Parse(smtpSettings["Port"]);
			var userName = smtpSettings["UserName"];
			var password = smtpSettings["Password"];

			using (var client = new SmtpClient(host, port))
			{
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(userName, password);
				client.EnableSsl = true;

				var mailMessage = new MailMessage
				{
					From = new MailAddress(userName),
					Subject = subject,
					Body = htmlMessage,
					IsBodyHtml = true,
				};
				mailMessage.To.Add(email);

				await client.SendMailAsync(mailMessage);
			}
		}
	}
}
