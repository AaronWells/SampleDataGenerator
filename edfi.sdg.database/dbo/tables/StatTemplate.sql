CREATE TABLE [dbo].[StatTemplate]
(
    [Id]        BIGINT         NOT NULL,
    [Value]     NVARCHAR (MAX) NOT NULL,
    [Attribute] NVARCHAR (400) NOT NULL,
    [Prop100k]  DECIMAL (8, 2) NOT NULL
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_StatTemplate_Id_Attribute]
    ON [dbo].[StatTemplate]([Id] ASC, [Attribute] ASC);


GO
CREATE CLUSTERED INDEX [IX_StatTemplate_Id]
    ON [dbo].[StatTemplate]([Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StatTemplate_Attribute]
    ON [dbo].[StatTemplate]([Attribute] ASC);

