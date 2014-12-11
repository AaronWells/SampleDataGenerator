CREATE TABLE [dbo].[ComplexObject]
(
	[Id] CHAR(10) NOT NULL PRIMARY KEY,
	[ComplexObjectClassId] BIGINT NOT NULL,
    [Xml] XML NOT NULL, 
    CONSTRAINT [FK_ComplexObject_ToTable] FOREIGN KEY ([ComplexObjectClassId]) REFERENCES [ComplexObjectClass]([Id])
)

GO

CREATE PRIMARY XML INDEX [XML_IX_ComplexObject_Column] ON [dbo].[ComplexObject] ([Xml])

GO
