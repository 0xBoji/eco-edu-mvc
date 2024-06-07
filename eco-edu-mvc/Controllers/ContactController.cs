using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Mail;
using MimeKit;
using eco_edu_mvc.Models.ContactViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
    public async Task<IActionResult> SendContact(ContactModel model,string code)
    {
        string ve = HttpContext.Session.GetString("code");
        if (code == null)
        {
            TempData["SendEmailError"] = false;
            return RedirectToAction("Contact", "Contact");
        }
        else if (ve != code)
        {
            TempData["SendEmailError"] = false;
            return RedirectToAction("Contact", "Contact");
        }
        else
        {
            var receiver = "rin04082004@gmail.com";
            var subject = HttpContext.Session.GetString("name") + " " + HttpContext.Session.GetString("mail") + " Contact";
            var message = HttpContext.Session.GetString("Message");

            await emailSender.SendEmailAsync(receiver, subject, message);
            TempData["SendEmail"] = false;
            return RedirectToAction("Contact", "Contact");

        }
        
    }
    private string GenerateVerificationCode()
    {
        return Guid.NewGuid().ToString().Substring(0, 6);
    }

    public IActionResult CheckVe()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CheckVe(string Code,ContactModel model) 
    {
        var verificationCode = GenerateVerificationCode();

        var receiver = model.Email;
        var subject = "Verification Code confirm Email";
        var message = verificationCode;
        HttpContext.Session.SetString("code", verificationCode);
        HttpContext.Session.SetString("name", model.FullName);
        HttpContext.Session.SetString("mail", model.Email);
        HttpContext.Session.SetString("Message", model.Message);


        await emailSender.SendEmailAsync(receiver, subject, message);
        return RedirectToAction("CheckVe", "Contact");
        
    }
    



}
