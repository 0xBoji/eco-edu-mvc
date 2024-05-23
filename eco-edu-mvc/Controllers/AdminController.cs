using Microsoft.AspNetCore.Mvc;

namespace eco_edu_mvc.Controllers;
public class AdminController : Controller
{
    public ActionResult Index()
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            return View();
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }
}
