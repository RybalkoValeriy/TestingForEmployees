using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingForEmployees.Models.Entities
{
    public class FactAnswersLog
    {
        public int Id { get; set; }
        public DateTime DateAdd { get; set; }
        public FactQuestLog FactQuestLog { get; set; }
        public string AnswersText { get; set; }
        public bool State { get; set; }
        public ICollection<AnswerUserResultLog> AnswerUserResultLog { get; set; }


        public FactAnswersLog()
        {
            DateAdd = DateTime.Now;
        }
    }
}
