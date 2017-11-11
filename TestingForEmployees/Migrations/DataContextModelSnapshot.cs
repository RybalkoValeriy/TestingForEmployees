using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TestingForEmployees.Models;

namespace TestingForEmployees.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TestingForEmployees.Models.ApplicationUsers", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int?>("BranchId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DateReg");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("MiddleName");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.AnswerUserResultLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAnsw");

                    b.Property<int?>("ResultReportId");

                    b.Property<bool>("State");

                    b.Property<string>("UserId");

                    b.Property<int?>("factAnswersLogId");

                    b.Property<int?>("factQuestLogId");

                    b.Property<int?>("factTitleLogId");

                    b.HasKey("Id");

                    b.HasIndex("ResultReportId");

                    b.HasIndex("UserId");

                    b.HasIndex("factAnswersLogId");

                    b.HasIndex("factQuestLogId");

                    b.HasIndex("factTitleLogId");

                    b.ToTable("AnswerUserResultLog");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BranchAddress");

                    b.Property<string>("BranchNumber");

                    b.HasKey("Id");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.FactAnswersLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AnswersText");

                    b.Property<DateTime>("DateAdd");

                    b.Property<int?>("FactQuestLogId");

                    b.Property<int?>("ResultReportId");

                    b.Property<bool>("State");

                    b.HasKey("Id");

                    b.HasIndex("FactQuestLogId");

                    b.HasIndex("ResultReportId");

                    b.ToTable("FactAnswersLog");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.FactQuestLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdd");

                    b.Property<int?>("FactTitleLogId");

                    b.Property<string>("QuestText");

                    b.Property<int?>("ResultReportId");

                    b.HasKey("Id");

                    b.HasIndex("FactTitleLogId");

                    b.HasIndex("ResultReportId");

                    b.ToTable("FactQuestLog");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.FactTitleLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdd");

                    b.Property<int?>("StartedTestLogId");

                    b.Property<string>("TitleText");

                    b.HasKey("Id");

                    b.HasIndex("StartedTestLogId");

                    b.ToTable("FactTitleLog");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.ResultReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAnsw");

                    b.Property<int?>("FactTitleLogId");

                    b.Property<decimal>("Percent");

                    b.Property<int?>("StartedTestLogId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("FactTitleLogId");

                    b.HasIndex("StartedTestLogId");

                    b.HasIndex("UserId");

                    b.ToTable("ResultReports");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.StartedTestLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateStarted");

                    b.Property<string>("IP");

                    b.Property<bool>("State");

                    b.Property<int?>("TitleUserAccessId");

                    b.Property<string>("UserIdId");

                    b.HasKey("Id");

                    b.HasIndex("TitleUserAccessId");

                    b.HasIndex("UserIdId");

                    b.ToTable("StartedTestLog");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.TestAnswers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdd");

                    b.Property<DateTime?>("DateCloseAnswers");

                    b.Property<bool>("State");

                    b.Property<int?>("TestQuestionsId");

                    b.Property<string>("Title");

                    b.Property<bool>("WorkStateAnswers");

                    b.HasKey("Id");

                    b.HasIndex("TestQuestionsId");

                    b.ToTable("TestAnswers");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.TestQuestions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdd");

                    b.Property<DateTime?>("DateCloseQuestion");

                    b.Property<string>("Questions");

                    b.Property<int?>("TitleId");

                    b.Property<bool>("WorkStateQuestion");

                    b.HasKey("Id");

                    b.HasIndex("TitleId");

                    b.ToTable("TestQuestions");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.TestTitle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CountTestQuestion");

                    b.Property<DateTime>("DateAdd");

                    b.Property<DateTime?>("DateCloseTitle");

                    b.Property<string>("Title");

                    b.Property<bool>("WorkStateTitle");

                    b.HasKey("Id");

                    b.ToTable("TestTitle");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.TitleUserCountAccess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Attempts");

                    b.Property<DateTime?>("DateStart");

                    b.Property<bool>("State");

                    b.Property<int?>("TitleId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TitleId");

                    b.HasIndex("UserId");

                    b.ToTable("TitleUserCountAccess");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TestingForEmployees.Models.ApplicationUsers")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TestingForEmployees.Models.ApplicationUsers")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TestingForEmployees.Models.ApplicationUsers")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TestingForEmployees.Models.ApplicationUsers", b =>
                {
                    b.HasOne("TestingForEmployees.Models.Entities.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("BranchId");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.AnswerUserResultLog", b =>
                {
                    b.HasOne("TestingForEmployees.Models.Entities.ResultReport", "ResultReport")
                        .WithMany("AnswerUserResultLog")
                        .HasForeignKey("ResultReportId");

                    b.HasOne("TestingForEmployees.Models.ApplicationUsers", "User")
                        .WithMany("AnswerUserResultLog")
                        .HasForeignKey("UserId");

                    b.HasOne("TestingForEmployees.Models.Entities.FactAnswersLog", "factAnswersLog")
                        .WithMany("AnswerUserResultLog")
                        .HasForeignKey("factAnswersLogId");

                    b.HasOne("TestingForEmployees.Models.Entities.FactQuestLog", "factQuestLog")
                        .WithMany("AnswerUserResultLog")
                        .HasForeignKey("factQuestLogId");

                    b.HasOne("TestingForEmployees.Models.Entities.FactTitleLog", "factTitleLog")
                        .WithMany("AnswerUserResultLog")
                        .HasForeignKey("factTitleLogId");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.FactAnswersLog", b =>
                {
                    b.HasOne("TestingForEmployees.Models.Entities.FactQuestLog", "FactQuestLog")
                        .WithMany("FactAnswersLog")
                        .HasForeignKey("FactQuestLogId");

                    b.HasOne("TestingForEmployees.Models.Entities.ResultReport", "ResultReport")
                        .WithMany("FactAnswersLog")
                        .HasForeignKey("ResultReportId");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.FactQuestLog", b =>
                {
                    b.HasOne("TestingForEmployees.Models.Entities.FactTitleLog", "FactTitleLog")
                        .WithMany("FactQuestCollection")
                        .HasForeignKey("FactTitleLogId");

                    b.HasOne("TestingForEmployees.Models.Entities.ResultReport", "ResultReport")
                        .WithMany("FactQuestLogTrue")
                        .HasForeignKey("ResultReportId");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.FactTitleLog", b =>
                {
                    b.HasOne("TestingForEmployees.Models.Entities.StartedTestLog", "StartedTestLog")
                        .WithMany("FactTitleLogCollection")
                        .HasForeignKey("StartedTestLogId");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.ResultReport", b =>
                {
                    b.HasOne("TestingForEmployees.Models.Entities.FactTitleLog", "FactTitleLog")
                        .WithMany()
                        .HasForeignKey("FactTitleLogId");

                    b.HasOne("TestingForEmployees.Models.Entities.StartedTestLog", "StartedTestLog")
                        .WithMany()
                        .HasForeignKey("StartedTestLogId");

                    b.HasOne("TestingForEmployees.Models.ApplicationUsers", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.StartedTestLog", b =>
                {
                    b.HasOne("TestingForEmployees.Models.Entities.TitleUserCountAccess", "TitleUserAccess")
                        .WithMany("StartedTestLogCollection")
                        .HasForeignKey("TitleUserAccessId");

                    b.HasOne("TestingForEmployees.Models.ApplicationUsers", "UserId")
                        .WithMany("StartedTestLogId")
                        .HasForeignKey("UserIdId");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.TestAnswers", b =>
                {
                    b.HasOne("TestingForEmployees.Models.Entities.TestQuestions", "TestQuestions")
                        .WithMany("TestAnswers")
                        .HasForeignKey("TestQuestionsId");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.TestQuestions", b =>
                {
                    b.HasOne("TestingForEmployees.Models.Entities.TestTitle", "Title")
                        .WithMany("QuestionsId")
                        .HasForeignKey("TitleId");
                });

            modelBuilder.Entity("TestingForEmployees.Models.Entities.TitleUserCountAccess", b =>
                {
                    b.HasOne("TestingForEmployees.Models.Entities.TestTitle", "Title")
                        .WithMany("TITLEUserCountId")
                        .HasForeignKey("TitleId");

                    b.HasOne("TestingForEmployees.Models.ApplicationUsers", "User")
                        .WithMany("TitleUSERCountId")
                        .HasForeignKey("UserId");
                });
        }
    }
}
