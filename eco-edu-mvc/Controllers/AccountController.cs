using Microsoft.AspNetCore.Mvc;

namespace eco_edu_mvc.Controllers;
public class AccountController : Controller
{
	public IActionResult Login() => View();
}
