using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TestingForEmployees.Models;
using TestingForEmployees.Models.Entities;

namespace TestingForEmployees.Models
{
    public class ApplicationUsers : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Branch Branch { get; set; }
        public DateTime DateReg { get; set; }
        public ICollection<TitleUserCountAccess> TitleUSERCountId { get; set; }
        public ICollection<StartedTestLog> StartedTestLogId { get; set; }
        public ICollection<AnswerUserResultLog> AnswerUserResultLog { get; set; }

        public ApplicationUsers()
        {
            DateReg = DateTime.Now;
        }
    }
}
