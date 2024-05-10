using Microsoft.AspNetCore.Mvc;

namespace eco_edu_mvc.Controllers;
public class accountsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
