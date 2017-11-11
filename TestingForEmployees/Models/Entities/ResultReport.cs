using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingForEmployees.Models.Entities
{
    public class ResultReport
    {
        public int Id { get; set; }
        public ApplicationUsers User { get; set; }
        public DateTime DateAnsw { get; set; }
        public FactTitleLog FactTitleLog { get; set; }
        public StartedTestLog StartedTestLog { get; set; }
        public decimal Percent { get; set; }

        public ResultReport()
        {
            DateAnsw = DateTime.Now;
        }
    }
}
