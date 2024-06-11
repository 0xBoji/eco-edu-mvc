using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.SurveyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace eco_edu_mvc.Controllers;
public class QuestionsController(EcoEduContext context) : Controller
{
    private readonly EcoEduContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Index(int? page, string filter)
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            // Pagination
            int pageSize = 10;
            int pageNumber = page ?? 1;

            IQueryable<Question> questions = _context.Questions.Include(q => q.Survey);
            if (!string.IsNullOrEmpty(filter))
            {
                questions = questions.Where(q => q.QuestionType == filter);
            }

            return View(await _context.Questions.Include(q => q.Survey).ToPagedListAsync(pageNumber, pageSize));
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpGet]
    public async Task<IActionResult> Post(int id, QuestionModel model)
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            var sv = await _context.Surveys.FindAsync(id);
            if (sv == null) return NotFound();

            model.SurveyId = sv.SurveyId;
            model.Title = sv.Title;

            return View(model);
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpPost]
    public async Task<IActionResult> Post(QuestionModel model)
    {
        if (ModelState.IsValid)
        {
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

            return RedirectToAction("index", "surveys");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            var quest = await _context.Questions.Include(q => q.Survey).FirstOrDefaultAsync(q => q.QuestionId == id);
            if (quest == null) return NotFound();

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

                Title = quest.Survey.Title
            };
            return View(model);
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, QuestionModel model)
    {
        if (ModelState.IsValid)
        {
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
        return View(model);
    }

}
