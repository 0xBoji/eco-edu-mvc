using Microsoft.AspNetCore.Mvc;

namespace eco_edu_mvc.Controllers;
public class ContactController : Controller
{
	public IActionResult Contact()
	{
		return View();
	}
}
