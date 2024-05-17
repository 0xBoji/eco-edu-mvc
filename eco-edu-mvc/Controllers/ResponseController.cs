using eco_edu_mvc.Models.Entities;
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
}