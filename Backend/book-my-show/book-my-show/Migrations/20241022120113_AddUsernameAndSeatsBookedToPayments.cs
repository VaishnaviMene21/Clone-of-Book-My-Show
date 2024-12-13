using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace book_my_show.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernameAndSeatsBookedToPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeatsBooked",
                table: "PaymentDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "PaymentDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatsBooked",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "PaymentDetails");
        }
    }
}
