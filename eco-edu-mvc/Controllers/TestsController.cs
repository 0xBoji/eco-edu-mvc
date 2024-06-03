using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace eco_edu_mvc.Controllers
{
    public class TestsController : Controller
    {
        private readonly EcoEduContext _context;

        public TestsController(EcoEduContext context)
        {
            _context = context;
        }

        // GET: Tests
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") != "Student" || !string.Equals(HttpContext.Session.GetString("Is_Accept"), "true", StringComparison.OrdinalIgnoreCase))
            {
                TempData["PermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }

            var competitions = await _context.Competitions.Where(c => c.Active == true).ToListAsync();
            return View(competitions);
        }

        // GET: Tests/Join/5
        public async Task<IActionResult> Join(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Student" || !string.Equals(HttpContext.Session.GetString("Is_Accept"), "true", StringComparison.OrdinalIgnoreCase))
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

            return View(new CompetitionEntry { CompetitionId = competition.CompetitionId });
        }

        // POST: Tests/Join/5
        [HttpPost]
        public async Task<IActionResult> Join([Bind("CompetitionId,SubmissionText")] CompetitionEntry competitionEntry)
        {
            if (HttpContext.Session.GetString("Role") != "Student" || !string.Equals(HttpContext.Session.GetString("Is_Accept"), "true", StringComparison.OrdinalIgnoreCase))
            {
                TempData["PermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }

            var userId = int.Parse(HttpContext.Session.GetString("UserId"));
            competitionEntry.UserId = userId;
            competitionEntry.SubmissionDate = DateTime.Now;

            
                _context.Add(competitionEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
    }
}
