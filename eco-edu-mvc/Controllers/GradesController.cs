using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eco_edu_mvc.Controllers
{
    public class GradesController : Controller
    {
        private readonly EcoEduContext _context;

        public GradesController(EcoEduContext context)
        {
            _context = context;
        }

        // GET: Grades
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                TempData["StaffPermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }

            var entries = await _context.CompetitionEntries.Include(e => e.User)
                                                           .Include(e => e.Competition)
                                                           .Include(e => e.GradeTests)
                                                           .ToListAsync();
            return View(entries);
        }

        public async Task<IActionResult> Ranking()
        {
            var entries = await _context.CompetitionEntries
                                        .Include(e => e.User)
                                        .Include(e => e.Competition)
                                        .Include(e => e.GradeTests)
                                        .Where(e => e.GradeTests.Any())
                                        .OrderByDescending(e => e.GradeTests.FirstOrDefault().Score)
                                        .ToListAsync();

            return View(entries);
        }

        // GET: Grades/Grade/5
        public async Task<IActionResult> Grade(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                TempData["StaffPermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.CompetitionEntries
                                      .Include(e => e.User)
                                      .Include(e => e.Competition)
                                      .FirstOrDefaultAsync(m => m.EntryId == id);
            if (entry == null || !entry.User.IsAccept)
            {
                return NotFound();
            }

            if (entry.GradeTests.Any())
            {
                TempData["AlreadyGraded"] = "This entry has already been graded.";
                return RedirectToAction(nameof(Index));
            }

            return View(new GradeTest { EntryId = entry.EntryId });
        }

        // POST: Grades/Grade/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Grade([Bind("EntryId,Score")] GradeTest gradeTest)
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                TempData["StaffPermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }

            if (gradeTest.Score > 100 || gradeTest.Score < 0)
            {
                ModelState.AddModelError("Score", "Score must between 0 and 100");
                return View(gradeTest);
            }

            var entry = await _context.CompetitionEntries
                                      .Include(e => e.GradeTests)
                                      .Include(e => e.User)
                                      .FirstOrDefaultAsync(e => e.EntryId == gradeTest.EntryId);

            if (entry == null || !entry.User.IsAccept || entry.GradeTests.Any())
            {
                TempData["AlreadyGraded"] = "This entry has already been graded or is not accepted.";
                return RedirectToAction(nameof(Index));
            }

            gradeTest.GradeDate = DateTime.Now;
            _context.Add(gradeTest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
