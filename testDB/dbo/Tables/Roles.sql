CREATE TABLE [dbo].[Roles] (
    [RoleId]      VARCHAR (128) NOT NULL,
    [Description] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);

