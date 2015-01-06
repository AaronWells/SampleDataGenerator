CREATE procedure GetNextValue
	@StatTableName varchar(100), -- a table name in the 'stat' schema (excluding stat.) -- e.g.: FamilyName
	@AttributeFilters as dbo.AttributeList readonly
as
begin
	declare @command as nvarchar(max) =
	'with RankedResult 
	as
	(
		select 
			t.id, sum(t.Prop100K) over (order by t.id) as Prop100K
			from 
			(
				select 		id, sum(Prop100K) as Prop100K
				from		stat.' + @StatTableName + ' a inner join @attributeFilters b on a.Attribute = b.Attribute
				group by	id
			) t
	)
	select top 1 map.Value 
	from 
	(
		select id, Prop100k
		from RankedResult 
		where Prop100K < (select max(Prop100K) * rand() from RankedResult )
	) t join stat.' + @StatTableName + ' as map on map.id = t.id
	order by t.Prop100K desc'

	execute sp_executesql @command, N'@attributeFilters dbo.AttributeList readonly', @attributeFilters = @AttributeFilters;
end