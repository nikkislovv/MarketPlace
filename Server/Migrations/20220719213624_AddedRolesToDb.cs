using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "68b3e288-5034-40d2-a4ba-f622f1dadd49", "56ba57d4-3a87-4ae5-a7c5-cf1c01803bfa", "client", "CLIENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cb549e9b-178c-40bf-8830-07b1cd4f3f63", "e12287b3-1ab3-4829-a3af-314a19a9b81d", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c05a14e3-56f1-4bb1-9b6b-6dbebd651087", "fe70151e-e338-4c6f-a106-a63f0e2346a2", "seller", "SELLER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "68b3e288-5034-40d2-a4ba-f622f1dadd49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c05a14e3-56f1-4bb1-9b6b-6dbebd651087");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb549e9b-178c-40bf-8830-07b1cd4f3f63");
        }
    }
}
