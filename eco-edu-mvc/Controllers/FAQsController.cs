using eco_edu_mvc.Models.Entities;
using eco_edu_mvc.Models.FAQsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eco_edu_mvc.Controllers;
public class FAQsController : Controller
{
    public readonly EcoEduContext _context;
    private readonly ILogger<FAQsController> _logger;
    public FAQsController (EcoEduContext context, ILogger<FAQsController> logger)

    {
        this._context=context;
        this._logger = logger;
    }
    public IActionResult FAQ()
    {
        var faq = _context.Faqs.ToList();
        return View(faq);
    }

    //GET: Admin/FAQS
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            TempData["PermissionDenied"] = true;
            return RedirectToAction("Index", "Home");
        }
        var faqs=await _context.Faqs.ToListAsync();
        return View(faqs);
    }

    //Get: admin/create faqs
    public ActionResult Create() {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            TempData["PermissionDenied"] = true;
            return RedirectToAction("Index","Home");
        }
        return View();
    }

    //Post: Admin/CreateFAQS
    [HttpPost]
    public async Task<IActionResult> Create([Bind("FaqId,Question,Answer")] Faq faq)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            TempData["PermissionDenied"] = true;
            return RedirectToAction("Index", "Home");
        }
        if(faq == null)
        {
            return BadRequest("Invalid faq data.");
        }
        if (ModelState.IsValid)
        {
            try
            {
               
                _context.Add(faq);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating faq.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the faq.");
            }
        }
        return View(faq);
    }

    // POST: Admin/Faq/id
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            TempData["PermissionDenied"] = true;
            return RedirectToAction("Index", "Home");
        }

        var faq = await _context.Faqs.FindAsync(id);
        if (faq != null)
        {
            try
            {
                _context.Faqs.Remove(faq);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting faq.");
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the faq.");
            }
        }
        return RedirectToAction(nameof(Index));
    }

}
