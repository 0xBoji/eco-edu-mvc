using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace eco_edu_mvc.Models.AccountsViewModel;

public class ChangePasswordModel
{
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
	[Required, DataType(DataType.Password),
				Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
	public string ConfirmPassword { get; set; }
}
