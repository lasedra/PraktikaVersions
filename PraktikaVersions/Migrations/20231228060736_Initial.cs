using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PraktikaVersions.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    patronymic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pkey", x => x.id);
                });

            migrationBuilder.Sql(
                "insert into \"User\" values " +
                "(default, 'Иванов', 'Иван', 'Иванович', 'ivanov@mail.ru')," +
                "(default, 'Петров', 'Пётр', 'Петрович', 'petrovi4@mail.ru')," +
                "(default, 'Фёдоров', 'Фёдор', 'Фёдорович', 'fed0r@mail.ru')," +
                "(default, 'Смирнов', 'Антон', 'Павлович', 'smirny@mail.ru')," +
                "(default, 'Столяров', 'Дмитрий', 'Алексеевич', 'dimon@mail.ru')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
