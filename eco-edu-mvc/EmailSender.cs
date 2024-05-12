using System.Net;
using System.Net.Mail;

namespace eco_edu_mvc;

public class EmailSender : IEmailSender
{
	public Task SendEmailAsync(string email, string subject, string message)
	{
		var mail = "greenworldz154@gmail.com";
		var password = "hehehihihaha";

		var client = new SmtpClient("smtp.gmail.com", 465)
		{
			EnableSsl = true,
			UseDefaultCredentials = false,
			Credentials = new NetworkCredential(mail, password)
		};
		return client.SendMailAsync(
			new MailMessage(from: mail,
							to: email,
							subject,
							message
							));
	}
}
