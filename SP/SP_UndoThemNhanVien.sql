CREATE PROC [dbo].[SP_UndoThemNhanVien]
@MANV int
AS
BEGIN
	delete from NHANVIEN where NHANVIEN.MANV = @MANV
END