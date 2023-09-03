using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Infrastructure.Migrations
{
    public partial class Applicant_Vacancy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vacancy",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    RecordStatus = table.Column<short>(nullable: false),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    NameAr = table.Column<string>(maxLength: 500, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DescriptionAr = table.Column<string>(nullable: true),
                    MaxApplicant = table.Column<int>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacancy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantVacancy",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    RecordStatus = table.Column<short>(nullable: false),
                    VacancyId = table.Column<long>(nullable: false),
                    ApplicantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantVacancy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantVacancy_AspNetUsers_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicantVacancy_Vacancy_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantVacancy_ApplicantId",
                table: "ApplicantVacancy",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantVacancy_VacancyId",
                table: "ApplicantVacancy",
                column: "VacancyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicantVacancy");

            migrationBuilder.DropTable(
                name: "Vacancy");
        }
    }
}
