using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TestingForEmployees.Migrations
{
    public partial class Migr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BranchAddress = table.Column<string>(nullable: true),
                    BranchNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestTitle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountTestQuestion = table.Column<int>(nullable: false),
                    DateAdd = table.Column<DateTime>(nullable: false),
                    DateCloseTitle = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    WorkStateTitle = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTitle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    BranchId = table.Column<int>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    DateReg = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdd = table.Column<DateTime>(nullable: false),
                    DateCloseQuestion = table.Column<DateTime>(nullable: true),
                    Questions = table.Column<string>(nullable: true),
                    TitleId = table.Column<int>(nullable: true),
                    WorkStateQuestion = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestQuestions_TestTitle_TitleId",
                        column: x => x.TitleId,
                        principalTable: "TestTitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TitleUserCountAccess",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Attempts = table.Column<int>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: true),
                    State = table.Column<bool>(nullable: false),
                    TitleId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleUserCountAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TitleUserCountAccess_TestTitle_TitleId",
                        column: x => x.TitleId,
                        principalTable: "TestTitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TitleUserCountAccess_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdd = table.Column<DateTime>(nullable: false),
                    DateCloseAnswers = table.Column<DateTime>(nullable: true),
                    State = table.Column<bool>(nullable: false),
                    TestQuestionsId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    WorkStateAnswers = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAnswers_TestQuestions_TestQuestionsId",
                        column: x => x.TestQuestionsId,
                        principalTable: "TestQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StartedTestLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateStarted = table.Column<DateTime>(nullable: false),
                    IP = table.Column<string>(nullable: true),
                    State = table.Column<bool>(nullable: false),
                    TitleUserAccessId = table.Column<int>(nullable: true),
                    UserIdId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartedTestLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StartedTestLog_TitleUserCountAccess_TitleUserAccessId",
                        column: x => x.TitleUserAccessId,
                        principalTable: "TitleUserCountAccess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StartedTestLog_AspNetUsers_UserIdId",
                        column: x => x.UserIdId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FactTitleLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdd = table.Column<DateTime>(nullable: false),
                    StartedTestLogId = table.Column<int>(nullable: true),
                    TitleText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactTitleLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactTitleLog_StartedTestLog_StartedTestLogId",
                        column: x => x.StartedTestLogId,
                        principalTable: "StartedTestLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResultReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAnsw = table.Column<DateTime>(nullable: false),
                    FactTitleLogId = table.Column<int>(nullable: true),
                    Percent = table.Column<decimal>(nullable: false),
                    StartedTestLogId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultReports_FactTitleLog_FactTitleLogId",
                        column: x => x.FactTitleLogId,
                        principalTable: "FactTitleLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultReports_StartedTestLog_StartedTestLogId",
                        column: x => x.StartedTestLogId,
                        principalTable: "StartedTestLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultReports_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FactQuestLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdd = table.Column<DateTime>(nullable: false),
                    FactTitleLogId = table.Column<int>(nullable: true),
                    QuestText = table.Column<string>(nullable: true),
                    ResultReportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactQuestLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactQuestLog_FactTitleLog_FactTitleLogId",
                        column: x => x.FactTitleLogId,
                        principalTable: "FactTitleLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FactQuestLog_ResultReports_ResultReportId",
                        column: x => x.ResultReportId,
                        principalTable: "ResultReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FactAnswersLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnswersText = table.Column<string>(nullable: true),
                    DateAdd = table.Column<DateTime>(nullable: false),
                    FactQuestLogId = table.Column<int>(nullable: true),
                    ResultReportId = table.Column<int>(nullable: true),
                    State = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactAnswersLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactAnswersLog_FactQuestLog_FactQuestLogId",
                        column: x => x.FactQuestLogId,
                        principalTable: "FactQuestLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FactAnswersLog_ResultReports_ResultReportId",
                        column: x => x.ResultReportId,
                        principalTable: "ResultReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnswerUserResultLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAnsw = table.Column<DateTime>(nullable: false),
                    ResultReportId = table.Column<int>(nullable: true),
                    State = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    factAnswersLogId = table.Column<int>(nullable: true),
                    factQuestLogId = table.Column<int>(nullable: true),
                    factTitleLogId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerUserResultLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerUserResultLog_ResultReports_ResultReportId",
                        column: x => x.ResultReportId,
                        principalTable: "ResultReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerUserResultLog_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerUserResultLog_FactAnswersLog_factAnswersLogId",
                        column: x => x.factAnswersLogId,
                        principalTable: "FactAnswersLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerUserResultLog_FactQuestLog_factQuestLogId",
                        column: x => x.factQuestLogId,
                        principalTable: "FactQuestLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerUserResultLog_FactTitleLog_factTitleLogId",
                        column: x => x.factTitleLogId,
                        principalTable: "FactTitleLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BranchId",
                table: "AspNetUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUserResultLog_ResultReportId",
                table: "AnswerUserResultLog",
                column: "ResultReportId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUserResultLog_UserId",
                table: "AnswerUserResultLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUserResultLog_factAnswersLogId",
                table: "AnswerUserResultLog",
                column: "factAnswersLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUserResultLog_factQuestLogId",
                table: "AnswerUserResultLog",
                column: "factQuestLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUserResultLog_factTitleLogId",
                table: "AnswerUserResultLog",
                column: "factTitleLogId");

            migrationBuilder.CreateIndex(
                name: "IX_FactAnswersLog_FactQuestLogId",
                table: "FactAnswersLog",
                column: "FactQuestLogId");

            migrationBuilder.CreateIndex(
                name: "IX_FactAnswersLog_ResultReportId",
                table: "FactAnswersLog",
                column: "ResultReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FactQuestLog_FactTitleLogId",
                table: "FactQuestLog",
                column: "FactTitleLogId");

            migrationBuilder.CreateIndex(
                name: "IX_FactQuestLog_ResultReportId",
                table: "FactQuestLog",
                column: "ResultReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FactTitleLog_StartedTestLogId",
                table: "FactTitleLog",
                column: "StartedTestLogId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultReports_FactTitleLogId",
                table: "ResultReports",
                column: "FactTitleLogId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultReports_StartedTestLogId",
                table: "ResultReports",
                column: "StartedTestLogId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultReports_UserId",
                table: "ResultReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StartedTestLog_TitleUserAccessId",
                table: "StartedTestLog",
                column: "TitleUserAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_StartedTestLog_UserIdId",
                table: "StartedTestLog",
                column: "UserIdId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAnswers_TestQuestionsId",
                table: "TestAnswers",
                column: "TestQuestionsId");

            migrationBuilder.CreateIndex(
                name: "IX_TestQuestions_TitleId",
                table: "TestQuestions",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_TitleUserCountAccess_TitleId",
                table: "TitleUserCountAccess",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_TitleUserCountAccess_UserId",
                table: "TitleUserCountAccess",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AnswerUserResultLog");

            migrationBuilder.DropTable(
                name: "TestAnswers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "FactAnswersLog");

            migrationBuilder.DropTable(
                name: "TestQuestions");

            migrationBuilder.DropTable(
                name: "FactQuestLog");

            migrationBuilder.DropTable(
                name: "ResultReports");

            migrationBuilder.DropTable(
                name: "FactTitleLog");

            migrationBuilder.DropTable(
                name: "StartedTestLog");

            migrationBuilder.DropTable(
                name: "TitleUserCountAccess");

            migrationBuilder.DropTable(
                name: "TestTitle");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Branches");
        }
    }
}
