using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.SurveyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Controllers;
public class ResponseController(EcoEduContext context) : Controller
{
	private readonly EcoEduContext _context = context;

	[HttpGet]
	public async Task<IActionResult> List() => View(await _context.Responses.ToListAsync());

	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		var rep = await _context.Responses.FirstOrDefaultAsync(r => r.ResponseId == id);
		if (rep == null) return NotFound();

		return View(rep);
	}

	//[HttpPost]
	//public async Task<IActionResult> Post(ResponseModel model)
	//{
	//	if (!ModelState.IsValid) return View(model);

	//	Response rep = new()
	//	{
	//		QuestionId = model.QuestionId,
	//		Answer = model.Answer
	//	};
	//	_context.Responses.Add(rep);
	//	await _context.SaveChangesAsync();

	//	return CreatedAtAction(nameof(List), new { id = rep.ResponseId }, rep);
	//}

}