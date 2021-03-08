using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmpManagmentWebApiDAL.Migrations
{
    public partial class Employee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEmployee",
                columns: table => new
                {
                    EmpId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PinCode = table.Column<string>(nullable: true),
                    GenderId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: false),
                    ContentType = table.Column<string>(nullable: false),
                    FileExtension = table.Column<string>(nullable: false),
                    Data = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmployee", x => x.EmpId);
                    table.ForeignKey(
                        name: "FK_tblEmployee_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblEmployee_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblEmployee_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "GenderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblEmployee_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployee_CityId",
                table: "tblEmployee",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployee_CountryId",
                table: "tblEmployee",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployee_GenderId",
                table: "tblEmployee",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployee_StateId",
                table: "tblEmployee",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEmployee");
        }
    }
}
