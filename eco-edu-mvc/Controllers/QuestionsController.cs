using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.SurveyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Controllers;
public class QuestionsController(EcoEduContext context) : Controller
{
    private readonly EcoEduContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            return View(await _context.Questions.Include(q => q.Survey).ToListAsync());
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpGet]
    public async Task<IActionResult> Post()
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            var surveys = await _context.Surveys.ToListAsync();
            QuestionModel model = new()
            {
                Surveys = surveys.Select(s => new SelectListItem
                {
                    Value = s.SurveyId.ToString(),
                    Text = s.Title
                }).ToList()
            };
            return View(model);
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpPost]
    public async Task<IActionResult> Post(QuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Surveys = await _context.Surveys.Select(s => new SelectListItem
            {
                Value = s.SurveyId.ToString(),
                Text = s.Title
            }).ToListAsync();
            return View(model);
        }

        Question quest = new()
        {
            SurveyId = model.SurveyId,
            QuestionText = model.QuestionText,
            QuestionType = model.QuestionType,
            Answer1 = model.Answer1,
            Answer2 = model.Answer2,
            Answer3 = model.Answer3,
            CorrectAnswer = model.CorrectAnswer
        };
        _context.Questions.Add(quest);
        await _context.SaveChangesAsync();

        return RedirectToAction("index");
    }

    // Controller
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            var quest = await _context.Questions.Include(q => q.Survey).FirstOrDefaultAsync(q => q.QuestionId == id);
            if (quest == null) return NotFound();

            var surveys = await _context.Surveys.ToListAsync();
            QuestionModel model = new()
            {
                QuestionId = quest.QuestionId,
                SurveyId = quest.SurveyId,
                QuestionText = quest.QuestionText,
                QuestionType = quest.QuestionType,
                Answer1 = quest.Answer1,
                Answer2 = quest.Answer2,
                Answer3 = quest.Answer3,
                CorrectAnswer = quest.CorrectAnswer,
                Surveys = surveys.Select(s => new SelectListItem
                {
                    Value = s.SurveyId.ToString(),
                    Text = s.Title
                }).ToList()
            };
            return View(model);
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, QuestionModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var quest = await _context.Questions.FindAsync(id);
        if (quest == null) return NotFound();

        try
        {
            quest.SurveyId = model.SurveyId;
            quest.QuestionText = model.QuestionText;
            quest.QuestionType = model.QuestionType;
            quest.Answer1 = model.Answer1;
            quest.Answer2 = model.Answer2;
            quest.Answer3 = model.Answer3;
            quest.CorrectAnswer = model.CorrectAnswer;

            _context.Update(quest);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
        }
        return View(model);
    }

}
