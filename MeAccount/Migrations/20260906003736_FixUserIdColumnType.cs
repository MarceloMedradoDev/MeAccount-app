using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeAccount.Migrations
{
    /// <inheritdoc />
    public partial class FixUserIdColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // PostgreSQL não converte varchar -> uuid automaticamente;
            // é necessário USING com cast explícito. Os valores existentes
            // são IDs do Identity (strings no formato GUID), então o cast é seguro.
            migrationBuilder.Sql(
                """
                ALTER TABLE "Transactions" ALTER COLUMN "UserId" TYPE uuid USING "UserId"::uuid;
                ALTER TABLE "Categories" ALTER COLUMN "UserId" TYPE uuid USING "UserId"::uuid;
                ALTER TABLE "Accounts" ALTER COLUMN "UserId" TYPE uuid USING "UserId"::uuid;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE "Transactions" ALTER COLUMN "UserId" TYPE varchar(450) USING "UserId"::varchar(450);
                ALTER TABLE "Categories" ALTER COLUMN "UserId" TYPE varchar(450) USING "UserId"::varchar(450);
                ALTER TABLE "Accounts" ALTER COLUMN "UserId" TYPE varchar(450) USING "UserId"::varchar(450);
                """);
        }
    }
}
