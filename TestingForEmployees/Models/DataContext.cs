using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestingForEmployees.Models;
using TestingForEmployees.Models.Entities;

namespace TestingForEmployees.Models
{
    public class DataContext : IdentityDbContext<ApplicationUsers>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<TestTitle> TestTitle { get; set; }
        public DbSet<TestQuestions> TestQuestions { get; set; }
        public DbSet<TestAnswers> TestAnswers { get; set; }
        public DbSet<TitleUserCountAccess> TitleUserCountAccess { get; set; }
        public DbSet<StartedTestLog> StartedTestLog { get; set; }
        public DbSet<FactTitleLog> FactTitleLog { get; set; }
        public DbSet<FactQuestLog> FactQuestLog { get; set; }
        public DbSet<FactAnswersLog> FactAnswersLog { get; set; }
        public DbSet<AnswerUserResultLog> AnswerUserResultLog { get; set; }
        public DbSet<ResultReport> ResultReports { get; set; }

    }
}
