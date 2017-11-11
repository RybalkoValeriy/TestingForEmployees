using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingForEmployees.Models.Entities
{
    // сколько попыток разрешено сделать пользователю
    public class TitleUserCountAccess
    {
        public int Id { get; set; }
        public ApplicationUsers User { get; set; }
        public TestTitle Title { get; set; }
        public int Attempts { get; set; }
        public bool State { get; set; }
        public DateTime? DateStart { get; set; }
        public ICollection<StartedTestLog> StartedTestLogCollection { get; set; }

        public TitleUserCountAccess()
        {
            Attempts = 3;
            DateStart = DateTime.Now;
        }
    }
}
