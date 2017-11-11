using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace TestingForEmployees.Models.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string BranchNumber { get; set; }
        public string BranchAddress { get; set; }
    }
}
