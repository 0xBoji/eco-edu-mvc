using eco_edu_mvc.Models;
using eco_edu_mvc.Models.AccountsViewModel;
using eco_edu_mvc.Models.ContactViewModel;
using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.HomeViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Linq;

namespace eco_edu_mvc.Controllers;
public class HomeController : Controller
{
    private readonly EcoEduContext _context;
    private readonly IEmailSender emailSender;

    public HomeController(EcoEduContext context, IEmailSender emailSender)
    {
        _context = context;
        this.emailSender = emailSender;
    }

    [HttpGet]
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

    [HttpGet]
    public async Task<IActionResult> Survey()
    {
        if (HttpContext.Session.GetString("Is_Accept") == "true")
        {
            var role = HttpContext.Session.GetString("Role");
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));
			var surveys = await _context.Surveys.Where(s => (s.Active == true && s.TargetAudience == "Both") ||
                (s.Active == true && s.TargetAudience.StartsWith("Both")) ||
                (s.Active == true && s.TargetAudience.StartsWith("Staff") == role.StartsWith("Staff")) ||
                (s.Active == true && s.TargetAudience.StartsWith("Student") == role.StartsWith("Student")))
                .OrderByDescending(s => s.CreateDate).Where(s => s.Questions.Any()).ToListAsync();

			var participatedSurveyIds = await _context.Responses
			.Where(r => r.UserId == userId)
			.Select(r => r.Question.SurveyId)
			.Distinct()
			.ToListAsync();

			ViewBag.ParticipatedSurveyIds = participatedSurveyIds;

            return View(surveys);
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpGet]
    public async Task<IActionResult> SurveyDetail(int id)
    {
        if (HttpContext.Session.GetString("Is_Accept") == "true")
        {
            var survey = await _context.Surveys.Include(s => s.Questions).FirstOrDefaultAsync(s => s.SurveyId == id);

            // Randomize answers for each question
            foreach (var question in survey.Questions)
            {
                List<string> answers = [];
                if (!string.IsNullOrEmpty(question.Answer1)) answers.Add(question.Answer1);
                if (!string.IsNullOrEmpty(question.Answer2)) answers.Add(question.Answer2);
                if (!string.IsNullOrEmpty(question.Answer3)) answers.Add(question.Answer3);
                if (!string.IsNullOrEmpty(question.CorrectAnswer)) answers.Add(question.CorrectAnswer);

                answers = [.. answers.OrderBy(a => Guid.NewGuid())];

                // Reassign shuffled answers
                question.Answer1 = answers.Count > 0 ? answers[0] : null;
                question.Answer2 = answers.Count > 1 ? answers[1] : null;
                question.Answer3 = answers.Count > 2 ? answers[2] : null;
                question.CorrectAnswer = answers.Count > 3 ? answers[3] : null;
            }

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

        var seminar = await _context.Seminars.Include(s => s.SeminarMembers).FirstOrDefaultAsync(m => m.SeminarId == id);
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

        var seminar = await _context.Seminars.Include(s => s.SeminarMembers).FirstOrDefaultAsync(m => m.SeminarId == id);
        if (seminar == null)
        {
            return NotFound();
        }

        var userId = int.Parse(HttpContext.Session.GetString("UserId"));

        if (seminar.SeminarMembers.Any(sm => sm.UserId == userId))
        {
            TempData["AlreadyJoined"] = "You have already joined this seminar!";
            return RedirectToAction("Seminar");
        }

        if (seminar.SeminarMembers.Count >= seminar.Participants)
        {
            TempData["SeminarFull"] = "The seminar is already full!";
            return RedirectToAction("Seminar");
        }

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

    public IActionResult Chat()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            TempData["LoginRequired"] = "You must be logged in to access the chat.";
            return RedirectToAction("Index");
        }

        return View();
    }


    public async Task<IActionResult> Seminar()
    {
        var seminars = await _context.Seminars
                                      .Include(s => s.SeminarMembers)
                                      .ThenInclude(sm => sm.User)
                                      .ToListAsync();
        return View(seminars);
    }


    private string GenerateVerificationCode()
    {
        return Guid.NewGuid().ToString().Substring(0, 6);
    }

    [HttpPost]
    public async Task<IActionResult> SendVerificationCode(ContactModel model)
    {
        var verificationCode = GenerateVerificationCode();

        var receiver = model.Email;
        var subject = "Verification Code confirm Email";
        var message = verificationCode;

        HttpContext.Session.SetString("code", verificationCode);
        HttpContext.Session.SetString("name", model.FullName);
        HttpContext.Session.SetString("mail", model.Email);
        HttpContext.Session.SetString("Message", model.Message);


        await emailSender.SendEmailAsync(receiver, subject, message);
        return RedirectToAction("CheckVerificationCode", "Home");

    }

    public IActionResult CheckVerificationCode() => View();


    [HttpPost]
    public async Task<IActionResult> CheckVerificationCode(CheckOnlyCode model)
    {
        string code = HttpContext.Session.GetString("code");
        if(model.code == code)
        {
            var receiver = "rin04082004@gmail.com";
            var subject = HttpContext.Session.GetString("name") + " " + HttpContext.Session.GetString("mail") + " Contact";
            var message = HttpContext.Session.GetString("Message");

            await emailSender.SendEmailAsync(receiver, subject, message);
            TempData["SendEmail"] = false;
            return RedirectToAction("Contact", "home");
        }
        ModelState.AddModelError("code", "Invalid Code!");
        return View(model);
    }

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
