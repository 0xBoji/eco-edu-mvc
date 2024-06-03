using eco_edu_mvc.Models.AdminViewModel;
using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace eco_edu_mvc.Controllers;
public class AdminController : Controller
{
	private readonly EcoEduContext context;

	public AdminController(EcoEduContext context)
	{
		this.context = context;
	}
	public ActionResult Index()
	{
		if (HttpContext.Session.GetString("Role") == "Admin")
		{
			return View();
		}
		TempData["PermissionDenied"] = true;
		return RedirectToAction("index", "home");
	}

	public async Task<IActionResult> RequestShow()
	{
		if (HttpContext.Session.GetString("Role") == "Admin")
		{
			return View(await context.Users.ToListAsync());
		}
		TempData["PermissionDenied"] = true;
		return RedirectToAction("index", "home");
	}

	[HttpPost]
	public async Task<IActionResult> Accept(int id)
	{
		var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
		if (user == null) return NotFound();
		user.IsAccept = true;
		context.Users.Update(user);
		await context.SaveChangesAsync();
		return RedirectToAction("RequestShow");
	}

	[HttpPost]
	public async Task<IActionResult> Decline(int id)
	{
		var user = await context.Users.FindAsync(id);
		if (user == null) return NotFound();
		context.Users.Remove(user);
		await context.SaveChangesAsync();
		return RedirectToAction("RequestShow");
	}

	public IActionResult Update()
	{
		if (HttpContext.Session.GetString("Role") == "Admin")
		{
			return View();
		}
		TempData["PermissionDenied"] = true;
		return RedirectToAction("index", "home");
	}

	[HttpPost]
	public async Task<IActionResult> Update(EditUserModel model, int id)
	{
		if (ModelState.IsValid)
		{
			var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
			if (user == null) return NotFound();
			if (model.EntryDate <= DateTime.Now)
			{
				user.EntryDate = model.EntryDate;
				context.Users.Update(user);
				await context.SaveChangesAsync();
				return RedirectToAction("RequestShow");
			}
			ModelState.AddModelError("EntryDate", "Entrydate must not be a future day!");
			return View(model);
		}
		ModelState.AddModelError("EntryDate", "Entrydate is invalid!");
		return View(model);
	}

	public async Task<IActionResult> UserList()
	{
		if (HttpContext.Session.GetString("Role") == "Admin")
		{
			return View(await context.Users.ToListAsync());
		}
		TempData["PermissionDenied"] = true;
		return RedirectToAction("index", "home");
	}

	[HttpPost]
	public async Task<IActionResult> RemoveUser(int id)
	{
		var user = await context.Users.FindAsync(id);
		if (user == null) return NotFound();

		//check and delete the relationship
		var competitionEntries = context.CompetitionEntries.Where(ce => ce.UserId == id);
		context.CompetitionEntries.RemoveRange(competitionEntries);
		var response = context.Responses.Where(rs => rs.UserId == id);
		context.Responses.RemoveRange(response);
		var seminars = context.Seminars.Where(s => s.UserId == id);
		context.Seminars.RemoveRange(seminars);

		context.Users.Remove(user);
		await context.SaveChangesAsync();
		return RedirectToAction("RequestShow");
	}
}
