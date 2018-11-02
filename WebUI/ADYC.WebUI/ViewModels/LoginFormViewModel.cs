using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class LoginFormViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}