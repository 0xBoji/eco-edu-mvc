using eco_edu_mvc.Models;
using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.HomeViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace eco_edu_mvc.Controllers;
public class HomeController(EcoEduContext context) : Controller
{
    private readonly ILogger<CompetitionsController> _logger;
    private readonly EcoEduContext _context = context;

    public async Task<IActionResult> Index()
    {
        var surveys = await _context.Surveys.Where(s => s.Active == true && s.EndDate > DateTime.Now).OrderByDescending(s => s.CreateDate).Take(4).ToListAsync();
        var competitions = await _context.Competitions.Where(c => c.Active == true && c.EndDate > DateTime.Now).OrderByDescending(s => s.StartDate).Take(6).ToListAsync();

        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
        {
            return RedirectToAction("Login", "Account");
        }


        var model = new HomeModel
        {
            Surveys = surveys,
            Competitions = competitions,
            Username = HttpContext.Session.GetString("Username"),
            UserId = HttpContext.Session.GetString("UserId")
        };

        return View(model);
    }


    public async Task<IActionResult> Survey() => View(await _context.Surveys.ToListAsync());

    public async Task<IActionResult> SurveyDetail(int id)
    {
        if (HttpContext.Session.GetString("Is_Accept") == "True")
        {
            var survey = await _context.Surveys.FirstOrDefaultAsync(s => s.SurveyId == id);
            if (survey == null) return NotFound();

            return View(survey);
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    public async Task<IActionResult> Competition()
    {
        if (HttpContext.Session.GetString("Role") != "Student" || !string.Equals(HttpContext.Session.GetString("Is_Accept"), "true", StringComparison.OrdinalIgnoreCase))
        {
            TempData["StudentPermissionDenied"] = true;
            return RedirectToAction("Index", "Home");
        }

        var competitions = await _context.Competitions.Where(c => c.Active == true).Include(u => u.CompetitionEntries).ToListAsync();
        return View(competitions);
    }


    public async Task<IActionResult> CompetitionDetail(int id)
    {
        if (HttpContext.Session.GetString("Is_Accept") == "True")
        {
            var competition = await _context.Competitions.FirstOrDefaultAsync(c => c.CompetitionId == id);
            if (competition == null) return NotFound();

            return View(competition);
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    public ActionResult About() => View();

    public ActionResult Contact() => View();

    public ActionResult FAQ() => View();

    public ActionResult Seminar() => View();


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
