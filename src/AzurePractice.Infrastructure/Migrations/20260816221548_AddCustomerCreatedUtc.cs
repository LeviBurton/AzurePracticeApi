using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzurePractice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerCreatedUtc : Migration
    {
        /// <inheritdoc />
       protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<DateTime>(
        name: "CreatedUtc",
        table: "Customers",
        type: "datetime2",
        nullable: true);

    migrationBuilder.Sql(
        "UPDATE Customers SET CreatedUtc = SYSUTCDATETIME() WHERE CreatedUtc IS NULL");

    migrationBuilder.AlterColumn<DateTime>(
        name: "CreatedUtc",
        table: "Customers",
        type: "datetime2",
        nullable: false,
        oldClrType: typeof(DateTime),
        oldType: "datetime2",
        oldNullable: true);
}
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Customers");
        }
    }
}
