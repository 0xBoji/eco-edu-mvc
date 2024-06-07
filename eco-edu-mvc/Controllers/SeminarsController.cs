using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eco_edu_mvc.Controllers
{
    public class SeminarsController : Controller
    {
        private readonly EcoEduContext _context;

        public SeminarsController(EcoEduContext context)
        {
            _context = context;
        }

        // GET: Seminars
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                TempData["PermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }

            var seminars = await _context.Seminars
                                         .Include(s => s.Sm)
                                         .ThenInclude(sm => sm.User)
                                         .ToListAsync();
            return View(seminars);
        }

        // GET: Seminars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seminar = await _context.Seminars
                                        .Include(s => s.Sm)
                                        .ThenInclude(sm => sm.User)
                                        .FirstOrDefaultAsync(m => m.SeminarId == id);
            if (seminar == null)
            {
                return NotFound();
            }

            return View(seminar);
        }

        // GET: Seminars/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                TempData["PermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Seminars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Location,Participants,OccursDate,Active")] Seminar seminar)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                TempData["PermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }

            
                if(seminar.OccursDate < DateTime.Now)
                {
                    ModelState.AddModelError("OccursDate", "Invalid OccursDate!!");
                    return View(seminar);
                }
                _context.Add(seminar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Seminars/Edit/5
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

            var seminar = await _context.Seminars.FindAsync(id);
            if (seminar == null)
            {
                return NotFound();
            }
            return View(seminar);
        }

        // POST: Seminars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SeminarId,Title,Location,Participants,OccursDate,Active")] Seminar seminar)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                TempData["PermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }

            if (id != seminar.SeminarId)
            {
                return NotFound();
            }

            if (seminar.OccursDate < DateTime.Now)
            {
                ModelState.AddModelError("OccursDate", "Invalid OccursDate!!");
                return View(seminar);
            }

            try
                {
                    _context.Update(seminar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeminarExists(seminar.SeminarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
        }

        // GET: Seminars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                TempData["PermissionDenied"] = true;
                return RedirectToAction("Index", "Home" );
            }

            if (id == null)
            {
                return NotFound();
            }

            var seminar = await _context.Seminars
                .FirstOrDefaultAsync(m => m.SeminarId == id);
            if (seminar == null)
            {
                return NotFound();
            }

            return View(seminar);
        }

        // POST: Seminars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                TempData["PermissionDenied"] = true;
                return RedirectToAction("Index", "Home");
            }

            var seminar = await _context.Seminars.FindAsync(id);
            _context.Seminars.Remove(seminar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeminarExists(int id)
        {
            return _context.Seminars.Any(e => e.SeminarId == id);
        }
    }
}
