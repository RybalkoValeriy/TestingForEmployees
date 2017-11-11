using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace TestingForEmployees.ViewModels
{
    public class LogInViewModel
    {
        [Required(ErrorMessage = "Поле логін обов'язкове")]
        [Display(Name = "Логін:")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле пароль обов'язкове")]
        [Display(Name = "Пароль:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
