using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TestingForEmployees.Models.Entities;

namespace TestingForEmployees.ViewModels
{

        public class RegisterViewModel
        {
            [Required(ErrorMessage ="Поле прізвище обов'язкове")]
            [Display(Name = "Прізвище:")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Поле Ім'я обов'язкове")]
            [Display(Name = "Ім'я:")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Поле по батькові обов'язкове")]
            [Display(Name = "По батькові:")]
            public string MiddleName { get; set; }

            [Required(ErrorMessage = "Поле бранч обов'язкове")]
            [Display(Name = "Номер відділення банку:")]
            public int IDBranch { get; set; }

            [Required(ErrorMessage = "Поле логин обов'язкове")]
            [Display(Name ="Особистий логін для входу:")]
            public string LoginUser { get; set; }

            [Required(ErrorMessage = "Поле пароль обов'язкове")]
            [DataType(DataType.Password)]
            [Display(Name = "Ваш пароль:")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Підтверження паролю обов'язково")]
            [Compare("Password", ErrorMessage = "Паролі не співпадають")]
            [DataType(DataType.Password)]
            [Display(Name = "Підтверження паролю:")]
            public string PasswordConfirm { get; set; }
        }
}
