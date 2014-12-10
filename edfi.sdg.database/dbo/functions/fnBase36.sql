CREATE FUNCTION [dbo].[fnBase36]
(
    @Val BIGINT
)
RETURNS VARCHAR(10)
AS
BEGIN
	DECLARE @result VARCHAR(10) = ''
	DECLARE @chars as VARCHAR(max) = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ'
	DECLARE @base as INT = LEN(@chars)
 
	IF(@val < 0)
		BEGIN
			RETURN '0'
		END

	WHILE(@val > 0)
		BEGIN
			SET @result =  SUBSTRING(@chars, (@val % @base) + 1, 1) + @result
			SET @val = FLOOR(@val / @base) 
		END
	SET @result = RIGHT('000000000' + @result, 9)
	SET @result = @result + SUBSTRING(@chars, (ABS(CHECKSUM(@result)) % @base) + 1,1) 

    RETURN @result
END
