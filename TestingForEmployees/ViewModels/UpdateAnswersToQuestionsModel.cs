using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingForEmployees.ViewModels
{
    public class UpdateAnswersToQuestionsModel
    {
        public int IdQue { get; set; }
        public IList<AnsewrsAndHisSate> CollAnswers { get; set; }
    }
}
