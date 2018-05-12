using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;

namespace DTQLVT
{
    public partial class formNhanVien : Form
    {
        int vitri = 0;
        string macn = "";
        const int THEM = 0;
        const int SUA = 1;
        const int XOA = 2;
        int luaChon;

        public Stack st = new Stack();

        public formNhanVien()
        {
            InitializeComponent();
        }


        private void formNhanVien_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.DATHANG' table. You can move, or remove it, as needed.
            this.v_DS_PHANMANGTableAdapter.Fill(this.qLVTDataSet.V_DS_PHANMANG);
            this.nHANVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nHANVIENTableAdapter.Fill(this.dS.NHANVIEN);
            this.dATHANGTableAdapter.Fill(this.dS.DATHANG);
            this.dATHANGTableAdapter.Connection.ConnectionString = Program.connstr;
            this.pHIEUXUATTableAdapter.Fill(this.dS.PHIEUXUAT);
            this.pHIEUXUATTableAdapter.Connection.ConnectionString = Program.connstr;
            this.pHIEUNHAPTableAdapter.Fill(this.dS.PHIEUNHAP);
            this.pHIEUNHAPTableAdapter.Connection.ConnectionString = Program.connstr;
            macn = ((DataRowView)bdsDSNV[0])["MACN"].ToString();
            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập  qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;
            if (Program.mGroup == "CONGTY") // bật tắt phân quyền 
            {
                cmbChiNhanh.Enabled = true;
                btnThoat.Enabled = btnInDSNV.Enabled = true;
                btnThem.Enabled = btnPhucHoi.Enabled = btnSua.Enabled = btnXoa.Enabled = btnGhi.Enabled = false;
                groupBox1.Enabled = false;
            }
            else if (Program.mGroup == "CHINHANH")
            {
                cmbChiNhanh.Enabled = groupBox1.Enabled = false;
            }
            else
            {
                cmbChiNhanh.Enabled = groupBox1.Enabled = btnInDSNV.Enabled = false;
            }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsDSNV.Position;
            groupBox1.Enabled = true;
            bdsDSNV.AddNew();
            txtMACN.Text = macn;
            txtMACN.Enabled = false;

            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            gcNhanVien.Enabled = false;
            luaChon = THEM;
        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbChiNhanh.SelectedValue != null)
            {
                if (cmbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView") return;
                Program.servername = cmbChiNhanh.SelectedValue.ToString();

                if (cmbChiNhanh.SelectedIndex != Program.mChinhanh)
                {
                    Program.mlogin = Program.remotelogin;
                    Program.password = Program.remotepassword;
                }
                else
                {
                    Program.mlogin = Program.mloginDN;
                    Program.password = Program.passwordDN;
                }
                if (Program.KetNoi() == 0)
                {
                    MessageBox.Show("Lỗi kết nối về chi nhánh mới", "", MessageBoxButtons.OK);
                }
                else
                {
                    this.nHANVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nHANVIENTableAdapter.Fill(this.dS.NHANVIEN);
                }
            }
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (luaChon)
            {
                case THEM:
                    if (txtMANV.Text.Trim() == "")
                    {
                        MessageBox.Show("Mã nhân viên không được thiếu!", "", MessageBoxButtons.OK);
                        txtMANV.Focus();
                        return;
                    }
                    if (txtHo.Text.Trim() == "")
                    {
                        MessageBox.Show("Họ nhân viên không được thiếu!", "", MessageBoxButtons.OK);
                        txtHo.Focus();
                        return;
                    }
                    if (txtTen.Text.Trim() == "")
                    {
                        MessageBox.Show("Tên nhân viên không được thiếu!", "", MessageBoxButtons.OK);
                        txtTen.Focus();
                        return;
                    }
                    if (dtNgaySinh.Text == "")
                    {
                        MessageBox.Show("Ngày Sinh Không được rỗng!!", "", MessageBoxButtons.OK);
                        dtNgaySinh.Focus();
                        return;
                    }
                    if (txtDiaChi.Text == "")
                    {
                        MessageBox.Show("Địa Chỉ Không được rỗng!!", "", MessageBoxButtons.OK);
                        txtDiaChi.Focus();
                        return;
                    }

                    // LUONg thỏa Miền giá trị
                    long n = 0;
                    long.TryParse(txtLuong.Text, out n);

                    if (n < 4000000)
                    {
                        MessageBox.Show("Lương nhân viên >= 4000000!", "", MessageBoxButtons.OK);
                        txtLuong.Focus();
                        return;
                    }

                    if (Program.conn.State == ConnectionState.Closed)
                        Program.conn.Open();
                    String strLenh = "dbo.SP_KiemTraMANV";
                    Program.sqlcmd = Program.conn.CreateCommand();
                    Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                    Program.sqlcmd.CommandText = strLenh;
                    Program.sqlcmd.Parameters.Add("@MANV", SqlDbType.Int).Value = txtMANV.Text;
                    Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                    Program.sqlcmd.ExecuteNonQuery();
                    Program.conn.Close();
                    String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                    if (Ret != "0")
                    {
                        MessageBox.Show("Mã nhân viên bị trùng!", "", MessageBoxButtons.OK);
                        txtMANV.Focus();
                        return;
                    }
                    try
                    {
                        bdsDSNV.EndEdit();
                        bdsDSNV.ResetCurrentItem();
                        this.nHANVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.nHANVIENTableAdapter.Update(this.dS.NHANVIEN);
                        string lenh = "exec SP_UndoThemNhanVien'" + txtMANV.Text + "'";
                        ObjectUndo ob = new ObjectUndo(luaChon, lenh);
                        st.Push(ob);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ghi nhân viên.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcNhanVien.Enabled = true;
                    btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnThoat.Enabled = true;
                    btnGhi.Enabled = btnInDSNV.Enabled = false;
                    groupBox1.Enabled = false;
                    break;

                case SUA:
                    try
                    {
                        bdsDSNV.EndEdit();
                        bdsDSNV.ResetCurrentItem();
                        this.nHANVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.nHANVIENTableAdapter.Update(this.dS.NHANVIEN);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ghi kho.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcNhanVien.Enabled = true;
                    btnThem.Enabled = btnThoat.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnSua.Enabled = true;
                    btnGhi.Enabled = groupBox1.Enabled = false;
                    break;
            }

        }

        private void btnReLoad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reload();
        }

        private void reload()
        {
            try
            {
                this.nHANVIENTableAdapter.Fill(this.dS.NHANVIEN);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload :" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Int32 manv = 0;
            if (bdsPhieuNhap.Count > 0)
            {
                MessageBox.Show("Nhân viên có PHIẾU NHẬP không thể xóa được", "", MessageBoxButtons.OK);
                return;
            }
            if (bdsPhieuXuat.Count > 0)
            {
                MessageBox.Show("Nhân viên có PHIẾU XUẤT không thể xóa được", "", MessageBoxButtons.OK);
                return;
            }
            if (bdsDatHang.Count > 0)
            {
                MessageBox.Show("Nhân viên có PHIẾU ĐẶT HÀNG không thể xóa được", "", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn có thật sự muốn xóa nhân viên này ?? ", "Xác nhận",
                       MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    luaChon = XOA;
                    NhanVien nhanvien = new NhanVien(txtMANV.Text, txtHo.Text, txtTen.Text, txtDiaChi.Text, dtNgaySinh.Text, txtLuong.Text, txtMACN.Text); //truyền các giá trị vô KHO
                    ObjectUndo ob = new ObjectUndo(luaChon, nhanvien);
                    st.Push(ob);

                    manv = int.Parse(((DataRowView)bdsDSNV[bdsDSNV.Position])["MANV"].ToString()); // giữ lại để khi xóa bij lỗi thì ta sẽ quay về lại
                    bdsDSNV.RemoveCurrent();
                    this.nHANVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nHANVIENTableAdapter.Update(this.dS.NHANVIEN);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa nhân viên. Bạn hãy xóa lại\n" + ex.Message, "",
                        MessageBoxButtons.OK);
                    this.nHANVIENTableAdapter.Fill(this.dS.NHANVIEN);
                    bdsDSNV.Position = bdsDSNV.Find("MANV", manv);
                    return;
                }
            }

            if (bdsDSNV.Count == 0) btnXoa.Enabled = false;
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            txtMANV.Enabled = txtMACN.Enabled = false;
            groupBox1.Enabled = btnPhucHoi.Enabled = btnGhi.Enabled = true;
            btnThem.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnThoat.Enabled = btnInDSNV.Enabled = false;
            luaChon = SUA;

            NhanVien nhanvien = new NhanVien(txtMANV.Text, txtHo.Text, txtTen.Text, txtDiaChi.Text, dtNgaySinh.Text, txtLuong.Text, txtMACN.Text); //truyền các giá trị vô KHO
            ObjectUndo ob = new ObjectUndo(luaChon, nhanvien);
            st.Push(ob);
            updateButtonPhucHoi();
        }

        //ràng buộc đánh số 
        private void txtLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        //ràng buộc đánh số
        private void txtMANV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btnSua.Enabled == false || btnThem.Enabled == false)
            {
                this.bdsDSNV.CancelEdit();
                //if (btnThem.Enabled == false) bdsKho.Position = vitri;
                gcNhanVien.Enabled = true;
                groupBox1.Enabled = false;
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnThoat.Enabled = true;
                btnGhi.Enabled = btnSua.Enabled = false;
            }
            try
            {
                ObjectUndo ob = (ObjectUndo)st.Pop();

                switch (ob.luaChon)
                {
                    case THEM:
                        String lenh = (String)ob.obj;
                        MessageBox.Show("Khôi phục sau khi THÊM ");
                        Program.ExecSqlDataReader(lenh);
                        this.nHANVIENTableAdapter.Fill(this.dS.NHANVIEN);
                        break;

                    case SUA:
                        MessageBox.Show("Khôi phục sau khi SỬA ");
                        NhanVien nhanvien = (NhanVien)ob.obj;
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh = "dbo.SP_UndoSuaNhanVien";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh;
                        Program.sqlcmd.Parameters.Add("@MANV", SqlDbType.Int).Value = nhanvien.maNV;
                        Program.sqlcmd.Parameters.Add("@HO", SqlDbType.NVarChar).Value = nhanvien.ho;
                        Program.sqlcmd.Parameters.Add("@TEN", SqlDbType.NVarChar).Value = nhanvien.ten;
                        Program.sqlcmd.Parameters.Add("@DIACHI", SqlDbType.NVarChar).Value = nhanvien.diaChi;
                        Program.sqlcmd.Parameters.Add("@NGAYSINH", SqlDbType.DateTime).Value = nhanvien.ngaySinh;
                        Program.sqlcmd.Parameters.Add("@LUONG", SqlDbType.Float).Value = nhanvien.luong;
                        Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
                        Program.sqlcmd.ExecuteNonQuery();
                        Program.conn.Close();
                        String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                        if (Ret != "0")
                        {
                            MessageBox.Show("Khôi phục không thành công", "", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("Khôi phục thành công", "", MessageBoxButtons.OK);
                        }
                        break;

                    case XOA:
                        MessageBox.Show("Khôi phục sau khi XÓA ");
                        NhanVien nhanvien1 = (NhanVien)ob.obj;
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh1 = "dbo.SP_UndoXoaNhanVien";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh1;
                        Program.sqlcmd.Parameters.Add("@MANV", SqlDbType.Int).Value = nhanvien1.maNV;
                        Program.sqlcmd.Parameters.Add("@HO", SqlDbType.NVarChar).Value = nhanvien1.ho;
                        Program.sqlcmd.Parameters.Add("@TEN", SqlDbType.NVarChar).Value = nhanvien1.ten;
                        Program.sqlcmd.Parameters.Add("@DIACHI", SqlDbType.NVarChar).Value = nhanvien1.diaChi;
                        Program.sqlcmd.Parameters.Add("@NGAYSINH", SqlDbType.DateTime).Value = nhanvien1.ngaySinh;
                        Program.sqlcmd.Parameters.Add("@LUONG", SqlDbType.Float).Value = nhanvien1.luong;
                        Program.sqlcmd.Parameters.Add("@MACN", SqlDbType.NChar).Value = nhanvien1.maCN;
                        Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
                        Program.sqlcmd.ExecuteNonQuery();
                        Program.conn.Close();
                        String Ret1 = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                        if (Ret1 != "0")
                        {
                            MessageBox.Show("Khôi phục không thành công", "", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("Khôi phục thành công", "", MessageBoxButtons.OK);
                        }
                        break;
                }
                bdsDSNV.EndEdit();
                bdsDSNV.ResetCurrentItem();
                this.nHANVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                this.nHANVIENTableAdapter.Update(this.dS.NHANVIEN);

                updateButtonPhucHoi();
                reload();


            }
            catch (Exception)
            {
                MessageBox.Show("Không có gì để UNDO", "THÔNG BÁO", MessageBoxButtons.OK);
            }
        }

        private void updateButtonPhucHoi()
        {
            if (st.Count == 0)
                btnPhucHoi.Enabled = false;
            else
                btnPhucHoi.Enabled = true;
        }

        private void btnInDSNV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form_rpDSNV fm = new Form_rpDSNV();
            fm.ShowDialog();
        }
    }
    public class NhanVien
    {
        public String maNV;
        public String ho;
        public String ten;
        public String diaChi;
        public String ngaySinh;
        public String luong;
        public String maCN;

        public NhanVien(String maNV, String ho, String ten, String diaChi, String ngaySinh, String luong, String maCN)
        {
            this.maNV = maNV;
            this.ho = ho;
            this.ten = ten;
            this.diaChi = diaChi;
            this.ngaySinh = ngaySinh;
            this.luong = luong;
            this.maCN = maCN;
        }
    }
}
