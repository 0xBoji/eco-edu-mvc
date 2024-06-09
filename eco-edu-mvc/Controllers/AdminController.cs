using eco_edu_mvc.Models.AccountsViewModel;
using eco_edu_mvc.Models.AdminViewModel;
using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Controllers;
public class AdminController(EcoEduContext context) : Controller
{
    private readonly EcoEduContext _context = context;

    public async Task<ActionResult> Index()
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            var surveys = await _context.Surveys.Where(s => s.Active == true).CountAsync();
            var competitions = await _context.Competitions.Where(c => c.Active == true).CountAsync();
            var users = await _context.Users.Where(u => u.Role == "Staff" && u.Role == "Student").CountAsync();
            var seminars = await _context.Seminars.Where(m => m.Active == true).CountAsync();

            AdminModel model = new()
            {
                Surveys = surveys,
                Competitions = competitions,
                Users = users,
                Seminars = seminars
            };
            return View(model);
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    public async Task<IActionResult> RequestShow()
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            return View(await _context.Users.ToListAsync());
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpPost]
    public async Task<IActionResult> Accept(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null) return NotFound();

        user.IsAccept = true;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("RequestShow");
    }

    [HttpPost]
    public async Task<IActionResult> Decline(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) return NotFound();
            if (model.EntryDate >= DateTime.Now)
            {
                user.EntryDate = model.EntryDate;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
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
            return View(await _context.Users.ToListAsync());
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

		//check and delete the relationship
		var competitionEntries = context.CompetitionEntries.Where(ce => ce.UserId == id);
		context.CompetitionEntries.RemoveRange(competitionEntries);
		var response = context.Responses.Where(rs => rs.UserId == id);
		context.Responses.RemoveRange(response);
		var seminars = context.SeminarMembers.Where(s => s.UserId == id);
		context.SeminarMembers.RemoveRange(seminars);

		context.Users.Remove(user);
		await context.SaveChangesAsync();
		return RedirectToAction("RequestShow");
	}
}
