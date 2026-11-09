IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE TABLE [Accounts] (
        [Id] int NOT NULL IDENTITY,
        [AccountName] nvarchar(max) NOT NULL,
        [AccountNumber] nvarchar(max) NOT NULL,
        [BankName] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        CONSTRAINT [PK_Accounts] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE TABLE [Customers] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(max) NOT NULL,
        [AmountConsumed] nvarchar(max) NOT NULL,
        [AmountPaid] nvarchar(max) NOT NULL,
        [Balance] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE TABLE [ExpenseTypes] (
        [Id] int NOT NULL IDENTITY,
        [ExpenseTypeName] nvarchar(max) NOT NULL,
        [ExpenseTypeDescription] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        CONSTRAINT [PK_ExpenseTypes] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE TABLE [PaymentMethods] (
        [Id] int NOT NULL IDENTITY,
        [PaymentMethodName] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [DataAdded] datetime2 NOT NULL,
        CONSTRAINT [PK_PaymentMethods] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE TABLE [ProductCategories] (
        [Id] int NOT NULL IDENTITY,
        [ProductCategoryName] nvarchar(max) NOT NULL,
        [ProductCategoryDescription] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [DataAdded] datetime2 NOT NULL,
        CONSTRAINT [PK_ProductCategories] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE TABLE [Sales] (
        [Id] int NOT NULL IDENTITY,
        [ProductName] nvarchar(max) NOT NULL,
        [ProductPrice] nvarchar(max) NOT NULL,
        [ProductQuantity] nvarchar(max) NOT NULL,
        [TotalAmount] nvarchar(max) NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [CustomerId] int NOT NULL,
        CONSTRAINT [PK_Sales] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Sales_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE TABLE [Expenses] (
        [Id] int NOT NULL IDENTITY,
        [ExpenseName] nvarchar(max) NOT NULL,
        [ExpenseAmount] nvarchar(max) NOT NULL,
        [ExpenseDate] datetime2 NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [ExpenseTypeId] int NOT NULL,
        CONSTRAINT [PK_Expenses] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Expenses_ExpenseTypes_ExpenseTypeId] FOREIGN KEY ([ExpenseTypeId]) REFERENCES [ExpenseTypes] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE TABLE [Products] (
        [Id] int NOT NULL IDENTITY,
        [ProductName] nvarchar(max) NOT NULL,
        [ProductDescription] nvarchar(max) NOT NULL,
        [ProductPrice] nvarchar(max) NOT NULL,
        [ProductQuantity] nvarchar(max) NOT NULL,
        [ProductCategoryId] int NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Products_ProductCategories_ProductCategoryId] FOREIGN KEY ([ProductCategoryId]) REFERENCES [ProductCategories] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE TABLE [Payments] (
        [Id] int NOT NULL IDENTITY,
        [PaymentName] nvarchar(max) NOT NULL,
        [PaymentAmount] nvarchar(max) NOT NULL,
        [PaymentDate] nvarchar(max) NOT NULL,
        [PaymentDescription] nvarchar(max) NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [SalesId] int NULL,
        [PaymentMethodId] int NOT NULL,
        [AccountId] int NULL,
        CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Payments_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]),
        CONSTRAINT [FK_Payments_PaymentMethods_PaymentMethodId] FOREIGN KEY ([PaymentMethodId]) REFERENCES [PaymentMethods] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Payments_Sales_SalesId] FOREIGN KEY ([SalesId]) REFERENCES [Sales] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE TABLE [Replenishments] (
        [Id] int NOT NULL IDENTITY,
        [ReplenishmentName] nvarchar(max) NOT NULL,
        [ReplenishmentAmount] nvarchar(max) NOT NULL,
        [ReplenishmentDate] nvarchar(max) NOT NULL,
        [QuantityReplenished] nvarchar(max) NOT NULL,
        [SellingPrice] nvarchar(max) NOT NULL,
        [PurchasePrice] nvarchar(max) NOT NULL,
        [totalpurchaseprice] nvarchar(max) NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [ProductId] int NOT NULL,
        CONSTRAINT [PK_Replenishments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Replenishments_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE TABLE [SalesItems] (
        [Id] int NOT NULL IDENTITY,
        [SaleItemName] nvarchar(max) NOT NULL,
        [SaleItemPrice] nvarchar(max) NOT NULL,
        [SaleItemQuantity] nvarchar(max) NOT NULL,
        [TotalSaleItemPrice] nvarchar(max) NOT NULL,
        [SaleItemCategory] nvarchar(max) NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [SalesId] int NOT NULL,
        [ProductId] int NOT NULL,
        CONSTRAINT [PK_SalesItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SalesItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_SalesItems_Sales_SalesId] FOREIGN KEY ([SalesId]) REFERENCES [Sales] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE TABLE [Stocks] (
        [Id] int NOT NULL IDENTITY,
        [StockName] nvarchar(max) NOT NULL,
        [QuantityInStock] nvarchar(max) NOT NULL,
        [QuantitySold] nvarchar(max) NOT NULL,
        [QuantityAvailable] nvarchar(max) NOT NULL,
        [Unit] nvarchar(max) NOT NULL,
        [SellingPrice] nvarchar(max) NOT NULL,
        [LastPrice] nvarchar(max) NOT NULL,
        [LastlyModified] nvarchar(max) NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [ProductId] int NOT NULL,
        CONSTRAINT [PK_Stocks] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Stocks_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Expenses_ExpenseTypeId] ON [Expenses] ([ExpenseTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_AccountId] ON [Payments] ([AccountId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_PaymentMethodId] ON [Payments] ([PaymentMethodId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Payments_SalesId] ON [Payments] ([SalesId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Products_ProductCategoryId] ON [Products] ([ProductCategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Replenishments_ProductId] ON [Replenishments] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Sales_CustomerId] ON [Sales] ([CustomerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SalesItems_ProductId] ON [SalesItems] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SalesItems_SalesId] ON [SalesItems] ([SalesId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Stocks_ProductId] ON [Stocks] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011131639_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261011131639_InitialCreate', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011140304_AddReplenishmentSupplier'
)
BEGIN
    ALTER TABLE [Replenishments] ADD [SupplierName] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011140304_AddReplenishmentSupplier'
)
BEGIN
    ALTER TABLE [Replenishments] ADD [SupplierPhone] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261011140304_AddReplenishmentSupplier'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261011140304_AddReplenishmentSupplier', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261012102739_UpdateSalesHeader'
)
BEGIN
    EXEC sp_rename N'[Sales].[TotalAmount]', N'TotalConsumed', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261012102739_UpdateSalesHeader'
)
BEGIN
    EXEC sp_rename N'[Sales].[DateAdded]', N'SaleDate', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261012102739_UpdateSalesHeader'
)
BEGIN
    ALTER TABLE [Sales] ADD [Balance] nvarchar(max) NOT NULL DEFAULT N'0';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261012102739_UpdateSalesHeader'
)
BEGIN
    ALTER TABLE [Sales] ADD [TotalPaid] nvarchar(max) NOT NULL DEFAULT N'0';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261012102739_UpdateSalesHeader'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261012102739_UpdateSalesHeader', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101091357_AddReplenishmentPaymentLink'
)
BEGIN
    ALTER TABLE [Payments] ADD [ReplenishmentId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101091357_AddReplenishmentPaymentLink'
)
BEGIN
    CREATE INDEX [IX_Payments_ReplenishmentId] ON [Payments] ([ReplenishmentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101091357_AddReplenishmentPaymentLink'
)
BEGIN
    ALTER TABLE [Payments] ADD CONSTRAINT [FK_Payments_Replenishments_ReplenishmentId] FOREIGN KEY ([ReplenishmentId]) REFERENCES [Replenishments] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101091357_AddReplenishmentPaymentLink'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261101091357_AddReplenishmentPaymentLink', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101100656_AddCustomerSex'
)
BEGIN
    ALTER TABLE [Customers] ADD [Sex] nvarchar(max) NOT NULL DEFAULT N'Unknown';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101100656_AddCustomerSex'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261101100656_AddCustomerSex', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101101400_Initial1110020Migration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261101101400_Initial1110020Migration', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    CREATE TABLE [Roles] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        [Status] int NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    CREATE TABLE [RoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] int NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    CREATE TABLE [UserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    CREATE TABLE [UserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] int NOT NULL,
        CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    CREATE TABLE [UserRoles] (
        [UserId] int NOT NULL,
        [RoleId] int NOT NULL,
        CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    CREATE TABLE [UserTokens] (
        [UserId] int NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    CREATE INDEX [IX_RoleClaims_RoleId] ON [RoleClaims] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [Roles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    CREATE INDEX [IX_UserClaims_UserId] ON [UserClaims] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    CREATE INDEX [IX_UserLogins_UserId] ON [UserLogins] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [Users] ([NormalizedEmail]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [Users] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261101120213_AddApplicationUserIdentity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261101120213_AddApplicationUserIdentity', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261102080813_AddAccountBalance'
)
BEGIN
    ALTER TABLE [Accounts] ADD [Balance] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261102080813_AddAccountBalance'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261102080813_AddAccountBalance', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261102094017_AddExpenseAccount'
)
BEGIN
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
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261102094017_AddExpenseAccount'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261102094017_AddExpenseAccount', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261103144258_AddAccountDepositHistory'
)
BEGIN
    CREATE TABLE [AccountDeposits] (
        [Id] int NOT NULL IDENTITY,
        [AccountId] int NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_AccountDeposits] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AccountDeposits_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261103144258_AddAccountDepositHistory'
)
BEGIN
    CREATE INDEX [IX_AccountDeposits_AccountId] ON [AccountDeposits] ([AccountId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261103144258_AddAccountDepositHistory'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261103144258_AddAccountDepositHistory', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261104152529_AddProductUnit'
)
BEGIN
    ALTER TABLE [Products] ADD [Unit] nvarchar(max) NOT NULL DEFAULT N'pcs';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261104152529_AddProductUnit'
)
BEGIN
    UPDATE product
    SET Unit = CASE
        WHEN LOWER(LTRIM(RTRIM(stock.Unit))) IN ('kg', 'kgs', 'kilogram', 'kilograms') THEN 'kg'
        WHEN LOWER(LTRIM(RTRIM(stock.Unit))) IN ('pc', 'pcs', 'piece', 'pieces', 'pieces (pcs)') THEN 'pcs'
        ELSE 'pcs'
    END
    FROM Products AS product
    OUTER APPLY (
        SELECT TOP (1) Unit
        FROM Stocks
        WHERE Stocks.ProductId = product.Id
        ORDER BY Stocks.DateAdded DESC, Stocks.Id DESC
    ) AS stock;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261104152529_AddProductUnit'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261104152529_AddProductUnit', N'10.0.0');
END;

COMMIT;
GO

