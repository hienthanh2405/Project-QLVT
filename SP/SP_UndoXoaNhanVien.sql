CREATE PROC [dbo].[SP_UndoXoaNhanVien]
@MANV int,
@HO nvarchar(40),
@TEN nvarchar(100),
@DIACHI nvarchar(100),
@NGAYSINH datetime,
@LUONG float,
@MACN nchar(10)
AS
BEGIN
	insert into NHANVIEN(MANV, HO, TEN, DIACHI, NGAYSINH, LUONG, MACN) values (@MANV, @HO, @TEN, @DIACHI, @NGAYSINH, @LUONG, @MACN)
END