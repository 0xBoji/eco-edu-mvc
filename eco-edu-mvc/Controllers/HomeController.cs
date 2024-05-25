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
		var surveys = await _context.Surveys.Where(s=> s.Active == true).OrderByDescending(s => s.CreateDate).Take(4).ToListAsync();
        var competitions = await _context.Competitions.ToListAsync();

		var model = new HomeModel
		{
			Surveys = surveys,
			Competitions = competitions
		};

        return View(model);
	}

    public ActionResult About() => View();

    public ActionResult Contact() => View();

    public async Task<IActionResult> Survey()
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            return View(await _context.Surveys.ToListAsync());
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }
 
    public async Task<IActionResult> SurveyDetail(int id)
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
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
        var competitions = await _context.Competitions.ToListAsync();
        return View(competitions);
    }
    public async Task<IActionResult> CompetitionDetail(int id)
    {
        var competition = await _context.Competitions.FirstOrDefaultAsync(c => c.CompetitionId == id);
        if (competition == null) return NotFound();

        return View(competition);
    }

    public ActionResult FAQ() => View();

    public ActionResult Seminar() => View();


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
