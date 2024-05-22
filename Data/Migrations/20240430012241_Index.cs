using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 create index if not exists user_birth_date_without_year
                                 on "Users" 
                                 (
                                    date_part('month', "BirthDate"), 
                                    date_part('day', "BirthDate")
                                 );
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "user_birth_date_without_year",
                table: "Users"
            );
        }
    }
}
