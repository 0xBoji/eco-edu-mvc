using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.SurveyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Controllers;
public class questionController(EcoEduContext context) : Controller
{
	private readonly EcoEduContext _context = context;

	[HttpGet]
	public async Task<IActionResult> List() => View(await _context.SurveyQuestions.ToListAsync());

	[HttpGet]
	public async Task<IActionResult> Get(int id)
	{
		var quest = await _context.SurveyQuestions.FirstOrDefaultAsync(q => q.QuestionId == id);
		if (quest == null) return NotFound();

		return View(quest);
	}

	public IActionResult Post() => View();

	[HttpPost]
	public async Task<IActionResult> Post(QuestionModel model, SurveyQuestion quest)
	{
		if (!ModelState.IsValid) return RedirectToAction("Login", "Account");

		quest = new()
		{
			SurveyId = model.SurveyId,
			Question = model.Question,
			QuestionType = model.QuestionType
		};
		_context.SurveyQuestions.Add(quest);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(List), new { id = quest.SurveyId }, quest);

	}

	public async Task<IActionResult> Update(int id, QuestionModel model)
	{
		if (id != model.QuestionId) return NotFound();

		var quest = await _context.SurveyQuestions.FindAsync(id);
		if (quest == null) return NotFound();

		model = new()
		{
			SurveyId = quest.SurveyId,
			Question = quest.Question,
			QuestionType = quest.QuestionType
		};
		return View(model);
	}

	public async Task<IActionResult> UpdateQuestion(int id, QuestionModel model, SurveyQuestion quest)
	{
		if (id != model.QuestionId) return NotFound();

		var check = await _context.SurveyQuestions.FindAsync(id);
		if (check == null) return NotFound();

		if (ModelState.IsValid)
		{
			try
			{
				 quest = new()
				{
					SurveyId = model.SurveyId,
					Question = model.Question,
					QuestionType = model.QuestionType
				};
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("Error", ex.Message);
			}
		}
		return View(model);
	}

	public async Task<IActionResult> Delete(int id)
	{
		var check = await _context.SurveyQuestions.FindAsync(id);
		if (check == null) return NotFound();

		_context.SurveyQuestions.Remove(check);
		await _context.SaveChangesAsync();

		return RedirectToAction(nameof(List));
	}
}
