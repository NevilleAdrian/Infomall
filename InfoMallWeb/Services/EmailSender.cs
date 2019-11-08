using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoMallWeb.Services
{
    //Mailing
	public class EmailSender : IEmailSender
	{
		private readonly ILogger<EmailSender> _logger;
		public EmailSender(ILogger<EmailSender> logger)
		{
			_logger = logger;
		}
		public async Task SendEmailAsync(string email, string subject, string message)
		{
			var mimeMessage = new MimeMessage();
			mimeMessage.From.Add(new MailboxAddress(
				"Mercy Chioma",
				"contact@infomall.ng"
			));
			mimeMessage.Subject = !string.IsNullOrEmpty(subject) ? subject : "MERRY CHRISTMAS";
			mimeMessage.Cc.Add(new MailboxAddress(email));
			mimeMessage.Bcc.Add(new MailboxAddress("Mercy Chioma", "cmercy@infomall.ng"));
			mimeMessage.Bcc.Add(new MailboxAddress("Chisom Nnadi", "cnnadi@infomall.ng"));
			mimeMessage.Body = !string.IsNullOrEmpty(message) ? new TextPart("plain") { Text = message } : new TextPart("plain")
			{
				//Text = "Dear Uche, \nCan you see this? If so, I sent it from my console application. \nRegards, \nNwisu."
				//Text = message
				//Text = "Dear Obinna, \nI call you 'Bi O' and Chika calls you 'Bina'."
				//Text = "Merry Christmas Ma, \nIf you can see this sms, I sent it from a C# application I built."
				Text = "Dear Victor, \nMerry Christmas and a prosperous new year."
			};

			using (var client = new SmtpClient())
			{
				client.Connect("infomall.ng", 25, SecureSocketOptions.None);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("contact@infomall.ng", "InfoMall01");
				await client.SendAsync(mimeMessage);
				_logger.LogInformation("message sent successfully...");
				await client.DisconnectAsync(true);
			}

		}

		public async Task SendEmailToAllAsync(List<string> emails, string subject, string message)
		{
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(
                "Mercy Chioma",
                "contact@infomall.ng"
            ));
            mimeMessage.Subject = !string.IsNullOrEmpty(subject) ? subject : "MERRY CHRISTMAS";
            mimeMessage.Bcc.Add(new MailboxAddress("Mercy Chioma", "cmercy@infomall.ng"));
            mimeMessage.Bcc.Add(new MailboxAddress("Chisom Nnadi", "cnnadi@infomall.ng"));

            foreach (string email in emails)
			{
                mimeMessage.Bcc.Add(new MailboxAddress(email));
			}
			
			//mimeMessage.Subject = "UCHE CAN YOU SEE THIS?";
			//mimeMessage.Subject = subject;
			mimeMessage.Subject = !string.IsNullOrEmpty(subject) ? subject : "MERRY CHRISTMAS";
			mimeMessage.Body = !string.IsNullOrEmpty(message) ? new TextPart("plain") { Text = message } : new TextPart("plain")
			{
				//Text = "Dear Uche, \nCan you see this? If so, I sent it from my console application. \nRegards, \nNwisu."
				//Text = message
				//Text = "Dear Obinna, \nI call you 'Bi O' and Chika calls you 'Bina'."
				//Text = "Merry Christmas Ma, \nIf you can see this sms, I sent it from a C# application I built."
				Text = "Dear Victor, \nMerry Christmas and a prosperous new year."
			};

			using (var client = new SmtpClient())
			{
                client.Connect("infomall.ng", 25, false);
                client.Authenticate("contact@infomall.ng", "InfoMall01");
                await client.SendAsync(mimeMessage);
                _logger.LogInformation("message sent successfully...");
                await client.DisconnectAsync(true);
            }
		}
	}
}
