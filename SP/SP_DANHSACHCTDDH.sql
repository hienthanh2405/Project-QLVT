CREATE PROC [dbo].[SP_DANHSACHCTDDH]
@MasoDDH nchar(8)
AS
BEGIN
	select * from dbo.CTDDH where CTDDH.MasoDDH = @MasoDDH
END