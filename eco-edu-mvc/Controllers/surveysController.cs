using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.SurveyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Controllers;
public class SurveyController(EcoEduContext context) : Controller
{
	private readonly EcoEduContext _context = context;

	[HttpGet]
	public async Task<ActionResult> List() => View(await _context.Surveys.ToListAsync());

	[HttpGet]
	public async Task<ActionResult> Get(int id)
	{
		var survey = await _context.Surveys.FirstOrDefaultAsync(p => p.SurveyId == id);
		if (survey == null) return NotFound();

		return View(survey);
	}

	public ActionResult Post() => View();

	[HttpPost]
	public async Task<ActionResult> Post(SurveyModel model)
	{
		if (!ModelState.IsValid) return View(model);

		Survey survey = new()
		{
			Title = model.Title,
			Topic = model.Topic,
			CreatedBy = model.CreatedBy,
			CreateDate = DateTime.UtcNow,
			EndDate = model.EndDate,
			TargetAudience = model.TargetAudience,
			Active = model.Active
		};
		_context.Surveys.Add(survey);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(List), new { id = survey.SurveyId }, survey);
	}

	public async Task<ActionResult> Update(int id)
	{
		var survey = await _context.Surveys.FindAsync(id);
		if (survey == null) return NotFound();

		SurveyModel model = new()
		{
			SurveyId = survey.SurveyId,
			Title = survey.Title,
			Topic = survey.Topic,
			CreatedBy = survey.CreatedBy,
			CreateDate = survey.CreateDate,
			EndDate = survey.EndDate,
			TargetAudience = survey.TargetAudience,
			Active = survey.Active
		};
		return View(model);
	}

	[HttpPost]
	public async Task<ActionResult> UpdateSurvey(int id, SurveyModel model)
	{
		if (id != model.SurveyId) return NotFound();

		if (!ModelState.IsValid) return View(model);

		var survey = await _context.Surveys.FindAsync(id);
		if (survey == null) return NotFound();

		try
		{
			survey.Title = model.Title;
			survey.Topic = model.Topic;
			survey.CreateDate = model.CreateDate;
			survey.EndDate = model.EndDate;
			survey.TargetAudience = model.TargetAudience;
			survey.Active = model.Active;

			_context.Surveys.Update(survey);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(List));
		}
		catch (Exception ex)
		{
			ModelState.AddModelError("Error", ex.Message);
		}
		return View(model);
	}

	public async Task<IActionResult> Delete(int id)
	{
		var survey = await _context.Surveys.FindAsync(id);
		if (survey == null) return NotFound();

		return View(survey);
	}

	[HttpPost, ActionName("Delete")]
	public async Task<ActionResult> DeleteConfirm(int id)
	{
		var survey = await _context.Surveys.FindAsync(id);
		if (survey == null) return NotFound();

		_context.Surveys.Remove(survey);
		await _context.SaveChangesAsync();

		return RedirectToAction(nameof(List));
	}
}
