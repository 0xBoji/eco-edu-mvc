using System.ComponentModel.DataAnnotations;

namespace eco_edu_mvc.Models.AccountsViewModel;

public class loginModel
{
    [Required]
    public string Username { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
}
