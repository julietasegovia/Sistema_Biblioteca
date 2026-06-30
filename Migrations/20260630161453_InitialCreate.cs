using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Biblioteca_Deheza_Segovia.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadosPrestamo",
                columns: table => new
                {
                    EstadoPrestamoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosPrestamo", x => x.EstadoPrestamoId);
                });

            migrationBuilder.CreateTable(
                name: "EstadosReserva",
                columns: table => new
                {
                    EstadoReservaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosReserva", x => x.EstadoReservaId);
                });

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    ISBN = table.Column<string>(type: "TEXT", nullable: false),
                    Titulo = table.Column<string>(type: "TEXT", nullable: false),
                    Autor = table.Column<string>(type: "TEXT", nullable: false),
                    Genero = table.Column<string>(type: "TEXT", nullable: false),
                    CantidadCopias = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.ISBN);
                });

            migrationBuilder.CreateTable(
                name: "TiposSocio",
                columns: table => new
                {
                    TipoSocioId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    MaxLibros = table.Column<int>(type: "INTEGER", nullable: false),
                    DiasPrestamo = table.Column<int>(type: "INTEGER", nullable: false),
                    MultaPorDia = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposSocio", x => x.TipoSocioId);
                });

            migrationBuilder.CreateTable(
                name: "Socios",
                columns: table => new
                {
                    NroSocio = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Apellido = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false),
                    TipoSocioId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socios", x => x.NroSocio);
                    table.ForeignKey(
                        name: "FK_Socios_TiposSocio_TipoSocioId",
                        column: x => x.TipoSocioId,
                        principalTable: "TiposSocio",
                        principalColumn: "TipoSocioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    PrestamoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NroSocio = table.Column<int>(type: "INTEGER", nullable: false),
                    ISBN = table.Column<string>(type: "TEXT", nullable: false),
                    FechaPrestamo = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaDevolucion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EstadoPrestamoId = table.Column<int>(type: "INTEGER", nullable: false),
                    MontoMulta = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.PrestamoId);
                    table.ForeignKey(
                        name: "FK_Prestamos_EstadosPrestamo_EstadoPrestamoId",
                        column: x => x.EstadoPrestamoId,
                        principalTable: "EstadosPrestamo",
                        principalColumn: "EstadoPrestamoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prestamos_Libros_ISBN",
                        column: x => x.ISBN,
                        principalTable: "Libros",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prestamos_Socios_NroSocio",
                        column: x => x.NroSocio,
                        principalTable: "Socios",
                        principalColumn: "NroSocio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    ReservaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NroSocio = table.Column<int>(type: "INTEGER", nullable: false),
                    ISBN = table.Column<string>(type: "TEXT", nullable: false),
                    FechaReserva = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EstadoReservaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.ReservaId);
                    table.ForeignKey(
                        name: "FK_Reservas_EstadosReserva_EstadoReservaId",
                        column: x => x.EstadoReservaId,
                        principalTable: "EstadosReserva",
                        principalColumn: "EstadoReservaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservas_Libros_ISBN",
                        column: x => x.ISBN,
                        principalTable: "Libros",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservas_Socios_NroSocio",
                        column: x => x.NroSocio,
                        principalTable: "Socios",
                        principalColumn: "NroSocio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "EstadosPrestamo",
                columns: new[] { "EstadoPrestamoId", "Nombre" },
                values: new object[,]
                {
                    { 1, "Activo" },
                    { 2, "Devuelto" },
                    { 3, "Vencido" }
                });

            migrationBuilder.InsertData(
                table: "EstadosReserva",
                columns: new[] { "EstadoReservaId", "Nombre" },
                values: new object[,]
                {
                    { 1, "Pendiente" },
                    { 2, "Cumplida" },
                    { 3, "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "Libros",
                columns: new[] { "ISBN", "Autor", "CantidadCopias", "Genero", "Titulo" },
                values: new object[,]
                {
                    { "978-0062316097", "Yuval Noah Harari", 3, "Ensayo", "Sapiens" },
                    { "978-0345539434", "Carl Sagan", 2, "Ciencia", "Cosmos" },
                    { "978-8437604572", "Julio Cortazar", 1, "Novela", "Rayuela" }
                });

            migrationBuilder.InsertData(
                table: "TiposSocio",
                columns: new[] { "TipoSocioId", "DiasPrestamo", "MaxLibros", "MultaPorDia", "Nombre" },
                values: new object[,]
                {
                    { 1, 14, 3, 50.0, "Estandar" },
                    { 2, 21, 6, 30.0, "Premium" }
                });

            migrationBuilder.InsertData(
                table: "Socios",
                columns: new[] { "NroSocio", "Activo", "Apellido", "Email", "Nombre", "TipoSocioId" },
                values: new object[,]
                {
                    { 1001, true, "Fernandez", "lucia@mail.com", "Lucia", 1 },
                    { 1002, true, "Gomez", "martin@mail.com", "Martin", 1 },
                    { 1003, true, "Diaz", "carolina@mail.com", "Carolina", 1 },
                    { 1004, true, "Lopez", "diego@mail.com", "Diego", 2 },
                    { 1006, false, "Ramirez", "federico@mail.com", "Federico", 1 }
                });

            migrationBuilder.InsertData(
                table: "Prestamos",
                columns: new[] { "PrestamoId", "EstadoPrestamoId", "FechaDevolucion", "FechaPrestamo", "FechaVencimiento", "ISBN", "MontoMulta", "NroSocio" },
                values: new object[] { 1, 1, null, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "978-8437604572", 0.0, 1003 });

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_EstadoPrestamoId",
                table: "Prestamos",
                column: "EstadoPrestamoId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_ISBN",
                table: "Prestamos",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_NroSocio",
                table: "Prestamos",
                column: "NroSocio");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_EstadoReservaId",
                table: "Reservas",
                column: "EstadoReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_ISBN",
                table: "Reservas",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_NroSocio",
                table: "Reservas",
                column: "NroSocio");

            migrationBuilder.CreateIndex(
                name: "IX_Socios_TipoSocioId",
                table: "Socios",
                column: "TipoSocioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prestamos");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "EstadosPrestamo");

            migrationBuilder.DropTable(
                name: "EstadosReserva");

            migrationBuilder.DropTable(
                name: "Libros");

            migrationBuilder.DropTable(
                name: "Socios");

            migrationBuilder.DropTable(
                name: "TiposSocio");
        }
    }
}
