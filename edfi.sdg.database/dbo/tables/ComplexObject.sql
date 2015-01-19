CREATE TABLE [dbo].[ComplexObject]
(
    [Identifier] CHAR(25) NOT NULL PRIMARY KEY,
    [ClassName] VARCHAR(MAX) NOT NULL,
    [Xml] XML NOT NULL, 
)

GO

CREATE PRIMARY XML INDEX [XML_IX_ComplexObject_Column] ON [dbo].[ComplexObject] ([Xml])

GO
