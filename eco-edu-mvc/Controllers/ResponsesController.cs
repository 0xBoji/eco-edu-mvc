using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Controllers;
public class ResponsesController(EcoEduContext context) : Controller
{
    private readonly EcoEduContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            return View(await _context.Responses.Include(q => q.Question).ThenInclude(s => s.Survey).ToListAsync());
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

}