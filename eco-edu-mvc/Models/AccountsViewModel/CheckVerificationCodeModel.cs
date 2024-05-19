using System.ComponentModel.DataAnnotations;

namespace eco_edu_mvc.Models.AccountsViewModel;

public class CheckVerificationCodeModel
{
	[Required, DataType(DataType.EmailAddress)]
	public string Email { get; set; }

    public string Username { get; set; }

    [Required]
	public string code { get; set; }

	[Required, DataType(DataType.Password)]
	public string Password { get; set; }

	[Required, DataType(DataType.Password), 
				Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
	public string ConfirmPassword { get; set; }
}
