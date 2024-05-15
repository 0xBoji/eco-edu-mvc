using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.SurveyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Controllers;
public class QuestionController(EcoEduContext context) : Controller
{
	private readonly EcoEduContext _context = context;

	[HttpGet]
	public async Task<IActionResult> List() => View(await _context.Questions.ToListAsync());

	[HttpGet]
	public async Task<IActionResult> Get(int id)
	{
		var quest = await _context.Questions.FirstOrDefaultAsync(q => q.QuestionId == id);
		if (quest == null) return NotFound();

		return View(quest);
	}

	public ActionResult Post() => View();

	[HttpPost]
	public async Task<IActionResult> Post(QuestionModel model)
	{
		if (!ModelState.IsValid) return View(model);

		Question quest = new()
		{
			SurveyId = model.SurveyId,
			QuestionText = model.QuestionText,
			QuestionType = model.QuestionType
		};
		_context.Questions.Add(quest);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(List), new { id = quest.SurveyId }, quest);
	}

	public async Task<IActionResult> Update(int id)
	{
		var quest = await _context.Questions.FindAsync(id);
		if (quest == null) return NotFound();

		QuestionModel model = new()
		{
			QuestionId = quest.QuestionId,
			SurveyId = quest.SurveyId,
			QuestionText = quest.QuestionText,
			QuestionType = quest.QuestionType
		};
		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> Update(int id, QuestionModel model)
	{
		if (id != model.QuestionId) return NotFound();

		if (!ModelState.IsValid) return View(model);

		var quest = await _context.Questions.FindAsync(id);
		if (quest == null) return NotFound();

		try
		{
			quest.SurveyId = model.SurveyId;
			quest.QuestionText = model.QuestionText;
			quest.QuestionType = model.QuestionType;

			_context.Update(quest);
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
		var quest = await _context.Questions.FindAsync(id);
		if (quest == null) return NotFound();

		return View(quest);
	}

	[HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirm(int id)
	{
		var quest = await _context.Questions.FindAsync(id);
		if (quest == null) return NotFound();

		_context.Questions.Remove(quest);
		await _context.SaveChangesAsync();

		return RedirectToAction(nameof(List));
	}
}
