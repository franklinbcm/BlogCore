using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class StoredProcedurespGet_Categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string storedPro = @"create procedure Sp_Get_Category
                                as
                                begin
	                                select * from [dbo].[Categories];
                                end;

                                ";
            migrationBuilder.Sql(storedPro);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string storedPro = @"drop procedure Sp_Get_Category  ";
            migrationBuilder.Sql(storedPro);

        }
    }
}
