using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dk.Odense.SSP.Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AgendaNumber = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    AgendaSent = table.Column<bool>(nullable: false),
                    MeetingHeld = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SocialBehavior = table.Column<int>(nullable: false),
                    SocialBehaviorElaborate = table.Column<string>(nullable: true),
                    PositiveSupport = table.Column<int>(nullable: false),
                    PositiveSupportElaborate = table.Column<string>(nullable: true),
                    Skills = table.Column<int>(nullable: false),
                    SkillsElaborate = table.Column<string>(nullable: true),
                    DrugRelationship = table.Column<int>(nullable: false),
                    DrugRelationshipElaborate = table.Column<string>(nullable: true),
                    GoodFriends = table.Column<int>(nullable: false),
                    GoodFriendsElaborate = table.Column<string>(nullable: true),
                    FutureDreams = table.Column<int>(nullable: false),
                    FutureDreamsElaborate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    DaysToExpire = table.Column<int>(nullable: false),
                    DeleteAfterSspEnd = table.Column<bool>(nullable: false),
                    ValidUntil = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Concerns",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CrimeConcern = table.Column<string>(nullable: true),
                    ReportedToPolice = table.Column<int>(nullable: false),
                    NotifyConcern = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concerns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groupings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groupings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoliceWorryCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ValidUntil = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliceWorryCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoliceWorryRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ValidUntil = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliceWorryRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportedPersons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReportedName = table.Column<string>(nullable: true),
                    ReportedCpr = table.Column<string>(nullable: true),
                    ReportedAdress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedPersons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reporters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Phonenumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Workplace = table.Column<string>(nullable: true),
                    ImmediateLeader = table.Column<string>(nullable: true),
                    ImmediateLeaderEmail = table.Column<string>(nullable: true),
                    ImmediateLeaderPhone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ValidUntil = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SspAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ValidUntil = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SspAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgendaItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    ProcesseDate = table.Column<DateTime>(nullable: true),
                    Agenda_Id = table.Column<Guid>(nullable: false),
                    Categorization_Id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgendaItems_Agendas_Agenda_Id",
                        column: x => x.Agenda_Id,
                        principalTable: "Agendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgendaItems_Categorizations_Categorization_Id",
                        column: x => x.Categorization_Id,
                        principalTable: "Categorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SocialSecNum = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Birthday = table.Column<string>(nullable: true),
                    SspStopDate = table.Column<DateTime>(nullable: true),
                    SspArea_Id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_SspAreas_SspArea_Id",
                        column: x => x.SspArea_Id,
                        principalTable: "SspAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Reporter = table.Column<string>(nullable: true),
                    Person_Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Persons_Person_Id",
                        column: x => x.Person_Id,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonGroupings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Person_Id = table.Column<Guid>(nullable: false),
                    Grouping_Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonGroupings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonGroupings_Groupings_Grouping_Id",
                        column: x => x.Grouping_Id,
                        principalTable: "Groupings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonGroupings_Persons_Person_Id",
                        column: x => x.Person_Id,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Robustnesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Increment = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EnrollmentPlace = table.Column<string>(nullable: true),
                    Person_Id = table.Column<Guid>(nullable: false),
                    ReportedPerson_Id = table.Column<Guid>(nullable: false),
                    Reporter_Id = table.Column<Guid>(nullable: false),
                    Assessment_Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Robustnesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Robustnesses_Assessments_Assessment_Id",
                        column: x => x.Assessment_Id,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Robustnesses_Persons_Person_Id",
                        column: x => x.Person_Id,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Robustnesses_ReportedPersons_ReportedPerson_Id",
                        column: x => x.ReportedPerson_Id,
                        principalTable: "ReportedPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Robustnesses_Reporters_Reporter_Id",
                        column: x => x.Reporter_Id,
                        principalTable: "Reporters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Worries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CrimeScene = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Processed = table.Column<DateTime>(nullable: true),
                    Groundless = table.Column<DateTime>(nullable: true),
                    Approved = table.Column<bool>(nullable: false),
                    Increment = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Concern_Id = table.Column<Guid>(nullable: true),
                    Person_Id = table.Column<Guid>(nullable: true),
                    ReportedPerson_Id = table.Column<Guid>(nullable: true),
                    Reporter_Id = table.Column<Guid>(nullable: false),
                    Assessment_Id = table.Column<Guid>(nullable: true),
                    PoliceWorryCategory_Id = table.Column<Guid>(nullable: true),
                    PoliceWorryRole_Id = table.Column<Guid>(nullable: true),
                    Source_Id = table.Column<Guid>(nullable: true),
                    AgendaItem_Id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Worries_AgendaItems_AgendaItem_Id",
                        column: x => x.AgendaItem_Id,
                        principalTable: "AgendaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Worries_Assessments_Assessment_Id",
                        column: x => x.Assessment_Id,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Worries_Concerns_Concern_Id",
                        column: x => x.Concern_Id,
                        principalTable: "Concerns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Worries_Persons_Person_Id",
                        column: x => x.Person_Id,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Worries_PoliceWorryCategories_PoliceWorryCategory_Id",
                        column: x => x.PoliceWorryCategory_Id,
                        principalTable: "PoliceWorryCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Worries_PoliceWorryRoles_PoliceWorryRole_Id",
                        column: x => x.PoliceWorryRole_Id,
                        principalTable: "PoliceWorryRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Worries_ReportedPersons_ReportedPerson_Id",
                        column: x => x.ReportedPerson_Id,
                        principalTable: "ReportedPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Worries_Reporters_Reporter_Id",
                        column: x => x.Reporter_Id,
                        principalTable: "Reporters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Worries_Sources_Source_Id",
                        column: x => x.Source_Id,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgendaItems_Agenda_Id",
                table: "AgendaItems",
                column: "Agenda_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AgendaItems_Categorization_Id",
                table: "AgendaItems",
                column: "Categorization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_Person_Id",
                table: "Notes",
                column: "Person_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PersonGroupings_Grouping_Id",
                table: "PersonGroupings",
                column: "Grouping_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PersonGroupings_Person_Id",
                table: "PersonGroupings",
                column: "Person_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_SspArea_Id",
                table: "Persons",
                column: "SspArea_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Robustnesses_Assessment_Id",
                table: "Robustnesses",
                column: "Assessment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Robustnesses_Person_Id",
                table: "Robustnesses",
                column: "Person_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Robustnesses_ReportedPerson_Id",
                table: "Robustnesses",
                column: "ReportedPerson_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Robustnesses_Reporter_Id",
                table: "Robustnesses",
                column: "Reporter_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Worries_AgendaItem_Id",
                table: "Worries",
                column: "AgendaItem_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Worries_Assessment_Id",
                table: "Worries",
                column: "Assessment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Worries_Concern_Id",
                table: "Worries",
                column: "Concern_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Worries_Person_Id",
                table: "Worries",
                column: "Person_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Worries_PoliceWorryCategory_Id",
                table: "Worries",
                column: "PoliceWorryCategory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Worries_PoliceWorryRole_Id",
                table: "Worries",
                column: "PoliceWorryRole_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Worries_ReportedPerson_Id",
                table: "Worries",
                column: "ReportedPerson_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Worries_Reporter_Id",
                table: "Worries",
                column: "Reporter_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Worries_Source_Id",
                table: "Worries",
                column: "Source_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "PersonGroupings");

            migrationBuilder.DropTable(
                name: "Robustnesses");

            migrationBuilder.DropTable(
                name: "Worries");

            migrationBuilder.DropTable(
                name: "Groupings");

            migrationBuilder.DropTable(
                name: "AgendaItems");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "Concerns");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "PoliceWorryCategories");

            migrationBuilder.DropTable(
                name: "PoliceWorryRoles");

            migrationBuilder.DropTable(
                name: "ReportedPersons");

            migrationBuilder.DropTable(
                name: "Reporters");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "Agendas");

            migrationBuilder.DropTable(
                name: "Categorizations");

            migrationBuilder.DropTable(
                name: "SspAreas");
        }
    }
}
