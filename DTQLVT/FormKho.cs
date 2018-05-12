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
    public partial class FormKho : Form
    {
        const int THEM = 0;
        const int SUA = 1;
        const int XOA = 2;
        int luaChon;

        int vitri = 0;
        string macn = "";

        public Stack st = new Stack();

        public FormKho()
        {
            InitializeComponent();
        }

        private void kHOBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsKho.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void FormKho_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.KHO' table. You can move, or remove it, as needed.      
            this.kHOTableAdapter.Fill(this.dS.KHO);
            this.kHOTableAdapter.Connection.ConnectionString = Program.connstr;
            this.dATHANGTableAdapter.Fill(this.dS.DATHANG);
            this.dATHANGTableAdapter.Connection.ConnectionString = Program.connstr;
            this.pHIEUNHAPTableAdapter.Fill(this.dS.PHIEUNHAP);
            this.pHIEUNHAPTableAdapter.Connection.ConnectionString = Program.connstr;
            this.pHIEUXUATTableAdapter.Fill(this.dS.PHIEUXUAT);
            this.pHIEUXUATTableAdapter.Connection.ConnectionString = Program.connstr;

            macn = ((DataRowView)bdsKho[0])["MACN"].ToString();
            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập  qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;
            if (Program.mGroup == "CONGTY")// Phân quyền 
            {
                cmbChiNhanh.Enabled = true;
                btnThoat.Enabled = true;
                groupBox1.Enabled = false;
                btnThem.Enabled = btnPhucHoi.Enabled = btnSua.Enabled = btnXoa.Enabled = btnGhi.Enabled = false;
            }
            else
            {
                cmbChiNhanh.Enabled = groupBox1.Enabled = btnGhi.Enabled = false;
            }

        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
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
                    this.kHOTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.kHOTableAdapter.Fill(this.dS.KHO);
                }
            }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //vitri = bdsKho.Position;
            groupBox1.Enabled = true;
            bdsKho.AddNew();
            txtMACN.Text = macn;
            txtMACN.Enabled = false;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnThoat.Enabled = gcKho.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = true;
            luaChon = THEM;
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            txtMACN.Enabled = btnThem.Enabled = btnXoa.Enabled = btnReLoad.Enabled = txtMAKHO.Enabled = btnSua.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = btnThoat.Enabled = true;
            luaChon = SUA;

            Kho kho = new Kho(txtMAKHO.Text, txtTENKHO.Text, txtDIACHI.Text, txtMACN.Text); //truyền các giá trị vô KHO
            ObjectUndo ob = new ObjectUndo(luaChon, kho);
            st.Push(ob);
            updateButtonPhucHoi();
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string makho = "";
            if (bdsPHIEUNHAP.Count > 0)
            {
                MessageBox.Show("Không xóa được Kho, Kho đã có PHIẾU NHẬP", "", MessageBoxButtons.OK);
                return;
            }
            if (bdsPHIEUXUAT.Count > 0)
            {
                MessageBox.Show("Không xóa được Kho, Kho đã có PHIẾU XUẤT", "", MessageBoxButtons.OK);
                return;
            }
            if (bdsDATHANG.Count > 0)
            {
                MessageBox.Show("Không xóa được Kho, Kho đã có phiếu ĐẶT HÀNG", "", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn có thật sự muốn xóa Kho này ?? ", "Xác nhận",
                       MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    luaChon = XOA;
                    Kho kho = new Kho(txtMAKHO.Text, txtTENKHO.Text, txtDIACHI.Text, txtMACN.Text); //truyền các giá trị vô KHO
                    ObjectUndo ob = new ObjectUndo(luaChon, kho);
                    st.Push(ob);

                    //makho = (((DataRowView)bdsKho[bdsKho.Position])["MAKHO"].ToString()); // giữ lại để khi xóa bị lỗi thì ta sẽ quay về lại
                    bdsKho.RemoveCurrent();
                    this.kHOTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.kHOTableAdapter.Update(this.dS.KHO);
                    this.pHIEUNHAPTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.pHIEUNHAPTableAdapter.Update(this.dS.PHIEUNHAP);
                    this.pHIEUXUATTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.pHIEUXUATTableAdapter.Update(this.dS.PHIEUXUAT);
                    this.dATHANGTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.dATHANGTableAdapter.Update(this.dS.DATHANG);
                   // makho = txtMAKHO.Text;
                    updateButtonPhucHoi();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa Kho. Bạn hãy xóa lại\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.kHOTableAdapter.Fill(this.dS.KHO);
                    //bdsKho.Position = bdsKho.Find("MAKHO", makho);
                    return;
                }
            }
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btnSua.Enabled == false || btnThem.Enabled == false)
            {
                this.bdsKho.CancelEdit();
                //if (btnThem.Enabled == false) bdsKho.Position = vitri;
                gcKho.Enabled = true;
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
                        this.kHOTableAdapter.Fill(this.dS.KHO);
                        break;

                    case SUA:
                        MessageBox.Show("Khôi phục sau khi SỬA ");
                        Kho kho = (Kho)ob.obj;
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh = "dbo.SP_UndoSuaKho";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh;
                        Program.sqlcmd.Parameters.Add("@MAKHO", SqlDbType.NChar).Value = kho.maKho;
                        Program.sqlcmd.Parameters.Add("@TENKHO", SqlDbType.NVarChar).Value = kho.tenKho;
                        Program.sqlcmd.Parameters.Add("@DIACHI", SqlDbType.NVarChar).Value = kho.diaChi;
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
                        Kho kho1 = (Kho)ob.obj;
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh1 = "dbo.SP_UndoXoaKho";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh1;
                        Program.sqlcmd.Parameters.Add("@MAKHO", SqlDbType.NChar).Value = kho1.maKho;
                        Program.sqlcmd.Parameters.Add("@TENKHO", SqlDbType.NVarChar).Value = kho1.tenKho;
                        Program.sqlcmd.Parameters.Add("@DIACHI", SqlDbType.NVarChar).Value = kho1.diaChi;
                        Program.sqlcmd.Parameters.Add("@MACN", SqlDbType.NChar).Value = kho1.MaCN;
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
                bdsKho.EndEdit();
                bdsKho.ResetCurrentItem();
                this.kHOTableAdapter.Connection.ConnectionString = Program.connstr;
                this.kHOTableAdapter.Update(this.dS.KHO);

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

        private void btnReLoad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reload();
        }

        private void reload()
        {
            try
            {
                this.kHOTableAdapter.Fill(this.dS.KHO);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload :" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (luaChon)
            {
                case THEM:
                    if (txtMAKHO.Text.Trim() == "")
                    {
                        MessageBox.Show("Mã Kho không được thiếu!", "", MessageBoxButtons.OK);
                        txtMAKHO.Focus();
                        return;
                    }
                    if (txtTENKHO.Text.Trim() == "")
                    {
                        MessageBox.Show("Tên Kho không được thiếu!", "", MessageBoxButtons.OK);
                        txtTENKHO.Focus();
                        return;
                    }
                    if (txtDIACHI.Text.Trim() == "")
                    {
                        MessageBox.Show("Địa chỉ không được thiếu!", "", MessageBoxButtons.OK);
                        txtDIACHI.Focus();
                        return;
                    }
                    if (Program.conn.State == ConnectionState.Closed)
                        Program.conn.Open();
                    String strLenh = "dbo.SP_KiemTraMAKHO";
                    Program.sqlcmd = Program.conn.CreateCommand();
                    Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                    Program.sqlcmd.CommandText = strLenh;
                    Program.sqlcmd.Parameters.Add("@MAKHO", SqlDbType.NChar).Value = txtMAKHO.Text;
                    Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
                    Program.sqlcmd.ExecuteNonQuery();
                    Program.conn.Close();
                    String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                    if (Ret != "0")
                    {
                        MessageBox.Show("Mã Kho bị trùng!", "", MessageBoxButtons.OK);
                        txtMAKHO.Focus();
                        return;
                    }
                    try
                    {
                        bdsKho.EndEdit();
                        bdsKho.ResetCurrentItem();
                        this.kHOTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.kHOTableAdapter.Update(this.dS.KHO);
                        string lenh = "exec SP_UndoThemKho'" + txtMAKHO.Text + "'";
                        ObjectUndo ob = new ObjectUndo(luaChon, lenh);
                        st.Push(ob);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ghi kho.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcKho.Enabled = true;
                    btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnThoat.Enabled = true;
                    btnGhi.Enabled = false; groupBox1.Enabled = false;
                    break;

                case SUA:
                    try
                    {
                        bdsKho.EndEdit();
                        bdsKho.ResetCurrentItem();
                        this.kHOTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.kHOTableAdapter.Update(this.dS.KHO);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi sửa kho.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcKho.Enabled = true;
                    btnThem.Enabled = btnThoat.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnSua.Enabled = true;
                    btnGhi.Enabled = groupBox1.Enabled = false;
                    break;
            }
        }
    }

    public class ObjectUndo
    {
        public int luaChon;
        public Object obj;

        public ObjectUndo(int t, Object obj)
        {
            this.luaChon = t;
            this.obj = obj;
        }
    }

    public class Kho
    {
        public String maKho;
        public String tenKho;
        public String diaChi;
        public String MaCN;

        public Kho(String maKho, String tenKho, String diaChi, String MaCN)
        {
            this.maKho = maKho;
            this.tenKho = tenKho;
            this.diaChi = diaChi;
            this.MaCN = MaCN;
        }
    }
}
