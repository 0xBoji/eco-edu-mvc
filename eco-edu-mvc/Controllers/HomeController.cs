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

    public IActionResult GroupChat()
    {
        return View();
    }

    public IActionResult CreateGroupChat()
    {
        return View();
    }

    public async Task<IActionResult> Survey()
    {
        if (HttpContext.Session.GetString("Is_Accept") == "true")
        {
            var role = HttpContext.Session.GetString("Role");
            var surveys = await _context.Surveys.Where(s => (s.Active == true && s.TargetAudience == "Both") ||
                (s.Active == true && s.TargetAudience.StartsWith("Both")) ||
                (s.Active == true && s.TargetAudience.StartsWith("Staff") == role.StartsWith("Staff")) ||
                (s.Active == true && s.TargetAudience.StartsWith("Student") == role.StartsWith("Student")))
                .OrderByDescending(s => s.CreateDate).ToListAsync();

            return View(surveys);
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    public async Task<IActionResult> SurveyDetail(int id)
    {
        if (HttpContext.Session.GetString("Is_Accept") == "true")
        {
            var survey = await _context.Surveys.Include(s => s.Questions).FirstOrDefaultAsync(s => s.SurveyId == id);
            SurveyDetailModel model = new()
            {
                Survey = survey,
                Questions = [.. survey.Questions]
            };
            return View(model);
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpPost]
    public async Task<IActionResult> SubmitDetail(SurveyDetailModel model)
    {
        var id = int.Parse(HttpContext.Session.GetString("UserId"));

        foreach (var question in model.Questions)
        {
            Response rep = new()
            {
                UserId = id,
                QuestionId = question.QuestionId,
                Answer = question.SelectedAnswer
            };
            _context.Responses.Add(rep);
        }
        await _context.SaveChangesAsync();

        TempData["SubmissionSuccess"] = "Your responses have been submitted successfully!";
        return RedirectToAction("Index", "Home");
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

    // GET: Home/JoinSeminar/5
    public async Task<IActionResult> JoinSeminar(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var seminar = await _context.Seminars.FirstOrDefaultAsync(m => m.SeminarId == id);
        if (seminar == null)
        {
            return NotFound();
        }

        return View(seminar);
    }

    [HttpPost]
    public async Task<IActionResult> JoinSeminar(int id)
    {
        if (HttpContext.Session.GetString("Role") != "Student" || !string.Equals(HttpContext.Session.GetString("Is_Accept"), "true", StringComparison.OrdinalIgnoreCase))
        {
            TempData["StudentPermissionDenied"] = true;
            return RedirectToAction("Index");
        }

        var seminar = await _context.Seminars.FirstOrDefaultAsync(m => m.SeminarId == id);
        if (seminar == null)
        {
            return NotFound();
        }

        var userId = int.Parse(HttpContext.Session.GetString("UserId"));

        var seminarMember = new SeminarMember
        {
            UserId = userId,
            SeminarId = seminar.SeminarId
        };

        _context.SeminarMembers.Add(seminarMember);
        await _context.SaveChangesAsync();

        return RedirectToAction("Seminar");
    }

    public ActionResult About() => View();

    public ActionResult Contact() => View();

    public async Task<IActionResult> FAQs() => View(await _context.Faqs.ToListAsync());
    

    public async Task<IActionResult> Seminar() => View(await _context.Seminars
                                     .Include(s => s.Sm)
                                     .ThenInclude(sm => sm.User)
                                     .ToListAsync());


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
