using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class Added_ProjectId_To_ExpenseReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "ExpenseReports",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkedHours",
                table: "ExpenseItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ExpenseReports");

            migrationBuilder.DropColumn(
                name: "WorkedHours",
                table: "ExpenseItems");
        }
    }
}
