using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
                TempData["PermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }

            var entries = await _context.CompetitionEntries.Include(e => e.User).Include(e => e.Competition).ToListAsync();
            return View(entries);
        }

        // GET: Grades/Grade/5
        public async Task<IActionResult> Grade(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                TempData["PermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.CompetitionEntries.Include(e => e.User).Include(e => e.Competition).FirstOrDefaultAsync(m => m.EntryId == id);
            if (entry == null)
            {
                return NotFound();
            }

            return View(new GradeTest { EntryId = entry.EntryId });
        }

        // POST: Grades/Grade/5
        [HttpPost]
        public async Task<IActionResult> Grade([Bind("EntryId,Score")] GradeTest gradeTest)
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                TempData["PermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }
            gradeTest.GradeDate = DateTime.Now;
            _context.Add(gradeTest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
