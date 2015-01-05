CREATE TABLE [stat].[FamilyName] (
    [Id]        BIGINT         NOT NULL,
    [Value]     NVARCHAR (MAX) NOT NULL,
    [Attribute] NVARCHAR (400) NOT NULL,
    [Prop100k]  DECIMAL (8, 2) NOT NULL
);





GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FamilyName_Id_Attribute]
    ON [stat].[FamilyName]([Id] ASC, [Attribute] ASC);


GO
CREATE CLUSTERED INDEX [IX_FamilyName_Id]
    ON [stat].[FamilyName]([Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FamilyName_Attribute]
    ON [stat].[FamilyName]([Attribute] ASC);

