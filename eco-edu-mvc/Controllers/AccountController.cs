using eco_edu_mvc.Models.AccountsViewModel;
using eco_edu_mvc.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace eco_edu_mvc.Controllers;
public class AccountController : Controller
{
	private readonly EcoEduContext context;
	private readonly IEmailSender emailSender;
	private readonly IMemoryCache _memoryCache;

	public AccountController(EcoEduContext context, IEmailSender emailSender, IMemoryCache memoryCache)
	{
		this.context = context;
		this.emailSender = emailSender;
		_memoryCache = memoryCache;
	}

	public IActionResult Signup() => View();

	[HttpPost]
	public async Task<IActionResult> Signup(SignupModel model)
	{
		if (ModelState.IsValid)
		{
			var existingUserWithUsername = await context.Users.AnyAsync(u => u.Username == model.Username);
			var existsingUserCode = await context.Users.AnyAsync(u => u.UserCode == model.UserCode);
			var isValidUserCode = model.UserCode.ToLower().StartsWith("st") || model.UserCode.ToLower().StartsWith("tf");
			if (existingUserWithUsername || existsingUserCode || !isValidUserCode)
			{
				if (existingUserWithUsername)
				{
					ModelState.AddModelError("Username", "The username has already been taken!");
				}
				if (existsingUserCode)
				{
					ModelState.AddModelError("User_Code", "The usercode is already exsits!");
				}
				if (!isValidUserCode)
				{
					ModelState.AddModelError("User_Code", "The usercode must start with st for student and tf for staff/faculty");
				}
				return View(model);
			}
			User user = new()
			{
				Username = model.Username,
				Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
				Fullname = model.Fullname,
				UserCode = model.UserCode,
				IsAccept = false,
				EmailVerify = false,
				CreateDate = DateTime.Now
			};
			context.Users.Add(user);
			await context.SaveChangesAsync();
			return RedirectToAction("login");
		}
		return View(model);
	}

	public IActionResult login() => View();

	[HttpPost]
	public async Task<IActionResult> login(loginModel model)
	{
		if (ModelState.IsValid)
		{
			var user = await context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
			if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
			{
				HttpContext.Session.SetString("UserId", user.UserId.ToString());
				HttpContext.Session.SetString("Username", user.Username);
				HttpContext.Session.SetString("User_Code", user.UserCode);
				HttpContext.Session.SetString("Is_Accept", user.IsAccept.ToString().ToLower());
                return RedirectToAction("Index", "Home");
			}
			ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			return View(model);
		}
		return View(model);
	}

	[HttpPost]
	public IActionResult logout()
	{
		HttpContext.Session.Clear();
		return RedirectToAction("login");
	}


	public async Task<IActionResult> Profile()
	{
		if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
		{
			return RedirectToAction("login");
		}
		var userId = int.Parse(HttpContext.Session.GetString("UserId"));
		var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
		if (user == null)
		{
			return NotFound();
		}

		ProfileViewModel profile = new()
		{
			Username = user.Username,
			FullName = user.Fullname,
			User_Code = user.UserCode,
			Entry_Date = user.EntryDate,
			Email = user.Email,
			Section = user.Section,
			Class = user.Class,
			Citizen_Id = user.CitizenId
		};

		return View(profile);
	}

	public async Task<IActionResult> Edit()
	{
		if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
		{
			return RedirectToAction("login");
		}
		var userId = int.Parse(HttpContext.Session.GetString("UserId"));
		var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
		if (user == null)
		{
			return NotFound();
		}

		ProfileEditModel profile = new()
		{
			FullName = user.Fullname,
			Email = user.Email,
			Section = user.Section,
			Class = user.Class,
			Citizen_Id = user.CitizenId
		};

		return View(profile);
	}

	public async Task<IActionResult> Edit(ProfileEditModel model)
	{
		if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
		{
			return RedirectToAction("login");
		}
		var userId = int.Parse(HttpContext.Session.GetString("UserId"));
		try
		{
			var existingEmail = await context.Users.AnyAsync(u => u.Email == model.Email);
			if (existingEmail)
			{
				ModelState.AddModelError("Email", "Email is already taken.");
			}
			var user = await context.Users.FindAsync(userId);
			if (user == null)
			{
				return NotFound();
			}

			user.Email = model.Email;
			user.Section = model.Section;
			user.Class = model.Class;
			user.Fullname = model.FullName;
			user.CitizenId = model.Citizen_Id;

			context.Users.Update(user);
			await context.SaveChangesAsync();

			return RedirectToAction("profile");
		}
		catch (Exception ex)
		{
			ModelState.AddModelError("Error", ex.Message);
			return View();
		}
	}

