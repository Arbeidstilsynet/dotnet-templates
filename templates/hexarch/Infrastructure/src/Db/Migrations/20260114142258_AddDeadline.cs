using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddDeadline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Saker",
                type: "timestamp with time zone",
                nullable: true
            );

            // Make sure existing rows have a sensible deadline
            migrationBuilder.Sql(
                @"UPDATE ""Saker"" 
                  SET ""Deadline"" = ""CreatedAt"" + INTERVAL '30 days' 
                  WHERE ""Deadline"" IS NULL"
            );

            // Make column non-nullable after populating
            migrationBuilder.AlterColumn<DateTime>(
                name: "Deadline",
                table: "Saker",
                type: "timestamp with time zone",
                nullable: false
            );

            // Fix typo in column name
            migrationBuilder.RenameColumn(
                name: "Organisajonsnummer",
                table: "Saker",
                newName: "Organisasjonsnummer"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Deadline", table: "Saker");

            migrationBuilder.RenameColumn(
                name: "Organisasjonsnummer",
                table: "Saker",
                newName: "Organisajonsnummer"
            );
        }
    }
}
