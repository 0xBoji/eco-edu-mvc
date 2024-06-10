using eco_edu_mvc.Models;
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
    public async Task<IActionResult> Post(SurveyModel model, IFormFile file)
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

        // Catch EndDate
        if (model.EndDate < DateTime.Now)
        {
            ModelState.AddModelError("EndDate", "EndDate must be in the future!");
            return View(model);
        }

        // Catch image files
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

            // Catch image files
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

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            try
            {
                var survey = await _context.Surveys.FindAsync(id);
                if (survey == null) return NotFound();

                // Check to delete relationship
                var response = _context.Responses.Where(r => r.QuestionId == id);
                _context.Responses.RemoveRange(response);

                var question = _context.Questions.Where(q => q.SurveyId == id);
                _context.Questions.RemoveRange(question);

                _context.Surveys.Remove(survey);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Delete", "Failed to delete survey: " + ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("Index", "Home");
    }

    //[HttpPost]
    //public async Task<IActionResult> RemoveUser(int id)
    //{
    //    var user = await _context.Users.FindAsync(id);
    //    if (user == null) return NotFound();

    //    //check and delete the relationship
    //    var competitionEntries = context.CompetitionEntries.Where(ce => ce.UserId == id);
    //    context.CompetitionEntries.RemoveRange(competitionEntries);
    //    var response = context.Responses.Where(rs => rs.UserId == id);
    //    context.Responses.RemoveRange(response);
    //    var seminars = context.SeminarMembers.Where(s => s.UserId == id);
    //    context.SeminarMembers.RemoveRange(seminars);

    //    context.Users.Remove(user);
    //    await context.SaveChangesAsync();
    //    return RedirectToAction("RequestShow");
    //}
}