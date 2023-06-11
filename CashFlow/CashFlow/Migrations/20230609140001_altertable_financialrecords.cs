using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashFlow.Migrations
{
    public partial class altertable_financialrecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "daterecords",
                table: "financialrecords",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "account",
                keyColumn: "accountid",
                keyValue: 1,
                column: "insertdate",
                value: new DateTime(2023, 6, 9, 10, 59, 59, 471, DateTimeKind.Local).AddTicks(7221));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "daterecords",
                table: "financialrecords");

            migrationBuilder.UpdateData(
                table: "account",
                keyColumn: "accountid",
                keyValue: 1,
                column: "insertdate",
                value: new DateTime(2023, 6, 7, 9, 21, 11, 905, DateTimeKind.Local).AddTicks(6320));
        }
    }
}
