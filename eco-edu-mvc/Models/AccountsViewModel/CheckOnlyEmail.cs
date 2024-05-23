using System.ComponentModel.DataAnnotations;

namespace eco_edu_mvc.Models.AccountsViewModel;

public class CheckOnlyEmail
{
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
