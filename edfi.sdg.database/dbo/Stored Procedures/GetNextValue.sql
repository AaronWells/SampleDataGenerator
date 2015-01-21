CREATE procedure [dbo].[GetNextValue]
	@StatTableName varchar(100), -- a table name in the 'stat' schema (excluding stat.) -- e.g.: FamilyName
	@AttributeFilters as dbo.AttributeList readonly
as
begin
	declare @filter varchar(200) = ''
	
	if(exists(select top 1 * from @AttributeFilters))
		set @filter = ' where Attribute in (select * from @AttributeFilters) '

	declare @command as nvarchar(max) =
	'with 
	RankedResult as
	(	
		select 
			t.id, sum(t.Prop100K) over (order by t.id) as Prop100K
		from 
		(
			select 		id, sum(Prop100K) as Prop100K
			from		stat.' + @StatTableName + @filter + '
			group by	id
		) t
	)
	,
	aggregates as
	(
		select 
			max(Prop100K) as maxValue,
			max(Prop100K) * rand() as randValue
		from RankedResult 
	)
	select top 1 Value
	from RankedResult join stat.' + @StatTableName + ' as map 
		on map.id = RankedResult.id
	where RankedResult.Prop100K >= (select randValue from aggregates)
	order by RankedResult.Prop100K'

	execute sp_executesql @command, N'@attributeFilters dbo.AttributeList readonly', @attributeFilters = @AttributeFilters;

/*
	declare @t as dbo.AttributeList
	insert into @t values('F')
	exec GetNextValue 'GivenName', @t
*/
end
