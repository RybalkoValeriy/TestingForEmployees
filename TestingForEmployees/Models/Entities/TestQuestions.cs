using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingForEmployees.Models.Entities
{
    public class TestQuestions
    {
        public int Id { get; set; }
        public string Questions { get; set; }
        public TestTitle Title { get; set; }
        public bool WorkStateQuestion { get; set; }
        public ICollection<TestAnswers> TestAnswers { get; set; }
        public DateTime DateAdd { get; set; }
        public DateTime? DateCloseQuestion { get; set; }

        public TestQuestions()
        {
            DateAdd = DateTime.Now;
            WorkStateQuestion = true;

        }
    }
}
