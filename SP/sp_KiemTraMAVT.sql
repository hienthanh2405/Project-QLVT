CREATE PROC [dbo].[sp_KiemTraMAVT]
@MAVT nchar(4)
AS
BEGIN
	if EXISTS(select 1 from dbo.VATTU where dbo.VATTU.MAVT = @MAVT)
	begin
		return 1
	end
		return 0
END