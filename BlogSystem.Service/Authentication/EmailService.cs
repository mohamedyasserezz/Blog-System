﻿using BlogSystem.Service.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BlogSystem.Service.Authentication
{
	public class EmailService : IEmailSender
	{
		private readonly MailSettings _maillSettins;
		private readonly ILogger<EmailService> _logger;
        public EmailService(IOptions<MailSettings> maillSettins, ILogger<EmailService> logger)
        {
			_maillSettins = maillSettins.Value;
			_logger = logger;

		}
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var message = new MimeMessage
			{
				Sender = MailboxAddress.Parse(_maillSettins.Mail),
				Subject = subject
			};
			message.To.Add(MailboxAddress.Parse(email));

			var builder = new BodyBuilder
			{
				HtmlBody = htmlMessage
			};

			message.Body = builder.ToMessageBody();

			using var smtp = new SmtpClient();

			_logger.LogInformation("Sending email to {email}", email);

			smtp.Connect(_maillSettins.Host, _maillSettins.Port, SecureSocketOptions.StartTls);
			smtp.Authenticate(_maillSettins.Mail, _maillSettins.Password);
			await smtp.SendAsync(message);
			smtp.Disconnect(true);
		}
	}
}
