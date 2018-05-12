CREATE PROC [DBO].[SP_KIEMTRAMAVTCTDDH]
@MasoDDH nchar(8),
@MAVT nchar(4)
AS
BEGIN
	if exists(select * from dbo.CTDDH where CTDDH.MasoDDH = @MasoDDH and CTDDH.MAVT = @MAVT)
	begin
		return 1
	end
	return 0
END