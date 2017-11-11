using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingForEmployees.Models.Entities
{
    public class AnswerUserResultLog
    {
        public int Id { get; set; }
        public ApplicationUsers User { get; set; }
        public FactTitleLog factTitleLog { get; set; }
        public FactQuestLog factQuestLog { get; set; }
        public FactAnswersLog factAnswersLog { get; set; }
        public DateTime DateAnsw { get; set; }
        public bool State { get; set; }

        public AnswerUserResultLog()
        {
            DateAnsw = DateTime.Now;
        }
    }
}
