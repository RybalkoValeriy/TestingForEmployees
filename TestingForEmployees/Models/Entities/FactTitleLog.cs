using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingForEmployees.Models.Entities
{
    public class FactTitleLog
    {
        public int Id { get; set; }
        public DateTime DateAdd { get; set; }
        public string TitleText { get; set; }
        public StartedTestLog StartedTestLog { get; set; }
        public ICollection<FactQuestLog> FactQuestCollection {get;set;}
        public ICollection<AnswerUserResultLog> AnswerUserResultLog { get; set; }

        public FactTitleLog()
        {
            DateAdd = DateTime.Now;
        }
    }
}
