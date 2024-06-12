using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace eco_edu_mvc.Controllers;
public class ResponsesController(EcoEduContext context) : Controller
{
    private readonly EcoEduContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Index(int? page)
    {
        if (HttpContext.Session.GetString("Role") == "Admin")
        {
            // Pagination
            int pageSize = 3;
            int pageNumber = page ?? 1;

            return View(await _context.Responses.Include(q => q.Question).ThenInclude(s => s.Survey).Include(u => u.User)
                .ToPagedListAsync(pageNumber, pageSize));
        }
        TempData["PermissionDenied"] = true;
        return RedirectToAction("index", "home");
    }

}