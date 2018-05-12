CREATE PROC [DBO].[SP_DANHSACHCTDDH_delete]
@MasoDDH nchar(8),
@MAVT nchar(4)
AS
BEGIN
	Delete from CTDDH where CTDDH.MasoDDH = @MasoDDH and CTDDH.MAVT = @MAVT
END