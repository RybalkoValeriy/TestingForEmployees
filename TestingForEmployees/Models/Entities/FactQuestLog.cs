using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingForEmployees.Models.Entities
{
    public class FactQuestLog
    {
        public int Id { get; set; }
        public DateTime DateAdd { get; set; }
        public string QuestText { get; set; }
        public FactTitleLog FactTitleLog { get; set; }
        public ICollection<FactAnswersLog> FactAnswersLog { get; set; }
        public ICollection<AnswerUserResultLog> AnswerUserResultLog { get; set; }

        public FactQuestLog()
        {
            DateAdd = DateTime.Now;
        }
    }
}
