using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DTQLVT
{
    public partial class FormCTPN : Form
    {
        public FormCTPN()
        {
            InitializeComponent();
        }

        private void cTPNBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsCTPN.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void FormCTPN_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dS.VATTU' table. You can move, or remove it, as needed.

            dS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.CTPN' table. You can move, or remove it, as needed.
            this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPNTableAdapter.Fill(this.dS.CTPN);
            this.vATTUTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vATTUTableAdapter.Fill(this.dS.VATTU);
            this.sP_DanhSachCTPhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.sP_DanhSachCTPhieuNhapTableAdapter.Fill(this.dS.SP_DanhSachCTPhieuNhap, Program.maPN);

            txtMAPN.Text = Program.maPN;
            groupBox1.Enabled = btnGhi.Enabled = false;
            String sql = "exec SP_KTRAVATTUTRONGPHIEUNHAP N'" + txtMAPN.Text.Trim() + "'";
            DataTable tb = Program.ExecSqlDataTable(sql);
            if (tb.Columns.Count > 0)
            {
                cmbMAVT.DataSource = tb;
                cmbMAVT.DisplayMember = "MAVT";
                cmbMAVT.ValueMember = "MAVT";
            }

            initDonGia();
            initSoLuong();
            txtMAPN.Enabled = false;

        }

        private void initSoLuong()
        {
            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            String strLenh = "dbo.SP_LAY_SO_LUONG";
            Program.sqlcmd = Program.conn.CreateCommand();
            Program.sqlcmd.CommandType = CommandType.StoredProcedure;
            Program.sqlcmd.CommandText = strLenh;
            Program.sqlcmd.Parameters.Add("@MAPN", SqlDbType.NChar).Value = txtMAPN.Text;
            Program.sqlcmd.Parameters.Add("@MAVT", SqlDbType.NChar).Value = cmbMAVT.Text;
            Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
            Program.sqlcmd.ExecuteNonQuery();
            Program.conn.Close();
            String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();

            txtSOLUONG.Text = Ret;

        }

        private void initDonGia()
        {
            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            String strLenh = "dbo.SP_LAY_DON_GIA";
            Program.sqlcmd = Program.conn.CreateCommand();
            Program.sqlcmd.CommandType = CommandType.StoredProcedure;
            Program.sqlcmd.CommandText = strLenh;
            Program.sqlcmd.Parameters.Add("@MAPN", SqlDbType.NChar).Value = txtMAPN.Text;
            Program.sqlcmd.Parameters.Add("@MAVT", SqlDbType.NChar).Value = cmbMAVT.Text;
            Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
            Program.sqlcmd.ExecuteNonQuery();
            Program.conn.Close();
            String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();

            txtDONGIA.Text = Ret;
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            sPDanhSachCTPhieuNhapBindingSource.AddNew();
            txtMAPN.Text = Program.maPN;
            txtMAPN.Enabled = btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = gcCTPN.Enabled = false;
            btnGhi.Enabled = btnThoat.Enabled = true;
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            btnThem.Enabled = btnXoa.Enabled = btnReload.Enabled = btnSua.Enabled = false;
            btnGhi.Enabled = btnThoat.Enabled = true;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtSOLUONG.Text.Trim() == "")
            {
                MessageBox.Show("Mã SỐ LƯỢNG không được thiếu!", "", MessageBoxButtons.OK);
                txtSOLUONG.Focus();
                return;
            }
            if (txtDONGIA.Text.Trim() == "")
            {
                MessageBox.Show("Mã ĐƠN GIÁ không được thiếu!", "", MessageBoxButtons.OK);
                txtDONGIA.Focus();
                return;
            }
            try
            {
                // kiểm tra mã vặt từ đã nhập
                if (Program.conn.State == ConnectionState.Closed)
                    Program.conn.Open();
                String strLenh = "dbo.SP_CTPhieuNhap_KtraMAVT";
                Program.sqlcmd = Program.conn.CreateCommand();
                Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                Program.sqlcmd.CommandText = strLenh;
                Program.sqlcmd.Parameters.Add("@MAPN", SqlDbType.NChar).Value = txtMAPN.Text;
                Program.sqlcmd.Parameters.Add("@MAVT", SqlDbType.NChar).Value = cmbMAVT.Text;
                Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                Program.sqlcmd.ExecuteNonQuery();
                Program.conn.Close();
                String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                if (Ret != "0")
                {
                    MessageBox.Show("Mã VẬT TƯ đã được nhập!", "", MessageBoxButtons.OK);
                    cmbMAVT.Focus();
                    return;
                }
                // kiểm tra số lương nhập 
                if (Program.conn.State == ConnectionState.Closed)
                    Program.conn.Open();
                String strLenh2 = "dbo.SP_PhieuNhap_Ktra_SoLuong";
                Program.sqlcmd = Program.conn.CreateCommand();
                Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                Program.sqlcmd.CommandText = strLenh2;
                Program.sqlcmd.Parameters.Add("@MAPN", SqlDbType.NChar).Value = txtMAPN.Text;
                Program.sqlcmd.Parameters.Add("@MAVT", SqlDbType.NChar).Value = cmbMAVT.Text;
                Program.sqlcmd.Parameters.Add("@SOLUONGNHAP", SqlDbType.Int).Value = txtSOLUONG.Text;
                Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                Program.sqlcmd.ExecuteNonQuery();
                Program.conn.Close();
                String Ret2 = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                if (Ret2 != "0")
                {
                    MessageBox.Show("Mã Số Lượng Nhập Phải nhỏ hơn hoặc bằng Số Lượng Đặt!", "", MessageBoxButtons.OK);
                    cmbMAVT.Focus();
                    return;
                }
                //update số lượng tồn của vật tư sau khi nhập 
                if (Program.conn.State == ConnectionState.Closed)
                    Program.conn.Open();
                String strLenh1 = "dbo.SP_PhieuNhap_UpdateSoLuongTon";
                Program.sqlcmd = Program.conn.CreateCommand();
                Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                Program.sqlcmd.CommandText = strLenh1;
                Program.sqlcmd.Parameters.Add("@MAVT", SqlDbType.NChar).Value = cmbMAVT.Text;
                Program.sqlcmd.Parameters.Add("@SOLUONGNHAP", SqlDbType.NChar).Value = txtSOLUONG.Text;
                Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                Program.sqlcmd.ExecuteNonQuery();
                Program.conn.Close();
                String Ret1 = Program.sqlcmd.Parameters["@Ret"].Value.ToString();

                sPDanhSachCTPhieuNhapBindingSource.EndEdit();
                sPDanhSachCTPhieuNhapBindingSource.ResetCurrentItem();
                this.sP_DanhSachCTPhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.sP_DanhSachCTPhieuNhapTableAdapter.Insert(txtMAPN.Text.Trim(),
                                                                cmbMAVT.Text.Trim(),
                                                                Int32.Parse(txtSOLUONG.Text.Trim()),
                                                                Double.Parse(txtDONGIA.Text.Trim()));
                reload();
                this.vATTUTableAdapter.Connection.ConnectionString = Program.connstr;
                this.vATTUTableAdapter.Update(this.dS.VATTU);

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("MAVT"))
                {
                    MessageBox.Show("Lỗi ghi CTPN.\n" + ex.Message, "", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Lỗi ghi CTPN.\n" + ex.Message, "", MessageBoxButtons.OK);

                }
                return;
            }
            gcCTPN.Enabled = true;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = false; groupBox1.Enabled = false;
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //if (MessageBox.Show("Bạn có thật sự muốn xóa CTPN này ?? ", "Xác nhận",
            //         MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    try
            //    {

            //        sPDanhSachCTPhieuNhapBindingSource.RemoveCurrent();
            //        this.sP_DanhSachCTPhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            //        this.sP_DanhSachCTPhieuNhapTableAdapter.Delete(txtMAPN.Text.Trim(),
            //                                            cmbMAVT.Text.Trim());

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Lỗi xóa PHIẾU NHẬP. Bạn hãy xóa lại\n" + ex.Message, "", MessageBoxButtons.OK);
            //        this.sP_DanhSachCTPhieuNhapTableAdapter.Fill(this.dS.SP_DanhSachCTPhieuNhap, Program.maPN);
            //        return;
            //    }
            //}
        }

        private void reload()
        {
            try
            {
                this.sP_DanhSachCTPhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.sP_DanhSachCTPhieuNhapTableAdapter.Fill(this.dS.SP_DanhSachCTPhieuNhap, Program.maPN);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload :" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }

        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reload();
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }




    }
}
