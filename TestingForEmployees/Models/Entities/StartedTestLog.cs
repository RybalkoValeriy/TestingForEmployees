using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingForEmployees.ViewModels;

namespace TestingForEmployees.Models.Entities
{
    // фиксация попыток
    public class StartedTestLog
    {
        public int Id { get; set; }
        public ApplicationUsers UserId { get; set; }
        public TitleUserCountAccess TitleUserAccess { get; set; }
        public DateTime DateStarted { get; set; }
        public bool State { get; set; }
        public ICollection<FactTitleLog> FactTitleLogCollection { get; set; }
        public string IP { get; set; }

        public StartedTestLog()
        {
            DateStarted = DateTime.Now;
        }
    }
}
