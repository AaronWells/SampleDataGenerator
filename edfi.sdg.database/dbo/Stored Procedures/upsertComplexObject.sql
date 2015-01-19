CREATE PROCEDURE [dbo].[upsertComplexObject]
	@identifier char(25),
	@className nvarchar(max),
	@xml XML
AS
BEGIN
	MERGE INTO dbo.ComplexObject target
	USING(SELECT @identifier, @className, @xml) AS source ([Identifier], [ClassName], [Xml])
	ON (target.Identifier = source.Identifier)
	WHEN MATCHED THEN
		UPDATE SET [ClassName] = source.[ClassName], [Xml]=source.[Xml]
	WHEN NOT MATCHED THEN
		INSERT (Identifier, ClassName, Xml)
		VALUES (source.Identifier, source.ClassName, source.Xml);
END;
