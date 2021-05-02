using Microsoft.EntityFrameworkCore.Migrations;

namespace Avalentini.Expensi.Api.Migrations
{
    public partial class FeedTestExpenses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[Expenses] ([Amount], [When], [Where], [What], [CreationDate]) VALUES ('100.00', '20120618 13:34:09', 'Somewhere', 'Something', GETDATE())");
            migrationBuilder.Sql("INSERT INTO [dbo].[Expenses] ([Amount], [When], [Where], [What], [CreationDate]) VALUES ('145.22', '20130711 18:22:12', 'SomewhereElse', 'SomethingElse', GETDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
