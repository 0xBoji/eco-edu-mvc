using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.SurveyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Controllers;
public class surveyController(EcoEduContext context) : Controller
{
	private readonly EcoEduContext _context = context;

	[HttpGet]
	public async Task<ActionResult> List() => View(await _context.Surveys.ToListAsync());

	[HttpGet]
	public async Task<ActionResult> Get(int id)
	{
		var sv = await _context.Surveys.FirstOrDefaultAsync(p => p.SurveyId == id);
		if (sv != null) return View(sv);

		return NotFound();
	}

	public ActionResult Post() => View();

	[HttpPost]
	public async Task<ActionResult> Post(SurveyModel model, Survey sv)
	{
		if (ModelState.IsValid)
		{
			sv = new()
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
		return View(model);
	}

	public async Task<ActionResult> Update(int id, SurveyModel model)
	{
		if (id != model.SurveyId) return NotFound();

		var sv = await _context.Surveys.FindAsync(id);
		if (sv == null) return NotFound();

		model = new()
		{
			SurveyId = sv.SurveyId,
			Title = sv.Title,
			Topic = sv.Topic,
			CreatedBy = sv.CreatedBy,
			CreateDate = sv.CreateDate,
			EndDate = sv.EndDate,
			TargetAudience = sv.TargetAudience,
			Active = sv.Active
		};
		return View(model);
	}

	[HttpPost]
	public async Task<ActionResult> UpdateSurvey(int id, SurveyModel model)
	{
		if (id != model.SurveyId) return NotFound();

		var sv = await _context.Surveys.FindAsync(id);
		if (sv == null) return NotFound();

		if (ModelState.IsValid)
		{
			try
			{
				sv = new()
				{
					Title = model.Title,
					Topic = model.Topic,
					CreateDate = model.CreateDate,
					EndDate = model.EndDate,
					TargetAudience = model.TargetAudience,
					Active = model.Active
				};
				_context.Update(sv);
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(List));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("Error", ex.Message);
			}
		}
		return View(model);
	}

	[HttpPost]
	public async Task<ActionResult> Delete(int id)
	{
		var sv = await _context.Surveys.FindAsync(id);
		if (sv == null) return NotFound();

		_context.Surveys.Remove(sv);
		await _context.SaveChangesAsync();

		return RedirectToAction(nameof(List));
	}
}
