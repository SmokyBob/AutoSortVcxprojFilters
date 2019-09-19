CREATE TABLE [dbo].[Parametri] (
    [Codice]       NVARCHAR (50)   NOT NULL,
    [Valore]       NVARCHAR (2000) NULL,
    [Tipo]         NVARCHAR (50)   NULL,
    [Descrizione]  NVARCHAR (200)  NOT NULL,
    [Gruppo]       NVARCHAR (50)   NULL,
    [flgEditabile] BIT             CONSTRAINT [DF_Parametri_flgEditabile] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Parametri_1] PRIMARY KEY CLUSTERED ([Codice] ASC)
);

