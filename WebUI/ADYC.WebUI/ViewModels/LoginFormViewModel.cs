using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class LoginFormViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}