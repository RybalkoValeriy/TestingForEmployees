using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingForEmployees.Models;
using TestingForEmployees.Models.Entities;


namespace TestingForEmployees.ViewModels
{
    public class UserAndTitle
    {
        public ApplicationUsers User { get; set; }
        public IEnumerable<TestTitle> TitleColl { get; set; }
    }
}
