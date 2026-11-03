using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH(N'dbo.Expenses', N'AccountId') IS NULL
                    ALTER TABLE [dbo].[Expenses] ADD [AccountId] int NULL;

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE [name] = N'IX_Expenses_AccountId'
                      AND [object_id] = OBJECT_ID(N'dbo.Expenses'))
                    CREATE INDEX [IX_Expenses_AccountId] ON [dbo].[Expenses] ([AccountId]);

                IF NOT EXISTS (
                    SELECT 1 FROM sys.foreign_keys
                    WHERE [name] = N'FK_Expenses_Accounts_AccountId')
                    ALTER TABLE [dbo].[Expenses]
                    ADD CONSTRAINT [FK_Expenses_Accounts_AccountId]
                    FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([Id]);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1 FROM sys.foreign_keys
                    WHERE [name] = N'FK_Expenses_Accounts_AccountId')
                    ALTER TABLE [dbo].[Expenses]
                    DROP CONSTRAINT [FK_Expenses_Accounts_AccountId];

                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE [name] = N'IX_Expenses_AccountId'
                      AND [object_id] = OBJECT_ID(N'dbo.Expenses'))
                    DROP INDEX [IX_Expenses_AccountId] ON [dbo].[Expenses];

                IF COL_LENGTH(N'dbo.Expenses', N'AccountId') IS NOT NULL
                    ALTER TABLE [dbo].[Expenses] DROP COLUMN [AccountId];
                """);
        }
    }
}
