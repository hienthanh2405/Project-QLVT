using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DTQLVT
{
    public partial class FormCTPX : Form
    {
        public FormCTPX()
        {
            InitializeComponent();
        }

        private void cTPXBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void FormCTPX_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dS.SP_VatTuCo_SoLuongTon_LonHon_0' table. You can move, or remove it, as needed.
            // TODO: This line of code loads data into the 'dS.CTPX' table. You can move, or remove it, as needed.
            dS.EnforceConstraints = false;
            txtMAPX.Text = Program.maPX;

            this.sP_DanhSachCTPhieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.sP_DanhSachCTPhieuXuatTableAdapter.Fill(this.dS.SP_DanhSachCTPhieuXuat, txtMAPX.Text);
            //this.sP_PhieuXuat_VatTu_TrongKhoTableAdapter.Fill(this.dS.SP_PhieuXuat_VatTu_TrongKho,)
            this.sP_VatTuCo_SoLuongTon_LonHon_0TableAdapter.Fill(this.dS.SP_VatTuCo_SoLuongTon_LonHon_0);
            initCmbVT();
            groupBox1.Enabled = btnGhi.Enabled = false;
        }

        private void initCmbVT()
        {
            String sql = "exec SP_PhieuXuat_VatTu_TrongKho N'" + Program.maKho.Trim() + "'";
            DataTable tb = Program.ExecSqlDataTable(sql);
            if (tb.Columns.Count > 0)
            {
                cmbMAVT.DataSource = tb;
                cmbMAVT.DisplayMember = "MAVT";
                cmbMAVT.ValueMember = "MAVT";
            }
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            sP_DanhSachCTPhieuXuatBindingSource.AddNew();
            txtMAPX.Text = Program.maPX;
            txtMAPX.Enabled = btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = gcCTPX.Enabled = false;
            btnGhi.Enabled = btnThoat.Enabled = true;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cmbMAVT.Text.Trim() == "")
            {
                MessageBox.Show("MÃ VẬT TƯ không được để trống!", "", MessageBoxButtons.OK);
                cmbMAVT.Focus();
                return;
            }
            if (txtSOLUONG.Text.Trim() == "")
            {
                MessageBox.Show("Mã SỐ LƯỢNG không được thiếu  !", "", MessageBoxButtons.OK);
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
                // kiểm tra xem vật tư này có có trong kho , số luong còn lại là bao nhiêu để giới hạn người dùng nhập liệu
                if (Program.conn.State == ConnectionState.Closed)
                    Program.conn.Open();
                String strLenh = "dbo.SP_PHIEUXUAT_SOLUONG";
                Program.sqlcmd = Program.conn.CreateCommand();
                Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                Program.sqlcmd.CommandText = strLenh;
                Program.sqlcmd.Parameters.Add("@MAKHO", SqlDbType.NChar).Value = Program.maKho.Trim();
                Program.sqlcmd.Parameters.Add("@MAVT", SqlDbType.NChar).Value = cmbMAVT.Text.Trim();
                Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                Program.sqlcmd.ExecuteNonQuery();
                Program.conn.Close();
                String Ret1 = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                if (Ret1.Equals("0"))
                {
                    MessageBox.Show("Vật tư này không còn trong kho hiện tại !", "", MessageBoxButtons.OK);
                    cmbMAVT.Focus();
                    return;
                }

                if (Convert.ToInt32(Ret1.Trim()) < Int32.Parse(txtSOLUONG.Text.Trim()))
                {
                    MessageBox.Show("Số lượng vật tư trong kho không đủ để xuất !!!. Max = " + Ret1, "", MessageBoxButtons.OK);
                    txtSOLUONG.Focus();
                    return;
                }
                //update so lượng tôn của mã vật tư sau khi xuất
                if (Program.conn.State == ConnectionState.Closed)
                    Program.conn.Open();
                String strLenh1 = "dbo.SP_PhieuXuat_UpdateSoLuongTon";
                Program.sqlcmd = Program.conn.CreateCommand();
                Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                Program.sqlcmd.CommandText = strLenh1;
                Program.sqlcmd.Parameters.Add("@MAVT", SqlDbType.NChar).Value = cmbMAVT.Text;
                Program.sqlcmd.Parameters.Add("@SOLUONGXUAT", SqlDbType.Int).Value = txtSOLUONG.Text;
                Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
                Program.sqlcmd.ExecuteNonQuery();
                Program.conn.Close();
                String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();


                sP_DanhSachCTPhieuXuatBindingSource.EndEdit();
                sP_DanhSachCTPhieuXuatBindingSource.ResetCurrentItem();
                this.sP_DanhSachCTPhieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.sP_DanhSachCTPhieuXuatTableAdapter.Insert(txtMAPX.Text.Trim(),
                                                                cmbMAVT.Text.Trim(),
                                                                Int32.Parse(txtSOLUONG.Text.Trim()),
                                                                Double.Parse(txtDONGIA.Text.Trim()));
                
               

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi CTPN.\n" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
            gcCTPX.Enabled = true;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = false; groupBox1.Enabled = false;
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //if (MessageBox.Show("Bạn có thật sự muốn xóa CTPX này ?? ", "Xác nhận",
            //         MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    try
            //    {
            //        //sPDanhSachCTPhieuXuatBindingSource.RemoveCurrent();
            //        //this.sP_DanhSachCTPhieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            //        //this.sP_DanhSachCTPhieuXuatTableAdapter.Delete(txtMAPX.Text.Trim(),
            //        //                                    cmbMAVT.Text.Trim());

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Lỗi xóa PHIẾU XUẤT. Bạn hãy xóa lại\n" + ex.Message, "", MessageBoxButtons.OK);
            //        this.sP_DanhSachCTPhieuXuatTableAdapter.Fill(this.dS.SP_DanhSachCTPhieuXuat, Program.maPX);
            //        return;
            //    }
            //}
        }

        private void reload()
        {
            try
            {
                this.sP_DanhSachCTPhieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.sP_DanhSachCTPhieuXuatTableAdapter.Fill(this.dS.SP_DanhSachCTPhieuXuat, Program.maPX);

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

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.sP_DanhSachCTPhieuXuatTableAdapter.Fill(this.dS.SP_DanhSachCTPhieuXuat, mAPXToolStripTextBox.Text);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }


    }
}