	[HttpPost]
	public async Task<IActionResult> SendVerificationCode()
	{
		try
		{
			if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
			{
				return RedirectToAction("login");
			}
			var userId = int.Parse(HttpContext.Session.GetString("UserId"));

			var user = await context.Users.FindAsync(userId);
			if (user == null)
			{
				return NotFound();
			}
			var verificationCode = GenerateVerificationCode();

			var receiver = user.Email;
			var subject = "Verification Code";
			var message = verificationCode;

			await emailSender.SendEmailAsync(receiver, subject, message);

			user.VerificationToken = verificationCode;
			context.Users.Update(user);
			await context.SaveChangesAsync();
			return RedirectToAction("CheckVerificationCode");
		}
		catch (Exception ex)
		{
			ModelState.AddModelError("Error", ex.Message);
			return View();
		}
	}


	public IActionResult CheckVerificationCode() => View();

	[HttpPost]
	public async Task<IActionResult> CheckVerificationCode(CheckVerificationCodeModel model)
	{
		try
		{
			if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
			{
				return RedirectToAction("login");
			}
			var userId = int.Parse(HttpContext.Session.GetString("UserId"));
			var user = await context.Users.FindAsync(userId);
			if (user == null)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				if (model.code == user.VerificationToken)
				{
					user.EmailVerify = true;
					context.Users.Update(user);
					await context.SaveChangesAsync();
					return RedirectToAction("Profile");
				}
			}
			return View(model);
		}
		catch (Exception ex)
		{
			ModelState.AddModelError("Error", ex.Message);
			return View();
		}
	}

	private string GenerateVerificationCode()
	{
		return Guid.NewGuid().ToString().Substring(0, 6);
	}

	public IActionResult ForgotPassword() => View();

	[HttpPost]
	public async Task<IActionResult> ForgotPassword(CheckVerificationCodeModel model)
	{
		try
		{
			if (ModelState.IsValid)
			{
				var user = await context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
				if (user == null)
				{
					return NotFound();
				}

				HttpContext.Session.SetString("Email", user.Email);
				HttpContext.Session.SetString("UserId", user.UserId.ToString());

				var RegainPwCode = GenerateVerificationCode();

				//store the code into cache
				_memoryCache.Set(user.Email, RegainPwCode, TimeSpan.FromMinutes(10));

				var reiceiver = user.Email;
				var subject = "Recovery Password";
				var message = RegainPwCode;

				await emailSender.SendEmailAsync(reiceiver, subject, message);
				return RedirectToAction("CheckRecoveryCode");
			}
			return View(model);
		}
		catch (Exception ex)
		{
			ModelState.AddModelError("Error", ex.Message);
			return View();
		}
	}

	public IActionResult CheckRecoveryCode() => View();

	[HttpPost]
	public async Task<IActionResult> CheckRecoveryCode(CheckVerificationCodeModel model)
	{
		try
		{
			if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
			{
				return RedirectToAction("login");
			}
			var userId = int.Parse(HttpContext.Session.GetString("UserId"));

			var user = await context.Users.FindAsync(userId);
			if (user == null)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{

				if (_memoryCache.TryGetValue(user.Email, out string cachedCode))
				{
					if (model.code == cachedCode)
					{
						HttpContext.Session.SetString("Email", user.Email);
						HttpContext.Session.SetString("UserId", user.UserId.ToString());
						return RedirectToAction("ChangePassword");
					}
				}
				return View(model);
			}
			return View(model);
		}
		catch (Exception ex)
		{
			ModelState.AddModelError("Error", ex.Message);
			return View();
		}
	}

	public IActionResult ChangePassword() => View();

	[HttpPost]
	public async Task<IActionResult> ChangePassword(CheckVerificationCodeModel model)
	{
		try
		{
			var userId = int.Parse(HttpContext.Session.GetString("UserId"));
			var user = await context.Users.FindAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
			context.Users.Update(user);
			await context.SaveChangesAsync();
			return RedirectToAction("login");
		}
		catch (Exception ex)
		{
			ModelState.AddModelError("Error", ex.Message);
			return View();
		}
	}

}
