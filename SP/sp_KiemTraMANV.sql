CREATE PROC [dbo].[sp_KiemTraMANV]
@MANV int 
AS
BEGIN
	if EXISTS(select 1 from dbo.NHANVIEN where dbo.NHANVIEN.MANV = @MANV)
	begin
		return 1
	end
	if EXISTS(select 2 from LINK1.QLVT.dbo.NHANVIEN as P where p.MANV = @MANV)
	begin
		return 2
	end
	return 0
END