using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingForEmployees.Models.Entities
{
    public class TestAnswers
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TestQuestions TestQuestions { get; set; }
        public bool State { get; set; }
        public DateTime DateAdd { get; set; }
        public bool WorkStateAnswers { get; set; }
        public DateTime? DateCloseAnswers { get; set; }

        public TestAnswers()
        {
            DateAdd = DateTime.Now;
            WorkStateAnswers = true;
        }
    }
}
