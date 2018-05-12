CREATE PROC [dbo].[sp_KiemTraMAKHO]
@MAKHO nchar(4)
AS
BEGIN
	if EXISTS(select 1 from dbo.KHO where dbo.KHO.MAKHO = @MAKHO)
	begin
		return 1
	end
	if EXISTS(select 2 from LINK1.QLVT.dbo.KHO as k where k.MAKHO = @MAKHO)
	begin
		return 2
	end
	return 0
END