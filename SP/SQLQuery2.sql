CREATE PROC sp_KiemTraNhanVienTonTai
@MANV NUMERIC(18, 0)
AS
BEGIN
	--ki?m tra trong table nhanvien c?a server hi?n t?i
	IF EXISTS(SELECT 1 FROM DBO.NHANVIEN WHERE DBO.NHANVIEN.MANV = @MANV)
	BEGIN
		RETURN 1; --m? nhân viên t?n t?i ? server hi?n t?i
	END
	--ki?m tra trong table nhanvien c?a server c?n l?i
	IF EXISTS(SELECT 1 FROM LINK1.QL_VATTU.DBO.NHANVIEN NV WHERE NV.MANV = @MANV)
	BEGIN
		RETURN 1; --m? nhân viên t?n t?i ? server hi?n t?i
	END
	RETURN 0;
END