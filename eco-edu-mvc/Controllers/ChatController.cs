using Microsoft.AspNetCore.Mvc;

public class ChatController : Controller
{
    public IActionResult Index()
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
        {
            return RedirectToAction("Login", "Account");
        }

        return View();
    }
}
