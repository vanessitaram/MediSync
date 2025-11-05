using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediSync.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaDepartamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departamento",
                columns: table => new
                {
                    Id_Departamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamento", x => x.Id_Departamento);
                });

            migrationBuilder.CreateTable(
                name: "Paciente",
                columns: table => new
                {
                    Id_Paciente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Edad = table.Column<int>(type: "int", nullable: true),
                    Sexo = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Telefono = table.Column<long>(type: "bigint", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Contraseña = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    NombreContacto = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TelefonoContacto = table.Column<long>(type: "bigint", nullable: true),
                    ParentescoContacto = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Fecha_Registro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paciente", x => x.Id_Paciente);
                });

            migrationBuilder.CreateTable(
                name: "Medico",
                columns: table => new
                {
                    Id_Medico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ContrasenaHash = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    EsActivo = table.Column<bool>(type: "bit", nullable: false),
                    Id_Departamento = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medico", x => x.Id_Medico);
                    table.ForeignKey(
                        name: "FK_Medico_Departamento_Id_Departamento",
                        column: x => x.Id_Departamento,
                        principalTable: "Departamento",
                        principalColumn: "Id_Departamento");
                });

            migrationBuilder.CreateTable(
                name: "Expediente",
                columns: table => new
                {
                    Id_Expediente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Paciente = table.Column<int>(type: "int", nullable: false),
                    Fecha_Ingreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora_Ingreso = table.Column<TimeSpan>(type: "time", nullable: false),
                    Sintomas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    Id_Medico = table.Column<int>(type: "int", nullable: true),
                    Id_Departamento = table.Column<int>(type: "int", nullable: true),
                    Prediagnostico = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expediente", x => x.Id_Expediente);
                    table.ForeignKey(
                        name: "FK_Expediente_Departamento_Id_Departamento",
                        column: x => x.Id_Departamento,
                        principalTable: "Departamento",
                        principalColumn: "Id_Departamento");
                    table.ForeignKey(
                        name: "FK_Expediente_Medico_Id_Medico",
                        column: x => x.Id_Medico,
                        principalTable: "Medico",
                        principalColumn: "Id_Medico");
                    table.ForeignKey(
                        name: "FK_Expediente_Paciente_Id_Paciente",
                        column: x => x.Id_Paciente,
                        principalTable: "Paciente",
                        principalColumn: "Id_Paciente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expediente_Id_Departamento",
                table: "Expediente",
                column: "Id_Departamento");

            migrationBuilder.CreateIndex(
                name: "IX_Expediente_Id_Medico",
                table: "Expediente",
                column: "Id_Medico");

            migrationBuilder.CreateIndex(
                name: "IX_Expediente_Id_Paciente",
                table: "Expediente",
                column: "Id_Paciente");

            migrationBuilder.CreateIndex(
                name: "IX_Medico_Id_Departamento",
                table: "Medico",
                column: "Id_Departamento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expediente");

            migrationBuilder.DropTable(
                name: "Medico");

            migrationBuilder.DropTable(
                name: "Paciente");

            migrationBuilder.DropTable(
                name: "Departamento");
        }
    }
}
