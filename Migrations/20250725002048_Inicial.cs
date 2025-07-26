using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Workshop.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Workshop\";");
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Instrutor\";");
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Usuario\";");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Cpf = table.Column<string>(type: "text", nullable: false),
                    Senha = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Telefone = table.Column<string>(type: "text", nullable: false),
                    Perfil = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Cpf);
                });

            migrationBuilder.CreateTable(
                name: "Workshop",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    UsuarioCpf = table.Column<string>(type: "text", nullable: false),
                    Datas = table.Column<List<DateTime>>(type: "timestamp with time zone[]", nullable: false),
                    Categoria = table.Column<string>(type: "text", nullable: false),
                    Modalidade = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshop", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Workshop_Usuario_UsuarioCpf",
                        column: x => x.UsuarioCpf,
                        principalTable: "Usuario",
                        principalColumn: "Cpf",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workshop_UsuarioCpf",
                table: "Workshop",
                column: "UsuarioCpf");
            migrationBuilder.Sql(InserirAdministrador());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workshop");

            migrationBuilder.DropTable(
                name: "Usuario");
        }

        private string InserirAdministrador()
        {
            //Login: Admin
            //Senha: Admin
            return @"
         INSERT INTO ""Usuario"" (""Cpf"", ""Nome"", ""Email"", ""Login"", ""Senha"", ""Telefone"", ""Perfil"")
         VALUES (
             '958.854.486-63',
             'Administrador',
             'admin@workshop.com',
             'Admin',
             'AQAAAAIAAYagAAAAEHFmTj244LrioF2lYGv3T5jIXv8f6P+EiNb+Ca8YUA+W+ooUdHSkddUIsUvsqBByOg==',
             '(81) 99508-9677',
             'Administrador'
         );
     ";
        }
    }
}
