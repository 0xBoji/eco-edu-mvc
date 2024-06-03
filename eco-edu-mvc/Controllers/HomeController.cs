using eco_edu_mvc.Models;
using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.HomeViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace eco_edu_mvc.Controllers;
public class HomeController(EcoEduContext context) : Controller
{
    private readonly EcoEduContext _context = context;

    public async Task<IActionResult> Index()
    {
        var surveys = await _context.Surveys.Where(s => s.Active == true).OrderByDescending(s => s.CreateDate).Take(4).ToListAsync();

        var competitions = await _context.Competitions.Where(c => c.Active == true).OrderByDescending(s => s.StartDate).Take(6).ToListAsync();

        var winners = await _context.GradeTests.Include(g => g.Entry).ThenInclude(e => e.User).OrderByDescending(w => w.Score).ToListAsync();

        var topWinner = winners.FirstOrDefault();
        var nextWinners = winners.Skip(1).Take(3).ToList();

        HomeModel model = new()
        {
            Surveys = surveys,
            Competitions = competitions,
            TopWinner = topWinner,
            NextWinners = nextWinners
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
    public async Task<IActionResult> Competition() => View(await _context.Competitions.ToListAsync());

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
