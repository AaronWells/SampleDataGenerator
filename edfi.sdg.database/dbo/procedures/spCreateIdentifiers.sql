CREATE PROCEDURE [dbo].[spCreateIdentifiers]
	@count INTEGER
AS
	DECLARE  @returntable TABLE
	(
		id BIGINT
	)
	;WITH cte_counter ( [Num] )
	AS
	(
		SELECT 1 as Num	UNION ALL
		SELECT Num + 1 as Num from cte_counter WHERE Num < @count
	)
	INSERT @returntable(id) SELECT NEXT VALUE FOR dbo.Sequence_Id FROM cte_counter 
	SELECT id, dbo.fnBase36(id) as eduId from @returntable
RETURN 0
