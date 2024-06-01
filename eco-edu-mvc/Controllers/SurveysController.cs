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
        if (HttpContext.Session.GetString("Role") == "Admin") return View();
        
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpPost]
    public async Task<IActionResult> Post(SurveyModel model, IFormFile file)
    {
        if(!ModelState.IsValid) return View(model);
        if (model.EndDate<DateTime.Now)
        {
            ModelState.AddModelError("EndDate", "Invalid Date");
            return View(model);
        }
        Survey survey = new()
        {
            Title = model.Title,
            Topic = model.Topic,
            CreateDate = DateTime.Now,
            EndDate = model.EndDate,
            TargetAudience = model.TargetAudience,
            Active = model.Active
        };

        // Catch EndDate
        if (model.EndDate < DateTime.Now)
        {
            ModelState.AddModelError("EndDate", "EndDate must be in the future!");
            return View(model);
        }

        if (file != null && file.Length > 0)
        {
            var fileName = DateTime.Now.Ticks + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                survey.Images = fileName;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Images", "File upload failed: " + ex.Message);
                return View(model);
            }
        }
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
    public async Task<IActionResult> Update(int id, SurveyModel model, IFormFile file)
    {
        if (!ModelState.IsValid) return View(model);

        var survey = await _context.Surveys.FindAsync(id);
        if (survey == null) return NotFound();

        if (model.EndDate < DateTime.Now)
        {
            ModelState.AddModelError("EndDate", "Invalid Date");
            return View(model);
        }

        try
        {
            survey.Title = model.Title;
            survey.Topic = model.Topic;
            survey.TargetAudience = model.TargetAudience;
            survey.Active = model.Active;

            // Catch EndDate
            if ((survey.EndDate = model.EndDate) < DateTime.Now)
            {
                ModelState.AddModelError("EndDate", "EndDate must be in the future!");
                return View(model);
            }

            if (file != null && file.Length > 0)
            {
                var fileName = DateTime.Now.Ticks + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);
                try
                {
                    var directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    if (!string.IsNullOrEmpty(survey.Images))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", survey.Images);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    survey.Images = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Images", "File upload failed: " + ex.Message);
                    return View(model);
                }
            }
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