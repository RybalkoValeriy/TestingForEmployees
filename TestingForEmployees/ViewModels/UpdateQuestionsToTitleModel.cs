using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingForEmployees.ViewModels
{
    public class UpdateQuestionsToTitleModel
    {
        public int IdTitle { get; set; }
        public IList<string> Questions { get; set; }
    }
}
