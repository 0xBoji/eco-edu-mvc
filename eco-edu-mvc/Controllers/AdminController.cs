using eco_edu_mvc.Models.AccountsViewModel;
using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eco_edu_mvc.Controllers;
public class AdminController : Controller
{
    private readonly EcoEduContext context;
    public IActionResult Index() => View();

    #region
    [HttpPost]
    public async Task<IActionResult> CreateCompetitions(SignupModel model)
    {
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCompetitions(SignupModel model)
    {
        return View(model);
    }
    #endregion

    public IActionResult Survey() => View();
}
