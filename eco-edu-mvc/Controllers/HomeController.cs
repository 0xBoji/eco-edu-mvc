using eco_edu_mvc.Models;
using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eco_edu_mvc.Controllers;
public class HomeController(EcoEduContext context) : Controller
{
    private readonly EcoEduContext _context = context;

    public IActionResult Index() => View();

    public IActionResult About() => View();

    public IActionResult Contact() => View();

    public IActionResult Survey() => View();

    public IActionResult Competition() => View();

    public IActionResult FAQ() => View();

    public IActionResult Seminar() => View();


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
