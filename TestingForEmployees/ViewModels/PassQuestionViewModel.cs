using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingForEmployees.Models.Entities;

namespace TestingForEmployees.ViewModels
{
    public class PassQuestionViewModel
    {
        public StartedTestLog startedTestLog { get; set; }
        public FactTitleLog factTitleLog { get; set; }
        public FactQuestLog factQuestLog { get; set; }
        public IEnumerable<FactAnswersLog> factAnswersLog { get; set; }
    }
}
