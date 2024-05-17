using System.Net;
using System.Net.Mail;

namespace eco_edu_mvc;

public class EmailSender : IEmailSender
{
	public async Task SendEmailAsync(string email, string subject, string message)
	{
		var mail = "hieuminh091304@gmail.com";
		var password = "trai tfkm oprl ulog";

		try
		{
			var client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(mail, password)
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(mail),
				Subject = subject,
				Body = message,
				IsBodyHtml = true
			};

			mailMessage.To.Add(email);

			await client.SendMailAsync(mailMessage);
		}
		catch (SmtpException ex)
		{
			// Log lỗi chi tiết
			Console.WriteLine($"SMTP Exception: {ex.Message}");
			throw;
		}
		catch (Exception ex)
		{
			// Log lỗi chi tiết
			Console.WriteLine($"General Exception: {ex.Message}");
			throw;
		}
	}
}
