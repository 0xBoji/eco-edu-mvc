using System.ComponentModel.DataAnnotations;

namespace eco_edu_mvc.Models.AccountsViewModel;

public class CheckOnlyCode
{
    [Required]
    public string code { get; set; }
}
