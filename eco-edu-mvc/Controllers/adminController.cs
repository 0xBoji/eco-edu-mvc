using Microsoft.AspNetCore.Mvc;

namespace eco_edu_mvc.Controllers;
public class AdminController : Controller
{
    public IActionResult Index() => View();
    
    public IActionResult Survey() => View();
}
