using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_HabeisEducacional.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCamposFotoUrlCertificadoCompleto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Certificados",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CargaHoraria",
                table: "Certificados",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Documento",
                table: "Certificados",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Nivel",
                table: "Certificados",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "NotaFinal",
                table: "Certificados",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Alunos",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Certificados");

            migrationBuilder.DropColumn(
                name: "CargaHoraria",
                table: "Certificados");

            migrationBuilder.DropColumn(
                name: "Documento",
                table: "Certificados");

            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "Certificados");

            migrationBuilder.DropColumn(
                name: "NotaFinal",
                table: "Certificados");

            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Alunos");
        }
    }
}
