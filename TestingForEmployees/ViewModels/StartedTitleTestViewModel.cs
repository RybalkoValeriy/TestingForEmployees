using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingForEmployees.Models.Entities;
using TestingForEmployees.Models;

namespace TestingForEmployees.ViewModels
{
    public class StartedTitleTestViewModel
    {
        public int IdTitle { get; set; }
        public string Title { get; set; }
        public int CountQuestion { get; set; }
        public DateTime? DateStarted { get; set; }
        public bool State { get; set; }
        public StartedTestLog StartedTestLog { get; set; }
    }
}
