CREATE TABLE [dbo].[Roles_Rights] (
    [RoleId]  VARCHAR (128) NOT NULL,
    [RightId] VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_Roles_Rights] PRIMARY KEY CLUSTERED ([RoleId] ASC, [RightId] ASC),
    CONSTRAINT [FK_Roles_Rights_Rights] FOREIGN KEY ([RightId]) REFERENCES [dbo].[Rights] ([RightId]),
    CONSTRAINT [FK_Roles_Rights_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([RoleId])
);

