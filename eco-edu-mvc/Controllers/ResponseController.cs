using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.SurveyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Controllers;
public class ResponseController(EcoEduContext context) : Controller
{
	private readonly EcoEduContext _context = context;
	public async Task<IActionResult> List() => View(await _context.Responses.ToListAsync());

	public async Task<IActionResult> Get(int id)
	{
		var rep = await _context.Responses.FirstOrDefaultAsync(r => r.ResponseId == id);
		if (rep == null) return NotFound();

		return View(rep);
	}

	public ActionResult Post() => View();

	[HttpPost]
	public async Task<IActionResult> Post(ResponseModel model)
	{
		if (!ModelState.IsValid) return View(model);

		Response rep = new()
		{
			QuestionId = model.QuestionId,
			Answer = model.Answer
		};
		_context.Responses.Add(rep);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(List), new { id = rep.ResponseId }, rep);
	}

	public async Task<IActionResult> Update(int id)
	{
		var rep = await _context.Responses.FindAsync(id);
		if (rep == null) return NotFound();

		ResponseModel model = new()
		{
			ResponseId = rep.ResponseId,
			QuestionId = rep.QuestionId,
			Answer = rep.Answer
		};
		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> Update(int id, ResponseModel model)
	{
		if (id != model.ResponseId) return NotFound();

		if (!ModelState.IsValid) return View(model);

		var rep = await _context.Responses.FindAsync(id);
		if (rep == null) return NotFound();

		try
		{
			rep.QuestionId = model.QuestionId;
			rep.Answer = model.Answer;

			_context.Responses.Update(rep);
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
		var rep = await _context.Responses.FindAsync(id);
		if (rep == null) return NotFound();

		return View(rep);
	}

	[HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirm(int id)
	{
		var rep = await _context.Responses.FindAsync(id);
		if (rep == null) return NotFound();

		_context.Responses.Remove(rep);
		await _context.SaveChangesAsync();

		return RedirectToAction(nameof(List));
	}
}