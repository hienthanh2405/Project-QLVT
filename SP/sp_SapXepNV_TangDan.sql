create PROC [dbo].[sp_SapXepNV_TangDan]
as
begin
	select * from NHANVIEN order by TEN ASC, HO ASC
end