using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace DTQLVT
{
    public partial class FormPhieuNhap : Form
    {

        const int THEM = 0;
        const int SUA = 1;
        const int XOA = 2;
        int luaChon;

        public Stack st = new Stack();

        public FormPhieuNhap()
        {
            InitializeComponent();
        }

        private void pHIEUNHAPBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsPhieuNhap.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void FormPhieuNhap_Load(object sender, EventArgs e)
        {

            this.dS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.PHIEUNHAP' table. You can move, or remove it, as needed.
            this.pHIEUNHAPTableAdapter.Connection.ConnectionString = Program.connstr;
            this.pHIEUNHAPTableAdapter.Fill(this.dS.PHIEUNHAP);
            this.dATHANGTableAdapter.Connection.ConnectionString = Program.connstr;
            this.dATHANGTableAdapter.Fill(this.dS.DATHANG);
            this.kHOTableAdapter.Connection.ConnectionString = Program.connstr;
            this.kHOTableAdapter.Fill(this.dS.KHO);
            this.nHANVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nHANVIENTableAdapter.Fill(this.dS.NHANVIEN);
            this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPNTableAdapter.Fill(this.dS.CTPN);
            this.sP_KTRADDHCOCTDDHTableAdapter.Fill(this.qLVTDataSet.SP_KTRADDHCOCTDDH);

            if (Program.mGroup == "CONGTY") // bật tắt phân quyền 
            {
                btnThoat.Enabled =  true;
                btnCTPN.Enabled = btnThem.Enabled = btnPhucHoi.Enabled = btnSua.Enabled = btnXoa.Enabled = btnGhi.Enabled = false;
                groupBox1.Enabled = false;
            }
            //btnThoat.Enabled = btnThem.Enabled = btnPhucHoi.Enabled = btnSua.Enabled = btnXoa.Enabled = true;
            btnGhi.Enabled = groupBox1.Enabled = btnPhucHoi.Enabled =false;
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            bdsPhieuNhap.AddNew();
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = gcPhieuNhap.Enabled = false;
            btnGhi.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled = true;
            luaChon = THEM;
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            txtMAPN.Enabled = btnThem.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnSua.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = btnThoat.Enabled = true;
            luaChon = SUA;

            PhieuNhap phieunhap = new PhieuNhap(txtMAPN.Text, dtNgay.Text, cmbMDDH.Text, cmbMANV.Text,cmbMAKHO.Text); //truyền các giá trị vô KHO
            ObjectUndo ob = new ObjectUndo(luaChon, phieunhap);
            st.Push(ob);
            updateButtonPhucHoi();
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (luaChon)
            {
                case THEM:
                    if (txtMAPN.Text.Trim() == "")
                    {
                        MessageBox.Show("Mã PHIẾU NHẬP không được thiếu!", "", MessageBoxButtons.OK);
                        txtMAPN.Focus();
                        return;
                    }
                    if (dtNgay.Text.Trim() == "")
                    {
                        MessageBox.Show("NGÀY không được thiếu!", "", MessageBoxButtons.OK);
                        dtNgay.Focus();
                        return;
                    }
                    if (cmbMDDH.Text.Trim() == "")
                    {
                        MessageBox.Show("Mã ĐƠN ĐẶT HÀNG không được thiếu!", "", MessageBoxButtons.OK);
                        cmbMDDH.Focus();
                        return;
                    }
                    if (cmbMANV.Text.Trim() == "")
                    {
                        MessageBox.Show("MÃ NHÂN VIÊN không được thiếu!", "", MessageBoxButtons.OK);
                        cmbMANV.Focus();
                        return;
                    }
                    if (cmbMAKHO.Text.Trim() == "")
                    {
                        MessageBox.Show("MÃ KHO không được thiếu!", "", MessageBoxButtons.OK);
                        cmbMAKHO.Focus();
                        return;
                    }
                    try
                    {
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh = "dbo.SP_KTMAPHIEUNHAP";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh;
                        Program.sqlcmd.Parameters.Add("@MAPN", SqlDbType.NChar).Value = txtMAPN.Text;
                        Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        Program.sqlcmd.ExecuteNonQuery();
                        Program.conn.Close();
                        String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                        if (Ret != "0")
                        {
                            MessageBox.Show("Mã PHIẾU NHẬP bị trùng!", "", MessageBoxButtons.OK);
                            txtMAPN.Focus();
                            return;
                        }

                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String ktrngay = "dbo.SP_PhieuNhap_KiemTraNgay";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = ktrngay;
                        Program.sqlcmd.Parameters.Add("@MasoDDH", SqlDbType.NChar).Value = cmbMDDH.Text;
                        Program.sqlcmd.Parameters.Add("@NgayNhap", SqlDbType.Date).Value = dtNgay.Text;
                        Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        Program.sqlcmd.ExecuteNonQuery();
                        Program.conn.Close();
                        String Ret1 = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                        if (Ret1 == "0")
                        {
                            MessageBox.Show("Ngày Nhập phải lớn hơn ngày Đặt Hàng !", "", MessageBoxButtons.OK);
                            dtNgay.Focus();
                            return;
                        }

                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String ktMADDH = "dbo.SP_KTRA_DDHCOPPHIEUNHAP";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = ktMADDH;
                        Program.sqlcmd.Parameters.Add("@MasoDDH", SqlDbType.NChar).Value = cmbMDDH.Text;
                        Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        Program.sqlcmd.ExecuteNonQuery();
                        Program.conn.Close();
                        String Ret3 = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                        if (Ret3 != "0")
                        {
                            MessageBox.Show("Mã DDH đã được nhập!", "", MessageBoxButtons.OK);
                            cmbMDDH.Focus();
                            return;
                        }

                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String ktkho = "dbo.SP_PhieuNhap_KiemTraMAKHO";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = ktkho;
                        Program.sqlcmd.Parameters.Add("@MasoDDH", SqlDbType.NChar).Value = cmbMDDH.Text;
                        Program.sqlcmd.Parameters.Add("@MAKHO", SqlDbType.NChar).Value = cmbMAKHO.Text;
                        Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        Program.sqlcmd.ExecuteNonQuery();
                        Program.conn.Close();
                        String Ret2 = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                        if (Ret2 == "0")
                        {
                            MessageBox.Show("MÃ KHO NHẬP phải đúng với MÃ KHO ĐẶT!", "", MessageBoxButtons.OK);
                            dtNgay.Focus();
                            return;
                        }
                        bdsPhieuNhap.EndEdit();
                        bdsPhieuNhap.ResetCurrentItem();
                        this.pHIEUNHAPTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.pHIEUNHAPTableAdapter.Update(this.dS.PHIEUNHAP);
                        string lenh = "exec SP_UndoThemPhieuNhap'" + txtMAPN.Text + "'";
                        ObjectUndo ob = new ObjectUndo(luaChon, lenh);
                        st.Push(ob);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ghi PHIẾU NHẬP.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcPhieuNhap.Enabled = true;
                    btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnThoat.Enabled = true;
                    btnGhi.Enabled = false; groupBox1.Enabled = false;
                    break;

                case SUA:
                    try
                    {
                        bdsPhieuNhap.EndEdit();
                        bdsPhieuNhap.ResetCurrentItem();
                        this.pHIEUNHAPTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.pHIEUNHAPTableAdapter.Update(this.dS.PHIEUNHAP);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi sửa PHIẾU NHẬP.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcPhieuNhap.Enabled = true;
                    btnThem.Enabled = btnThoat.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnSua.Enabled = true;
                    btnGhi.Enabled = groupBox1.Enabled = false;
                    break;
            }

        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            String strLenh = "dbo.SP_PhieuNhap_Co_CTPN";
            Program.sqlcmd = Program.conn.CreateCommand();
            Program.sqlcmd.CommandType = CommandType.StoredProcedure;
            Program.sqlcmd.CommandText = strLenh;
            Program.sqlcmd.Parameters.Add("@MAPN", SqlDbType.NChar).Value = txtMAPN.Text;
            Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            Program.sqlcmd.ExecuteNonQuery();
            Program.conn.Close();
            String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
            if (Ret != "0")
            {
                MessageBox.Show("Phiếu Nhập Đã Có CTPN!", "", MessageBoxButtons.OK);
                txtMAPN.Focus();
                return;
            }
            if (MessageBox.Show("Bạn có thật sự muốn xóa PHIẾU NHẬP này ?? ", "Xác nhận",
                      MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    luaChon = XOA;
                    PhieuNhap phieunhap = new PhieuNhap(txtMAPN.Text, dtNgay.Text, cmbMDDH.Text, cmbMANV.Text, cmbMAKHO.Text); ; //truyền các giá trị vô KHO
                    ObjectUndo ob = new ObjectUndo(luaChon, phieunhap);
                    st.Push(ob);

                    bdsPhieuNhap.RemoveCurrent();
                    this.pHIEUNHAPTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.pHIEUNHAPTableAdapter.Update(this.dS.PHIEUNHAP);

                    updateButtonPhucHoi();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa PHIẾU NHẬP. Bạn hãy xóa lại\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.pHIEUNHAPTableAdapter.Fill(this.dS.PHIEUNHAP);
                    //bdsKho.Position = bdsKho.Find("MAKHO", makho);
                    return;
                }
            }
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btnSua.Enabled == false || btnThem.Enabled == false)
            {
                this.bdsPhieuNhap.CancelEdit();
                //if (btnThem.Enabled == false) bdsKho.Position = vitri;
                gcPhieuNhap.Enabled = true;
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
                        this.pHIEUNHAPTableAdapter.Fill(this.dS.PHIEUNHAP);
                        break;

                    case SUA:
                        MessageBox.Show("Khôi phục sau khi SỬA ");
                        PhieuNhap phieunhap = (PhieuNhap)ob.obj;
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh = "dbo.SP_UndoSuaPhieuNhap";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh;
                        Program.sqlcmd.Parameters.Add("@MAPN", SqlDbType.NChar).Value = phieunhap.maPhieuNhap;
                        Program.sqlcmd.Parameters.Add("@NGAY", SqlDbType.Date).Value = phieunhap.ngay;
                        Program.sqlcmd.Parameters.Add("@MasoDDH", SqlDbType.NChar).Value = phieunhap.MasoDDH;
                        Program.sqlcmd.Parameters.Add("@MANV", SqlDbType.Int).Value = phieunhap.MaNV;
                        Program.sqlcmd.Parameters.Add("@MAKHO", SqlDbType.NChar).Value = phieunhap.MaKho;


                        Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
                        Program.sqlcmd.ExecuteNonQuery();
                        Program.conn.Close();
                        String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                        if (Ret != "0")
                        {
                            MessageBox.Show("Khoi phuc khong thanh cong", "", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("Khoi phuc thanh cong", "", MessageBoxButtons.OK);
                        }
                        break;

                    case XOA:
                        MessageBox.Show("Khôi phục sau khi XÓA ");
                        PhieuNhap phieunhap1 = (PhieuNhap)ob.obj;
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh1 = "dbo.SP_UndoXoaPhieuNhap";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh1;
                        Program.sqlcmd.Parameters.Add("@MAPN", SqlDbType.NChar).Value = phieunhap1.maPhieuNhap;
                        Program.sqlcmd.Parameters.Add("@NGAY", SqlDbType.Date).Value = phieunhap1.ngay;
                        Program.sqlcmd.Parameters.Add("@MasoDDH", SqlDbType.NChar).Value = phieunhap1.MasoDDH;
                        Program.sqlcmd.Parameters.Add("@MANV", SqlDbType.Int).Value = phieunhap1.MaNV;
                        Program.sqlcmd.Parameters.Add("@MAKHO", SqlDbType.NChar).Value = phieunhap1.MaKho;

                        Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
                        Program.sqlcmd.ExecuteNonQuery();
                        Program.conn.Close();
                        String Ret1 = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                        if (Ret1 != "0")
                        {
                            MessageBox.Show("Khoi phuc khong thanh cong", "", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("Khoi phuc thanh cong", "", MessageBoxButtons.OK);
                        }
                        break;
                }
                bdsPhieuNhap.EndEdit();
                bdsPhieuNhap.ResetCurrentItem();
                this.pHIEUNHAPTableAdapter.Connection.ConnectionString = Program.connstr;
                this.pHIEUNHAPTableAdapter.Update(this.dS.PHIEUNHAP);

                updateButtonPhucHoi();
                reload();


            }
            catch (Exception)
            {
                MessageBox.Show("Không có gì để UNDO", "THÔNG BÁO", MessageBoxButtons.OK);
            }
        }

        private void reload()
        {
            try
            {
                this.pHIEUNHAPTableAdapter.Fill(this.dS.PHIEUNHAP);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload :" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }

        }

        private void btnReLoad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reload();
        }

        private void btnCTPN_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Program.maPN = txtMAPN.Text;
            FormCTPN fm = new FormCTPN();
            fm.ShowDialog();
        }

        private void updateButtonPhucHoi()
        {
            if (st.Count == 0)
                btnPhucHoi.Enabled = false;
            else
                btnPhucHoi.Enabled = true;
        }

    }
    public class PhieuNhap
    {
        public String maPhieuNhap;
        public String ngay;
        public String MasoDDH;
        public String MaNV;
        public String MaKho;

        public PhieuNhap(String maPhieuNhap, String ngay, String MasoDDH, String MaNV,String MaKho)
        {
            this.maPhieuNhap = maPhieuNhap;
            this.ngay = ngay;
            this.MasoDDH = MasoDDH;
            this.MaNV = MaNV;
            this.MaKho = MaKho;
        }
    }
}
