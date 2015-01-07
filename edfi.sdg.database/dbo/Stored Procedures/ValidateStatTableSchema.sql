
create procedure dbo.ValidateStatTableSchema
	@errorLevel int output
as
	set @errorLevel = 0
	declare @statTableName as varchar(100)
	declare @columnName as varchar(100)
	declare @matchCount as int 

	declare tableCursor cursor for 
	select table_name from information_schema.tables where table_schema = 'stat'

	open tableCursor
	fetch next from tableCursor into @statTableName

	while @@fetch_status = 0
	begin
		declare columnCursor cursor for 
		select column_name from information_schema.columns where table_schema = 'dbo' and table_name = 'StatTemplate'
		open columnCursor
		fetch next from columnCursor into @columnName 

		while @@fetch_status = 0
		begin
			-- compare column in Template table and stat table
			select @matchCount = count(*) from 
			(select * from information_schema.columns where table_schema = 'dbo' and TABLE_NAME = 'StatTemplate' and column_name = @columnName) as template
			inner join 
			(select * from information_schema.columns where table_schema = 'stat' and table_name = @statTableName and column_name = @columnName) as statTable
			on 
				template.TABLE_CATALOG = statTable.TABLE_CATALOG and 
				template.ORDINAL_POSITION = statTable.ORDINAL_POSITION and 
				template.COLUMN_DEFAULT is null and statTable.COLUMN_DEFAULT is null and 
				template.IS_NULLABLE = statTable.IS_NULLABLE and 
				template.DATA_TYPE = statTable.DATA_TYPE and 
				isnull(template.CHARACTER_MAXIMUM_LENGTH, -5) = isnull(statTable.CHARACTER_MAXIMUM_LENGTH, -5) and 
				isnull(template.CHARACTER_OCTET_LENGTH, -5) = isnull(statTable.CHARACTER_OCTET_LENGTH, -5) and 
				isnull(template.NUMERIC_PRECISION, 0) = isnull(statTable.NUMERIC_PRECISION, 0) and 
				isnull(template.NUMERIC_PRECISION_RADIX, -5) = isnull(statTable.NUMERIC_PRECISION_RADIX, -5) and 
				isnull(template.NUMERIC_SCALE, -5) = isnull(statTable.NUMERIC_SCALE, -5) and 
				isnull(template.DATETIME_PRECISION, -5) = isnull(statTable.DATETIME_PRECISION, -5)  

			if @matchCount = 0 
			begin
				set @errorLevel = @errorLevel + 1
				print 'mistmatch found in stat tables schema structure:'
				print '-- table: ''stat.' + @statTableName + ''''
				print '-- column: ''' + @columnName + ''''
			end

			fetch next from columnCursor into @columnName 
		end
		close columnCursor;
		deallocate columnCursor;

		fetch next from tableCursor into @statTableName
	end 

	close tableCursor;
	deallocate tableCursor;

	if @errorLevel = 0
		print 'stat tables structure has been verified'

	return @errorLevel

/*
	declare @errorLevel int
	exec ValidateStatTableSchema  @errorLevel output
	print @errorLevel
*/