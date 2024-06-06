using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using eco_edu_mvc.Models.ContactViewModel;

namespace eco_edu_mvc.Controllers;
public class ContactController : Controller

{
    private readonly IEmailSender emailSender;
   
    public ContactController( IEmailSender emailSender)
    {
        
        this.emailSender = emailSender;

    }
    
    public IActionResult Contact()
	{
		return View();
	}
    [HttpPost]
    public async Task<IActionResult> SendContact(ContactModel model)
    {
       
        var receiver = "rin04082004@gmail.com";
        var subject = model.FullName +" "+model.Email;
        var message = model.Message;

        await emailSender.SendEmailAsync(receiver, subject, message);
        return RedirectToAction("Contact", "Contact");
    }


    
}
