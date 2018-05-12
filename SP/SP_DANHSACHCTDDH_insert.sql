CREATE PROC [DBO].[SP_DANHSACHCTDDH_insert]
@MasoDDH nchar(8),
@MAVT nchar(4),
@SOLUONG int,
@DONGIA float
AS
BEGIN
	if (exists( select 1 from DATHANG where DATHANG.MasoDDH = @MasoDDH))
		insert into CTDDH(MasoDDH,MAVT,SOLUONG,DONGIA) values (@MasoDDH,@MAVT,@SOLUONG,@DONGIA)
	ELSE IF (exists( select 1 from LINK1.QLVT.dbo.DATHANG where MasoDDH = @MasoDDH))
				insert into LINK1.QLVT.dbo.CTDDH(MasoDDH,MAVT,SOLUONG,DONGIA) values (@MasoDDH,@MAVT,@SOLUONG,@DONGIA)
END