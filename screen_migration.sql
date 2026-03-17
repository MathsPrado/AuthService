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
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    CREATE TABLE [Permissions] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_Permissions] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    CREATE TABLE [Roles] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Screen] nvarchar(max) NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    CREATE TABLE [UsersSystem] (
        [Id] int NOT NULL IDENTITY,
        [Username] nvarchar(450) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_UsersSystem] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    CREATE TABLE [RolePermissions] (
        [RoleId] int NOT NULL,
        [PermissionId] int NOT NULL,
        [AssignedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([RoleId], [PermissionId]),
        CONSTRAINT [FK_RolePermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RolePermissions_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    CREATE TABLE [UserPermissions] (
        [UserId] int NOT NULL,
        [PermissionId] int NOT NULL,
        [AssignedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_UserPermissions] PRIMARY KEY ([UserId], [PermissionId]),
        CONSTRAINT [FK_UserPermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserPermissions_UsersSystem_UserId] FOREIGN KEY ([UserId]) REFERENCES [UsersSystem] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    CREATE TABLE [UserRoles] (
        [UserId] int NOT NULL,
        [RoleId] int NOT NULL,
        [AssignedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRoles_UsersSystem_UserId] FOREIGN KEY ([UserId]) REFERENCES [UsersSystem] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_RolePermissions_PermissionId] ON [RolePermissions] ([PermissionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_UserPermissions_PermissionId] ON [UserPermissions] ([PermissionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_UsersSystem_Username] ON [UsersSystem] ([Username]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'Description', N'Screen') AND [object_id] = OBJECT_ID(N'[Roles]'))
        SET IDENTITY_INSERT [Roles] ON;
    EXEC(N'INSERT INTO [Roles] ([Id], [Name], [Description], [Screen])
    VALUES (1, N''Admin'', N''Administrator role'', N''*'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'Description', N'Screen') AND [object_id] = OBJECT_ID(N'[Roles]'))
        SET IDENTITY_INSERT [Roles] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'Description') AND [object_id] = OBJECT_ID(N'[Permissions]'))
        SET IDENTITY_INSERT [Permissions] ON;
    EXEC(N'INSERT INTO [Permissions] ([Id], [Name], [Description])
    VALUES (1, N''FullAccess'', N''Grants all permissions'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'Description') AND [object_id] = OBJECT_ID(N'[Permissions]'))
        SET IDENTITY_INSERT [Permissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'PermissionId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] ON;
    EXEC(N'INSERT INTO [RolePermissions] ([RoleId], [PermissionId], [AssignedAt])
    VALUES (1, 1, ''2026-03-17T01:12:37.6095704Z'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'PermissionId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Username', N'Password') AND [object_id] = OBJECT_ID(N'[UsersSystem]'))
        SET IDENTITY_INSERT [UsersSystem] ON;
    EXEC(N'INSERT INTO [UsersSystem] ([Id], [Username], [Password])
    VALUES (1, N''admin'', N''123'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Username', N'Password') AND [object_id] = OBJECT_ID(N'[UsersSystem]'))
        SET IDENTITY_INSERT [UsersSystem] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserId', N'RoleId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[UserRoles]'))
        SET IDENTITY_INSERT [UserRoles] ON;
    EXEC(N'INSERT INTO [UserRoles] ([UserId], [RoleId], [AssignedAt])
    VALUES (1, 1, ''2026-03-17T01:12:37.6095753Z'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserId', N'RoleId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[UserRoles]'))
        SET IDENTITY_INSERT [UserRoles] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217125726_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260217125726_InitialCreate', N'10.0.1');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134249_SeedTestData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Permissions]'))
        SET IDENTITY_INSERT [Permissions] ON;
    EXEC(N'INSERT INTO [Permissions] ([Id], [Description], [Name])
    VALUES (2, NULL, N''ReadUsers''),
    (3, NULL, N''EditUsers''),
    (4, NULL, N''DeleteUsers'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Permissions]'))
        SET IDENTITY_INSERT [Permissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134249_SeedTestData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name', N'Screen') AND [object_id] = OBJECT_ID(N'[Roles]'))
        SET IDENTITY_INSERT [Roles] ON;
    EXEC(N'INSERT INTO [Roles] ([Id], [Description], [Name], [Screen])
    VALUES (2, N''Regular user'', N''User'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name', N'Screen') AND [object_id] = OBJECT_ID(N'[Roles]'))
        SET IDENTITY_INSERT [Roles] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134249_SeedTestData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Password', N'Username') AND [object_id] = OBJECT_ID(N'[UsersSystem]'))
        SET IDENTITY_INSERT [UsersSystem] ON;
    EXEC(N'INSERT INTO [UsersSystem] ([Id], [Password], [Username])
    VALUES (1, N''admin123'', N''admin''),
    (2, N''password'', N''jdoe''),
    (3, N''secret'', N''alice'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Password', N'Username') AND [object_id] = OBJECT_ID(N'[UsersSystem]'))
        SET IDENTITY_INSERT [UsersSystem] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134249_SeedTestData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionId', N'RoleId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] ON;
    EXEC(N'INSERT INTO [RolePermissions] ([PermissionId], [RoleId], [AssignedAt])
    VALUES (2, 2, ''2026-01-01T00:00:00.0000000'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionId', N'RoleId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[RolePermissions]'))
        SET IDENTITY_INSERT [RolePermissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134249_SeedTestData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionId', N'UserId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[UserPermissions]'))
        SET IDENTITY_INSERT [UserPermissions] ON;
    EXEC(N'INSERT INTO [UserPermissions] ([PermissionId], [UserId], [AssignedAt])
    VALUES (3, 3, ''2026-01-01T00:00:00.0000000'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionId', N'UserId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[UserPermissions]'))
        SET IDENTITY_INSERT [UserPermissions] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134249_SeedTestData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[UserRoles]'))
        SET IDENTITY_INSERT [UserRoles] ON;
    EXEC(N'INSERT INTO [UserRoles] ([RoleId], [UserId], [AssignedAt])
    VALUES (1, 1, ''2026-01-01T00:00:00.0000000''),
    (2, 2, ''2026-01-01T00:00:00.0000000''),
    (2, 3, ''2026-01-01T00:00:00.0000000'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[UserRoles]'))
        SET IDENTITY_INSERT [UserRoles] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134249_SeedTestData'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260217134249_SeedTestData', N'10.0.1');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134536_AddAssignedAtDefaults'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserRoles]') AND [c].[name] = N'AssignedAt');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [UserRoles] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [UserRoles] ADD DEFAULT (GETUTCDATE()) FOR [AssignedAt];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134536_AddAssignedAtDefaults'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserPermissions]') AND [c].[name] = N'AssignedAt');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [UserPermissions] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [UserPermissions] ADD DEFAULT (GETUTCDATE()) FOR [AssignedAt];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134536_AddAssignedAtDefaults'
)
BEGIN
    DECLARE @var2 nvarchar(max);
    SELECT @var2 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RolePermissions]') AND [c].[name] = N'AssignedAt');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [RolePermissions] DROP CONSTRAINT ' + @var2 + ';');
    ALTER TABLE [RolePermissions] ADD DEFAULT (GETUTCDATE()) FOR [AssignedAt];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134536_AddAssignedAtDefaults'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [AssignedAt] = ''2026-01-01T00:00:00.0000000''
    WHERE [PermissionId] = 1 AND [RoleId] = 1;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134536_AddAssignedAtDefaults'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [AssignedAt] = ''2026-01-01T00:00:00.0000000''
    WHERE [PermissionId] = 2 AND [RoleId] = 1;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134536_AddAssignedAtDefaults'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [AssignedAt] = ''2026-01-01T00:00:00.0000000''
    WHERE [PermissionId] = 3 AND [RoleId] = 1;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134536_AddAssignedAtDefaults'
)
BEGIN
    EXEC(N'UPDATE [RolePermissions] SET [AssignedAt] = ''2026-01-01T00:00:00.0000000''
    WHERE [PermissionId] = 1 AND [RoleId] = 2;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134536_AddAssignedAtDefaults'
)
BEGIN
    EXEC(N'UPDATE [UserPermissions] SET [AssignedAt] = ''2026-01-01T00:00:00.0000000''
    WHERE [PermissionId] = 2 AND [UserId] = 2;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134536_AddAssignedAtDefaults'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-01-01T00:00:00.0000000''
    WHERE [RoleId] = 1 AND [UserId] = 1;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134536_AddAssignedAtDefaults'
)
BEGIN
    EXEC(N'UPDATE [UserRoles] SET [AssignedAt] = ''2026-01-01T00:00:00.0000000''
    WHERE [RoleId] = 2 AND [UserId] = 2;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260217134536_AddAssignedAtDefaults'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260217134536_AddAssignedAtDefaults', N'10.0.1');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317011222_AddScreenAndRoleScreen'
)
BEGIN
    CREATE TABLE [Screens] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(450) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Route] nvarchar(max) NULL,
        CONSTRAINT [PK_Screens] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317011222_AddScreenAndRoleScreen'
)
BEGIN
    CREATE TABLE [RoleScreens] (
        [RoleId] int NOT NULL,
        [ScreenId] int NOT NULL,
        [AssignedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        CONSTRAINT [PK_RoleScreens] PRIMARY KEY ([RoleId], [ScreenId]),
        CONSTRAINT [FK_RoleScreens_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RoleScreens_Screens_ScreenId] FOREIGN KEY ([ScreenId]) REFERENCES [Screens] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317011222_AddScreenAndRoleScreen'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name', N'Route') AND [object_id] = OBJECT_ID(N'[Screens]'))
        SET IDENTITY_INSERT [Screens] ON;
    EXEC(N'INSERT INTO [Screens] ([Id], [Description], [Name], [Route])
    VALUES (1, N''Tela principal'', N''Dashboard'', N''/dashboard''),
    (2, N''Gerenciamento de usuários'', N''UserManagement'', N''/admin/users''),
    (3, N''Gerenciamento de roles'', N''RoleManagement'', N''/admin/roles''),
    (4, N''Relatórios'', N''Reports'', N''/reports'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name', N'Route') AND [object_id] = OBJECT_ID(N'[Screens]'))
        SET IDENTITY_INSERT [Screens] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317011222_AddScreenAndRoleScreen'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'ScreenId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[RoleScreens]'))
        SET IDENTITY_INSERT [RoleScreens] ON;
    EXEC(N'INSERT INTO [RoleScreens] ([RoleId], [ScreenId], [AssignedAt])
    VALUES (1, 1, ''2026-01-01T00:00:00.0000000''),
    (1, 2, ''2026-01-01T00:00:00.0000000''),
    (1, 3, ''2026-01-01T00:00:00.0000000''),
    (1, 4, ''2026-01-01T00:00:00.0000000''),
    (2, 1, ''2026-01-01T00:00:00.0000000'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'ScreenId', N'AssignedAt') AND [object_id] = OBJECT_ID(N'[RoleScreens]'))
        SET IDENTITY_INSERT [RoleScreens] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317011222_AddScreenAndRoleScreen'
)
BEGIN
    CREATE INDEX [IX_RoleScreens_ScreenId] ON [RoleScreens] ([ScreenId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317011222_AddScreenAndRoleScreen'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Screens_Name] ON [Screens] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260317011222_AddScreenAndRoleScreen'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260317011222_AddScreenAndRoleScreen', N'10.0.1');
END;

COMMIT;
GO

