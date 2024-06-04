using eco_edu_mvc.Models.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eco_edu_mvc.Controllers;
public class CompetitionsController : Controller
{
    private readonly EcoEduContext _context;
    private readonly ILogger<CompetitionsController> _logger;

    public CompetitionsController(EcoEduContext context, ILogger<CompetitionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Admin/Competitions
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            TempData["PermissionDenied"] = true;
            return RedirectToAction("Index", "Home");
        }

        var competitions = await _context.Competitions.ToListAsync();
        return View(competitions);
    }

    // GET: Admin/CreateCompetition
    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            TempData["PermissionDenied"] = true;
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    // POST: Admin/CreateCompetition
    [HttpPost]
    public async Task<IActionResult> Create([Bind("CompetitionId,Title,Description,StartDate,EndDate,Prizes,Images")] Competition competition)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            TempData["PermissionDenied"] = true;
            return RedirectToAction("Index", "Home");
        }

        if (competition == null)
        {
            return BadRequest("Invalid competition data.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                competition.Active = true;
                _context.Add(competition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating competition.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the competition.");
            }
        }
        return View(competition);
    }

    // GET: Admin/EditCompetition/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            TempData["PermissionDenied"] = true;
            return RedirectToAction("Index", "Home");
        }

        if (id == null)
        {
            return NotFound();
        }

        var competition = await _context.Competitions.FindAsync(id);
        if (competition == null)
        {
            return NotFound();
        }

        return View(competition);
    }

    // POST: Admin/EditCompetition/5
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("CompetitionId,Title,Description,StartDate,EndDate,Prizes,Images,Active")] Competition competition)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            TempData["PermissionDenied"] = true;
            return RedirectToAction("Index", "Home");
        }

        if (id != competition.CompetitionId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(competition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompetitionExists(competition.CompetitionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return View(competition);
    }

    // POST: Admin/DeleteCompetition/5
    [HttpPost, ActionName("DeleteCompetition")]
    public async Task<IActionResult> DeleteCompetitionConfirmed(int id)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            TempData["PermissionDenied"] = true;
            return RedirectToAction("Index", "Home");
        }

        var competition = await _context.Competitions.FindAsync(id);
        if (competition != null)
        {
            try
            {
                _context.Competitions.Remove(competition);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting competition.");
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the competition.");
            }
        }
        return RedirectToAction(nameof(Index));
    }

    private bool CompetitionExists(int id)
    {
        return _context.Competitions.Any(e => e.CompetitionId == id);
    }
}
