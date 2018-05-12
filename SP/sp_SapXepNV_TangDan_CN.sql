create PROC [dbo].[sp_SapXepNV_TangDan_CN]
as
begin
	select * from LINK1.QLVT.dbo.NHANVIEN order by TEN asc, HO asc
end