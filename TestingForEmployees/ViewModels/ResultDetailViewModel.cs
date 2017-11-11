using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingForEmployees.Models.Entities;
using TestingForEmployees.Models;

namespace TestingForEmployees.ViewModels
{
    public class ResultDetailViewModel
    {
        public FactTitleLog factTitle { get; set; }
        public ICollection<AnswerUserResultLog> AnswerUserResultLog { get; set; }
        public ICollection<FactAnswersLog> factAnswersLog { get; set; }
        public ApplicationUsers AppUser { get; set; }
        public List<FactQuestLog> trueQuest { get; set; }
        public decimal Percent { get; set; }
    }
}
