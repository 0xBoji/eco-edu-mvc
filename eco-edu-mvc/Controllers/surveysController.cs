using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.SurveysViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eco_edu_mvc.Controllers;
public class surveysController(EcoEduContext context) : Controller
{
	private readonly EcoEduContext _context = context;

	[HttpGet]
	public async Task<ActionResult> List() => View(await _context.Surveys.ToListAsync());

	[HttpGet]
	public async Task<ActionResult> Get(int id)
	{
		if (string.IsNullOrEmpty(HttpContext.Session.GetString("Role"))) return Unauthorized("You dont have the permission to see this content");

		var sv = await _context.Surveys.FirstOrDefaultAsync(p => p.SurveyId == id);
		if (sv != null) return View(sv);

		return NotFound();
	}

	public ActionResult Post() => View();

	[HttpPost]
	public async Task<ActionResult> Post(SurveyModel model)
	{
		if (string.IsNullOrEmpty(HttpContext.Session.GetString("Role"))) return Unauthorized("You dont have the permission to see this content");

		if (ModelState.IsValid)
		{
			Survey sv = new()
			{
				Title = model.Title,
				Topic = model.Topic,
				CreatedBy = model.CreatedBy,
				CreateDate = DateTime.UtcNow,
				EndDate = model.EndDate,
				TargetAudience = model.TargetAudience,
				Active = model.Active
			};
			_context.Surveys.Add(sv);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(List), new { id = sv.SurveyId }, sv);
		}
		return RedirectToAction("Login", "Account");
	}

	public async Task<ActionResult> Update(int id, SurveyModel model)
	{
		if (string.IsNullOrEmpty(HttpContext.Session.GetString("Admin"))) return Unauthorized("You dont have the permission to see this content");

		var sv = await _context.Surveys.FindAsync(id);
		if (sv == null) return NotFound();

		model.Title = sv.Title;
		model.Topic = sv.Topic;
		model.CreatedBy = sv.CreatedBy;
		model.CreateDate = sv.CreateDate;
		model.EndDate = sv.EndDate;
		model.TargetAudience = sv.TargetAudience;
		model.Active = sv.Active;

		return View(sv);
	}

	[HttpPost]
	public async Task<ActionResult> UpdateSurvey(int id, SurveyModel model)
	{
		if (string.IsNullOrEmpty(HttpContext.Session.GetString("Role"))) return Unauthorized("You dont have the permission to see this content");
		var surveyId = HttpContext.Session.GetString("SurveyId");
		if (surveyId == null) return NotFound();

		if (!ModelState.IsValid) return NotFound();


		var sv = await _context.Surveys.FindAsync(id);
		if (sv == null) return NotFound();

		sv.Title = model.Title;
		sv.Topic = model.Topic;
		sv.CreatedBy = model.CreatedBy;
		sv.CreateDate = model.CreateDate;
		sv.EndDate = model.EndDate;
		sv.TargetAudience = model.TargetAudience;
		sv.Active = model.Active;
		try
		{
			_context.Update(sv);
			await _context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			ModelState.AddModelError("Error", ex.Message);
		}
		return View();
	}

	public async Task<ActionResult> Delete(int? id)
	{
		if (id == null) return NotFound();

		var sv = await _context.Surveys.FirstOrDefaultAsync(d => d.SurveyId == id);
		if (sv == null) return NotFound();

		return View(sv);
	}

	[HttpPost, ActionName("DeleteConfirmed")]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var sv = await _context.Surveys.FindAsync(id);
		if (sv != null)
		{
			_context.Surveys.Remove(sv);
			await _context.SaveChangesAsync();
		}
		return RedirectToAction(nameof(List));
	}
}
