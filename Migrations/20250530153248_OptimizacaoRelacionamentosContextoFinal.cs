using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_HabeisEducacional.Migrations
{
    /// <inheritdoc />
    public partial class OptimizacaoRelacionamentosContextoFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificados_Alunos_Aluno_ID",
                table: "Certificados");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificados_Cursos_Curso_ID",
                table: "Certificados");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Alunos_Aluno_ID",
                table: "Matriculas");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Cursos_Curso_ID",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Matriculas_Aluno_ID",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Certificados_Aluno_ID",
                table: "Certificados");

            migrationBuilder.RenameIndex(
                name: "IX_Certificados_Codigo_Validacao",
                table: "Certificados",
                newName: "IX_Certificados_CodigoValidacao_Unique");

            migrationBuilder.RenameIndex(
                name: "IX_Alunos_Email",
                table: "Alunos",
                newName: "IX_Alunos_Email_Unique");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Matriculas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Cursos",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Cursos",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "Instrutor",
                table: "Cursos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Cursos",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo_Validacao",
                table: "Certificados",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Senha",
                table: "Alunos",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Alunos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Alunos",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_Aluno_Curso_Unique",
                table: "Matriculas",
                columns: new[] { "Aluno_ID", "Curso_ID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificados_Aluno_Curso_Unique",
                table: "Certificados",
                columns: new[] { "Aluno_ID", "Curso_ID" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificados_Alunos",
                table: "Certificados",
                column: "Aluno_ID",
                principalTable: "Alunos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificados_Cursos",
                table: "Certificados",
                column: "Curso_ID",
                principalTable: "Cursos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Alunos",
                table: "Matriculas",
                column: "Aluno_ID",
                principalTable: "Alunos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Cursos",
                table: "Matriculas",
                column: "Curso_ID",
                principalTable: "Cursos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificados_Alunos",
                table: "Certificados");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificados_Cursos",
                table: "Certificados");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Alunos",
                table: "Matriculas");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Cursos",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Matriculas_Aluno_Curso_Unique",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Certificados_Aluno_Curso_Unique",
                table: "Certificados");

            migrationBuilder.RenameIndex(
                name: "IX_Certificados_CodigoValidacao_Unique",
                table: "Certificados",
                newName: "IX_Certificados_Codigo_Validacao");

            migrationBuilder.RenameIndex(
                name: "IX_Alunos_Email_Unique",
                table: "Alunos",
                newName: "IX_Alunos_Email");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Matriculas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Cursos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Cursos",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Instrutor",
                table: "Cursos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Cursos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo_Validacao",
                table: "Certificados",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Senha",
                table: "Alunos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Alunos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Alunos",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_Aluno_ID",
                table: "Matriculas",
                column: "Aluno_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Certificados_Aluno_ID",
                table: "Certificados",
                column: "Aluno_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificados_Alunos_Aluno_ID",
                table: "Certificados",
                column: "Aluno_ID",
                principalTable: "Alunos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificados_Cursos_Curso_ID",
                table: "Certificados",
                column: "Curso_ID",
                principalTable: "Cursos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Alunos_Aluno_ID",
                table: "Matriculas",
                column: "Aluno_ID",
                principalTable: "Alunos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Cursos_Curso_ID",
                table: "Matriculas",
                column: "Curso_ID",
                principalTable: "Cursos",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
