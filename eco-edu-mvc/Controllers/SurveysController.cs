using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.SurveyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Controllers;
public class SurveysController(EcoEduContext context) : Controller
{
    private readonly EcoEduContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            return View(await _context.Surveys.ToListAsync());
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    public ActionResult Post()
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            return View();
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpPost]
    public async Task<IActionResult> Post(SurveyModel model)
    {
        if (!ModelState.IsValid) return View(model);

        Survey survey = new()
        {
            Title = model.Title,
            Topic = model.Topic,
            CreateDate = DateTime.Now,
            EndDate = model.EndDate,
            TargetAudience = model.TargetAudience,
            Active = model.Active
        };
        _context.Surveys.Add(survey);
        await _context.SaveChangesAsync();

        return RedirectToAction("index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            var survey = await _context.Surveys.FindAsync(id);
            if (survey == null) return NotFound();

            SurveyModel model = new()
            {
                SurveyId = survey.SurveyId,
                Title = survey.Title,
                Topic = survey.Topic,
                CreateDate = survey.CreateDate,
                EndDate = survey.EndDate,
                TargetAudience = survey.TargetAudience,
                Active = survey.Active
            };
            return View(model);
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, SurveyModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var survey = await _context.Surveys.FindAsync(id);
        if (survey == null) return NotFound();

        try
        {
            survey.Title = model.Title;
            survey.Topic = model.Topic;
            survey.EndDate = model.EndDate;
            survey.TargetAudience = model.TargetAudience;
            survey.Active = model.Active;

            _context.Surveys.Update(survey);
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
