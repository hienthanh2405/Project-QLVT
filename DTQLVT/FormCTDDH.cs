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
    public partial class FormCTDDH : Form
    {

        int choose = 0;

        public FormCTDDH()
        {
            InitializeComponent();
        }

        //private void cTDDHBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        //{
        //    this.Validate();
        //    this.bdsCTDDH.EndEdit();
        //    this.tableAdapterManager.UpdateAll(this.dS);

        //}


        private void FormCTDDH_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dS.DATHANG' table. You can move, or remove it, as needed.

            this.dS.EnforceConstraints = false;
            // TODO: This line of code loads data into th 'dS.VATTU' table. You can move, or remove it, as needed.
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Fill(this.dS.CTDDH);
            this.vATTUTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vATTUTableAdapter.Fill(this.dS.VATTU);
            txtMasoDDH.Text = Program.maDH;
            this.sP_DanhSachCTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.sP_DanhSachCTDDHTableAdapter.Fill(this.dS.SP_DanhSachCTDDH, txtMasoDDH.Text.Trim());

            groupBox1.Enabled = false;

        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            sP_DanhSachCTDDHBindingSource.AddNew();
            txtMasoDDH.Text = Program.maDH;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = gcCTDDH.Enabled = false;
            btnGhi.Enabled = btnThoat.Enabled = true;
            choose = Program.THEM;
        }

       

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (choose)
            {
                case Program.THEM:
                    if (cmbMAVT.Text.Trim() == "")
                    {
                        MessageBox.Show("Mã VẬT TƯ không được thiếu!", "", MessageBoxButtons.OK);
                        cmbMAVT.Focus();
                        return;
                    }
                    if (txtDONGIA.Text.Trim() == "")
                    {
                        MessageBox.Show("SỐ LƯỢNG không được thiếu!", "", MessageBoxButtons.OK);
                        txtDONGIA.Focus();
                        return;
                    }
                    if (txtSOLUONG.Text.Trim() == "")
                    {
                        MessageBox.Show("ĐƠN GIÁ không được thiếu!", "", MessageBoxButtons.OK);
                        txtSOLUONG.Focus();
                        return;
                    }
                    try
                    {
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh = "dbo.SP_KTRAMAVTCTDDH";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh;
                        Program.sqlcmd.Parameters.Add("@MasoDDH", SqlDbType.NChar).Value = txtMasoDDH.Text;
                        Program.sqlcmd.Parameters.Add("@MAVT", SqlDbType.NChar).Value = cmbMAVT.Text;
                        Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
                        Program.sqlcmd.ExecuteNonQuery();
                        Program.conn.Close();
                        String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                        if (Ret != "0")
                        {
                            MessageBox.Show("Mã Vat tu bị trùng!", "", MessageBoxButtons.OK);
                            cmbMAVT.Focus();
                            return;
                        }
                        sP_DanhSachCTDDHBindingSource.EndEdit();
                        sP_DanhSachCTDDHBindingSource.ResetCurrentItem();
                        //this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.sP_DanhSachCTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.sP_DanhSachCTDDHTableAdapter.Insert(txtMasoDDH.Text.Trim(),
                                                                cmbMAVT.Text.Trim(),
                                                                Int32.Parse(txtSOLUONG.Text.Trim()),
                                                                Double.Parse(txtDONGIA.Text.Trim()));
                        reload();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ghi CTDDH.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcCTDDH.Enabled = true;
                    btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnThoat.Enabled = true;
                    btnGhi.Enabled = false; groupBox1.Enabled = false;

                    break;
            }
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            String strLenh1 = "dbo.SP_KTRA_DDHCOPPHIEUNHAP";
            Program.sqlcmd = Program.conn.CreateCommand();
            Program.sqlcmd.CommandType = CommandType.StoredProcedure;
            Program.sqlcmd.CommandText = strLenh1;
            Program.sqlcmd.Parameters.Add("@MasoDDH", SqlDbType.NChar).Value = txtMasoDDH.Text;
            Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
            Program.sqlcmd.ExecuteNonQuery();
            Program.conn.Close();
            String Ret1 = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
            if (Ret1 != "0")
            {
                MessageBox.Show("Mã ĐƠN ĐẶT HÀNG đã có Phiếu Nhập!", "", MessageBoxButtons.OK);
                txtMasoDDH.Focus();
                return;
            }
            if (MessageBox.Show("Bạn có thật sự muốn xóa CTDDH này ?? ", "Xác nhận",
                      MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    sP_DanhSachCTDDHBindingSource.RemoveCurrent();
                    this.sP_DanhSachCTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.sP_DanhSachCTDDHTableAdapter.Delete(txtMasoDDH.Text.Trim(),
                                                        cmbMAVT.Text.Trim());
                    //reload();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa CTDDH. Bạn hãy xóa lại\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.sP_DanhSachCTDDHTableAdapter.Fill(this.dS.SP_DanhSachCTDDH,Program.maDH);
                    return;
                }
            }
        }

        private void reload()
        {
            try
            {
                this.sP_DanhSachCTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                this.sP_DanhSachCTDDHTableAdapter.Fill(this.dS.SP_DanhSachCTDDH, Program.maDH);

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

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        

    }
}
