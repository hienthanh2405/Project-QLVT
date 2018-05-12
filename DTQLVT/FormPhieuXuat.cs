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
    public partial class FormPhieuXuat : Form
    {
        const int THEM = 0;
        const int SUA = 1;
        const int XOA = 2;
        int luaChon;

        public Stack st = new Stack();

        public FormPhieuXuat()
        {
            InitializeComponent();
        }

        private void pHIEUXUATBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsPhieuXuat.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void FormPhieuXuat_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dS.CTPX' table. You can move, or remove it, as needed.
            
            // TODO: This line of code loads data into the 'dS.PHIEUNHAP' table. You can move, or remove it, as needed.
           
            dS.EnforceConstraints = false;
            this.cTPXTableAdapter.Fill(this.dS.CTPX);
            // TODO: This line of code loads data into the 'dS.KHO' table. You can move, or remove it, as needed.
            this.kHOTableAdapter.Connection.ConnectionString = Program.connstr;
            this.kHOTableAdapter.Fill(this.dS.KHO);
            this.nHANVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nHANVIENTableAdapter.Fill(this.dS.NHANVIEN);
            this.pHIEUXUATTableAdapter.Connection.ConnectionString = Program.connstr;
            this.pHIEUXUATTableAdapter.Fill(this.dS.PHIEUXUAT);
            this.pHIEUNHAPTableAdapter.Connection.ConnectionString = Program.connstr;
            this.pHIEUNHAPTableAdapter.Fill(this.dS.PHIEUNHAP);

            if (Program.mGroup == "CONGTY") // bật tắt phân quyền 
            {
                btnThoat.Enabled = true;
                btnThem.Enabled = btnPhucHoi.Enabled = btnSua.Enabled = btnXoa.Enabled = btnGhi.Enabled = false;
                groupBox1.Enabled = btnCTPX.Enabled = false;
            }
            btnGhi.Enabled = btnPhucHoi.Enabled = groupBox1.Enabled = false;
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            bdsPhieuXuat.AddNew();
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = gcPhieuXuat.Enabled = false;
            btnGhi.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled = true;
            luaChon = THEM;
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            txtMAPX.Enabled = btnThem.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnSua.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = btnThoat.Enabled = true;
            luaChon = SUA;

            PhieuXuat phieuxuat = new PhieuXuat(txtMAPX.Text, dtNgay.Text, txtHOTENKH.Text, cmbMANV.Text, cmbMAKHO.Text); //truyền các giá trị vô KHO
            ObjectUndo ob = new ObjectUndo(luaChon, phieuxuat);
            st.Push(ob);
            updateButtonPhucHoi();
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (luaChon)
            {
                case THEM:
                    if (txtMAPX.Text.Trim() == "")
                    {
                        MessageBox.Show("Mã PHIẾU XUẤT không được thiếu!", "", MessageBoxButtons.OK);
                        txtMAPX.Focus();
                        return;
                    }
                    if (dtNgay.Text.Trim() == "")
                    {
                        MessageBox.Show("NGÀY không được thiếu!", "", MessageBoxButtons.OK);
                        dtNgay.Focus();
                        return;
                    }
                    if (txtHOTENKH.Text.Trim() == "")
                    {
                        MessageBox.Show("HỌ TÊN KHÁCH HÀNG không được thiếu!", "", MessageBoxButtons.OK);
                        txtHOTENKH.Focus();
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
                        String strLenh = "dbo.SP_KTMMAPHIEUXUAT";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh;
                        Program.sqlcmd.Parameters.Add("@MAPX", SqlDbType.NChar).Value = txtMAPX.Text;
                        Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        Program.sqlcmd.ExecuteNonQuery();
                        Program.conn.Close();
                        String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                        if (Ret != "0")
                        {
                            MessageBox.Show("Mã PHIẾU XUẨT bị trùng!", "", MessageBoxButtons.OK);
                            txtMAPX.Focus();
                            return;
                        }

                      
                        bdsPhieuXuat.EndEdit();
                        bdsPhieuXuat.ResetCurrentItem();
                        this.pHIEUXUATTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.pHIEUXUATTableAdapter.Update(this.dS.PHIEUXUAT);
                        string lenh = "exec SP_UndoThemPhieuXuat'" + txtMAPX.Text + "'";
                        ObjectUndo ob = new ObjectUndo(luaChon, lenh);
                        st.Push(ob);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ghi PHIẾU XUẨT.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcPhieuXuat.Enabled = true;
                    btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnThoat.Enabled = true;
                    btnGhi.Enabled = false; groupBox1.Enabled = false;
                    break;

                case SUA:
                    try
                    {
                        bdsPhieuXuat.EndEdit();
                        bdsPhieuXuat.ResetCurrentItem();
                        this.pHIEUXUATTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.pHIEUXUATTableAdapter.Update(this.dS.PHIEUXUAT);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi sửa PHIẾU XUẤT.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcPhieuXuat.Enabled = true;
                    btnThem.Enabled = btnThoat.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnSua.Enabled = true;
                    btnGhi.Enabled = groupBox1.Enabled = false;
                    break;
            }
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            String strLenh = "dbo.SP_PhieuXuat_Ktra_CTPX";
            Program.sqlcmd = Program.conn.CreateCommand();
            Program.sqlcmd.CommandType = CommandType.StoredProcedure;
            Program.sqlcmd.CommandText = strLenh;
            Program.sqlcmd.Parameters.Add("@MAPX", SqlDbType.NChar).Value = txtMAPX.Text;
            Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            Program.sqlcmd.ExecuteNonQuery();
            Program.conn.Close();
            String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
            if (Ret != "0")
            {
                MessageBox.Show("PHIẾU XUẤT CÓ CTPX KHÔNG XÓA ĐƯỢC!", "", MessageBoxButtons.OK);
                txtMAPX.Focus();
                return;
            }
            if (MessageBox.Show("Bạn có thật sự muốn xóa PHIẾU XUẤT này ?? ", "Xác nhận",
                     MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    luaChon = XOA;
                    PhieuXuat phieuxuat = new PhieuXuat(txtMAPX.Text, dtNgay.Text, txtHOTENKH.Text, cmbMANV.Text, cmbMAKHO.Text); ; //truyền các giá trị vô KHO
                    ObjectUndo ob = new ObjectUndo(luaChon, phieuxuat);
                    st.Push(ob);

                    bdsPhieuXuat.RemoveCurrent();
                    this.pHIEUXUATTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.pHIEUXUATTableAdapter.Update(this.dS.PHIEUXUAT);
                    updateButtonPhucHoi();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa PHIẾU XUẨT. Bạn hãy xóa lại\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.pHIEUXUATTableAdapter.Fill(this.dS.PHIEUXUAT);
                    return;
                }
            }
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btnSua.Enabled == false || btnThem.Enabled == false)
            {
                this.bdsPhieuXuat.CancelEdit();
                //if (btnThem.Enabled == false) bdsKho.Position = vitri;
                gcPhieuXuat.Enabled = true;
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
                        this.pHIEUXUATTableAdapter.Fill(this.dS.PHIEUXUAT);
                        break;

                    case SUA:
                        MessageBox.Show("Khôi phục sau khi SỬA ");
                        PhieuXuat phieuxuat = (PhieuXuat)ob.obj;
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh = "dbo.SP_UndoSuaPhieuXuat";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh;
                        Program.sqlcmd.Parameters.Add("@MAPX", SqlDbType.NChar).Value = phieuxuat.maPhieuXuat;
                        Program.sqlcmd.Parameters.Add("@NGAY", SqlDbType.Date).Value = phieuxuat.ngay;
                        Program.sqlcmd.Parameters.Add("@HOTENKH", SqlDbType.NVarChar).Value = phieuxuat.HotenKH;
                        Program.sqlcmd.Parameters.Add("@MANV", SqlDbType.Int).Value = phieuxuat.MaNV;
                        Program.sqlcmd.Parameters.Add("@MAKHO", SqlDbType.NChar).Value = phieuxuat.MaKho;


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
                bdsPhieuXuat.EndEdit();
                bdsPhieuXuat.ResetCurrentItem();
                this.pHIEUXUATTableAdapter.Connection.ConnectionString = Program.connstr;
                this.pHIEUXUATTableAdapter.Update(this.dS.PHIEUXUAT);

                updateButtonPhucHoi();
                reload();


            }
            catch (Exception)
            {
                MessageBox.Show("Không có gì để UNDO", "THÔNG BÁO", MessageBoxButtons.OK);
            }
        }

        private void reload() {
            try
            {
                this.pHIEUXUATTableAdapter.Fill(this.dS.PHIEUXUAT);

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

        private void btnCTPX_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Program.maKho = cmbMAKHO.Text;
            Program.maPX = txtMAPX.Text;
            FormCTPX fr = new FormCTPX();
            fr.ShowDialog();
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void updateButtonPhucHoi()
        {
            if (st.Count == 0)
                btnPhucHoi.Enabled = false;
            else
                btnPhucHoi.Enabled = true;
        }
    }
    public class PhieuXuat
    {
        public String maPhieuXuat;
        public String ngay;
        public String HotenKH;
        public String MaNV;
        public String MaKho;

        public PhieuXuat(String maPhieuXuat, String ngay, String HotenKH, String MaNV, String MaKho)
        {
            this.maPhieuXuat = maPhieuXuat;
            this.ngay = ngay;
            this.HotenKH = HotenKH;
            this.MaNV = MaNV;
            this.MaKho = MaKho;
        }
    }
}
