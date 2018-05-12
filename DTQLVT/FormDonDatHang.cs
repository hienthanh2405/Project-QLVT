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
    public partial class FormDonDatHang : Form
    {
        const int THEM = 0;
        const int SUA = 1;
        const int XOA = 2;
        int luaChon;

        public Stack st = new Stack();

        public FormDonDatHang()
        {
            InitializeComponent();
        }

        private void dATHANGBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsDDH.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void FormDonDatHang_Load(object sender, EventArgs e)
        {
            this.dS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.DATHANG' table. You can move, or remove it, as needed.
            this.dATHANGTableAdapter.Fill(this.dS.DATHANG);
            this.dATHANGTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nHANVIENTableAdapter.Fill(this.dS.NHANVIEN);
            this.nHANVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.kHOTableAdapter.Fill(this.dS.KHO);
            this.kHOTableAdapter.Connection.ConnectionString = Program.connstr;
            this.pHIEUNHAPTableAdapter.Fill(this.dS.PHIEUNHAP);
            this.pHIEUNHAPTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Fill(this.dS.CTDDH);
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;

            if (Program.mGroup == "CONGTY") // bật tắt phân quyền 
            {
                btnThoat.Enabled  = true;
                btnThem.Enabled = btnPhucHoi.Enabled = btnSua.Enabled = btnXoa.Enabled = btnGhi.Enabled = false;
                groupBox1.Enabled = btnCTDDH.Enabled = false;
            }
            groupBox1.Enabled = btnGhi.Enabled = btnUndo.Enabled = false;
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnCTDDH_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Program.maDH = txtMaDDH.Text;
            FormCTDDH fm = new FormCTDDH();
            fm.ShowDialog();
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            bdsDDH.AddNew();
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = gcDatHang.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = btnThoat.Enabled = true;
            luaChon = THEM;
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            btnThem.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnSua.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = btnThoat.Enabled = true;
            luaChon = SUA;

            DDH ddh = new DDH(txtMaDDH.Text, dtNgay.Text, txtNhaCC.Text, cmbMANV.Text, cmbMAKHO.Text); //truyền các giá trị vô KHO
            ObjectUndo ob = new ObjectUndo(luaChon, ddh);
            st.Push(ob);
            updateButtonPhucHoi();
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (luaChon)
            {
                case THEM:
                    if (txtMaDDH.Text.Trim() == "")
                    {
                        MessageBox.Show("Mã ĐƠN ĐẶT HÀNG không được thiếu!", "", MessageBoxButtons.OK);
                        txtMaDDH.Focus();
                        return;
                    }
                    if (dtNgay.Text.Trim() == "")
                    {
                        MessageBox.Show("Ngày không được thiếu!", "", MessageBoxButtons.OK);
                        dtNgay.Focus();
                        return;
                    }
                    if (txtNhaCC.Text.Trim() == "")
                    {
                        MessageBox.Show("Nhà Cung Cấp không được thiếu!", "", MessageBoxButtons.OK);
                        txtNhaCC.Focus();
                        return;
                    }
                    if (cmbMANV.Text.Trim() == "")
                    {
                        MessageBox.Show("NHÂN VIÊN không được thiếu!", "", MessageBoxButtons.OK);
                        cmbMANV.Focus();
                        return;
                    }
                    if (cmbMAKHO.Text.Trim() == "")
                    {
                        MessageBox.Show("MÃ KHO không được thiếu!", "", MessageBoxButtons.OK);
                        cmbMAKHO.Focus();
                        return;
                    }
                    if (Program.conn.State == ConnectionState.Closed)
                        Program.conn.Open();
                    String strLenh = "dbo.SP_KiemTraMaDatHang";
                    Program.sqlcmd = Program.conn.CreateCommand();
                    Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                    Program.sqlcmd.CommandText = strLenh;
                    Program.sqlcmd.Parameters.Add("@MasoDDH", SqlDbType.NChar).Value = txtMaDDH.Text;
                    Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
                    Program.sqlcmd.ExecuteNonQuery();
                    Program.conn.Close();
                    String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                    if (Ret != "0")
                    {
                        MessageBox.Show("Mã ĐƠN ĐẶT HÀNG bị trùng!", "", MessageBoxButtons.OK);
                        txtMaDDH.Focus();
                        return;
                    }
                    try
                    {
                        bdsDDH.EndEdit();
                        bdsDDH.ResetCurrentItem();
                        this.dATHANGTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.dATHANGTableAdapter.Update(this.dS.DATHANG);
                       
                        string lenh = "exec SP_UndoThemDDH'" + txtMaDDH.Text + "'";
                        ObjectUndo ob = new ObjectUndo(luaChon, lenh);
                        st.Push(ob);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ghi kho.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcDatHang.Enabled = true;
                    btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnThoat.Enabled = true;
                    btnGhi.Enabled = false; groupBox1.Enabled = false;
                    break;

                case SUA:
                     try
                    {
                        bdsDDH.EndEdit();
                        bdsDDH.ResetCurrentItem();
                        this.dATHANGTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.dATHANGTableAdapter.Update(this.dS.DATHANG);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi sửa ĐẶT HÀNG.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcDatHang.Enabled = true;
                    btnThem.Enabled = btnThoat.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnSua.Enabled = true;
                    btnGhi.Enabled = groupBox1.Enabled = false;
                    break;

            }


        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            String strLenh = "dbo.SP_Ktra_DDH_co_CTDDH";
            Program.sqlcmd = Program.conn.CreateCommand();
            Program.sqlcmd.CommandType = CommandType.StoredProcedure;
            Program.sqlcmd.CommandText = strLenh;
            Program.sqlcmd.Parameters.Add("@MasoDDH", SqlDbType.NChar).Value = txtMaDDH.Text;
            Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
            Program.sqlcmd.ExecuteNonQuery();
            Program.conn.Close();
            String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
            if (Ret != "0")
            {
                MessageBox.Show("Mã ĐƠN ĐẶT HÀNG đã có CTDDH!", "", MessageBoxButtons.OK);
                txtMaDDH.Focus();
                return;
            }
           
            if (MessageBox.Show("Bạn có thật sự muốn xóa DDH này ?? ", "Xác nhận",
                       MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    luaChon = XOA;
                    DDH ddh = new DDH(txtMaDDH.Text, dtNgay.Text, txtNhaCC.Text, cmbMANV.Text, cmbMAKHO.Text); //truyền các giá trị vô KHO
                    ObjectUndo ob = new ObjectUndo(luaChon, ddh);
                    st.Push(ob);

                    bdsDDH.RemoveCurrent();
                    this.dATHANGTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.dATHANGTableAdapter.Update(this.dS.DATHANG);
                    this.nHANVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nHANVIENTableAdapter.Update(this.dS.NHANVIEN);
                    this.kHOTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.kHOTableAdapter.Update(this.dS.KHO);
                    updateButtonPhucHoi();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa Đơn Đặt Hàng. Bạn hãy xóa lại\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.dATHANGTableAdapter.Fill(this.dS.DATHANG);
                    //bdsKho.Position = bdsKho.Find("MAKHO", makho);
                    return;
                }
            }
        }

        private void reload()
        {
            try
            {
                this.dATHANGTableAdapter.Fill(this.dS.DATHANG);

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

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btnSua.Enabled == false || btnThem.Enabled == false)
            {
                this.bdsDDH.CancelEdit();
                gcDatHang.Enabled = true;
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
                        this.dATHANGTableAdapter.Fill(this.dS.DATHANG);
                        break;

                    case SUA:
                        MessageBox.Show("Khôi phục sau khi SỬA ");
                        DDH ddh = (DDH)ob.obj;
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh = "dbo.SP_UndoSuaDDH";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh;
                        Program.sqlcmd.Parameters.Add("@MasoDDH", SqlDbType.NChar).Value = ddh.MasoDDH;
                        Program.sqlcmd.Parameters.Add("@NGAY", SqlDbType.NVarChar).Value = ddh.Ngay;
                        Program.sqlcmd.Parameters.Add("@NhaCC", SqlDbType.NVarChar).Value = ddh.NhaCC;
                        Program.sqlcmd.Parameters.Add("@MANV", SqlDbType.NVarChar).Value = ddh.MaNV;
                        Program.sqlcmd.Parameters.Add("@MAKHO", SqlDbType.NVarChar).Value = ddh.MaKho;

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
                        DDH ddh1 = (DDH)ob.obj;
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh1 = "dbo.SP_UndoXoaDDH";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh1;
                        Program.sqlcmd.Parameters.Add("@MasoDDH", SqlDbType.NChar).Value = ddh1.MasoDDH;
                        Program.sqlcmd.Parameters.Add("@NGAY", SqlDbType.NVarChar).Value = ddh1.Ngay;
                        Program.sqlcmd.Parameters.Add("@NhaCC", SqlDbType.NVarChar).Value = ddh1.NhaCC;
                        Program.sqlcmd.Parameters.Add("@MANV", SqlDbType.NVarChar).Value = ddh1.MaNV;
                        Program.sqlcmd.Parameters.Add("@MAKHO", SqlDbType.NVarChar).Value = ddh1.MaKho;

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
                bdsDDH.EndEdit();
                bdsDDH.ResetCurrentItem();
                this.dATHANGTableAdapter.Connection.ConnectionString = Program.connstr;
                this.dATHANGTableAdapter.Update(this.dS.DATHANG);

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
                btnUndo.Enabled = false;
            else
                btnUndo.Enabled = true;
        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.sP_DanhSachCTDDHTableAdapter.Fill(this.dS.SP_DanhSachCTDDH, masoDDHToolStripTextBox.Text);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

    }
    public class DDH
    {
        public String MasoDDH;
        public String Ngay;
        public String NhaCC;
        public String MaNV;
        public String MaKho;
        public DDH(String MasoDDH, String Ngay, String NhaCC, String MaNV, String MaKho)
        {
            this.MasoDDH = MasoDDH;
            this.Ngay = Ngay;
            this.NhaCC = NhaCC;
            this.MaNV = MaNV;
            this.MaKho = MaKho;
        }
    }
}
