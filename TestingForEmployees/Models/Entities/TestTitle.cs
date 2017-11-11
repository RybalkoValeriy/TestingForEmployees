using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingForEmployees.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;


namespace TestingForEmployees.Models.Entities
{
    public class TestTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateAdd { get; set; }
        public int CountTestQuestion { get; set; }
        public bool WorkStateTitle { get; set; }
        public DateTime? DateCloseTitle { get; set; }
        public ICollection<TitleUserCountAccess> TITLEUserCountId { get; set; }
        public ICollection<TestQuestions> QuestionsId { get; set; }

        public TestTitle()
        {
            DateAdd = DateTime.Now;
            CountTestQuestion = 0;
            WorkStateTitle = true;
        }
    }
}
