using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingForEmployees.Models.Entities;
using TestingForEmployees.Models;


namespace TestingForEmployees.ViewModels
{
    public class AccessViewModel
    {
        public ApplicationUsers User { get; set; }
        public IEnumerable<TitleUserCountAccess> AccessCollection { get; set; }
    }
}
