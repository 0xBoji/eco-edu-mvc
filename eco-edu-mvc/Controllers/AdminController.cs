using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Controllers;
public class AdminController : Controller
{
    private readonly EcoEduContext _context;

    // GET: Admin/Competitions
    public async Task<IActionResult> Competitions()
    {
        var competitions = await _context.Competitions.ToListAsync();
        return View(competitions);
    }

    // GET: Admin/CreateCompetition
    public IActionResult CreateCompetition()
    {
        return View();
    }

    // POST: Admin/CreateCompetition
    [HttpPost]
    public async Task<IActionResult> CreateCompetition(Competition competition)
    {
        if (ModelState.IsValid)
        {
            _context.Add(competition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Competitions));
        }
        return View(competition);
    }

    // POST: Admin/DeleteCompetition/5
    [HttpPost, ActionName("DeleteCompetition")]
    public async Task<IActionResult> DeleteCompetitionConfirmed(int id)
    {
        var competition = await _context.Competitions.FindAsync(id);
        if (competition != null)
        {
            _context.Competitions.Remove(competition);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Competitions));
    }
}
