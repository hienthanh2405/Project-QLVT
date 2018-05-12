CREATE PROCEDURE [dbo].[SP_UndoXoaDatHang]
@MasoDDH nchar(8),
	@NGAY date,
	@NhaCC nvarchar(100),
	@MANV int,
	@MAKHO nchar(6)
AS
BEGIN
	insert into DATHANG(MasoDDH,NGAY,NhaCC,MANV, MAKHO) values (@MasoDDH,@NGAY,@NhaCC,@MANV, @MAKHO);
END