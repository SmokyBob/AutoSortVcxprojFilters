CREATE TABLE [dbo].[Rights] (
    [RightId]       VARCHAR (128) NOT NULL,
    [Description]   VARCHAR (250) NULL,
    [Group]         VARCHAR (100) NULL,
    [EnablingValue] INT           NOT NULL,
    [flgEditable]   INT           NOT NULL,
    CONSTRAINT [PK_Rights] PRIMARY KEY CLUSTERED ([RightId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0: visibilità in base al ruolo, 1: abilitato per tutti, altro (normalmente -1): non abilitato', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rights', @level2type = N'COLUMN', @level2name = N'EnablingValue';

