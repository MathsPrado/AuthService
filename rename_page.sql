BEGIN TRANSACTION;
ALTER TABLE [RoleScreens] DROP CONSTRAINT [FK_RoleScreens_Screens_ScreenId];

EXEC sp_rename N'[RoleScreens].[ScreenId]', N'PageId', 'COLUMN';

EXEC sp_rename N'[RoleScreens].[IX_RoleScreens_ScreenId]', N'IX_RoleScreens_PageId', 'INDEX';

ALTER TABLE [RoleScreens] ADD CONSTRAINT [FK_RoleScreens_Screens_PageId] FOREIGN KEY ([PageId]) REFERENCES [Screens] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260317015547_RenameScreenToPage', N'10.0.1');

COMMIT;
GO

