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
    public partial class FormVatTu : Form
    {
        const int THEM = 0;
        const int SUA = 1;
        const int XOA = 2;
        int vitri = 0;
        int luaChon;

        public Stack st = new Stack();

        public FormVatTu()
        {
            InitializeComponent();
        }

        private void vATTUBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsVATTU.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void FormVatTu_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;
            this.vATTUTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vATTUTableAdapter.Fill(this.dS.VATTU);
            if (Program.mGroup == "CONGTY") // bật tắt phân quyền 
            {
                btnThoat.Enabled = btnInDSVatTu.Enabled = true;
                btnThem.Enabled = btnPhucHoi.Enabled = btnSua.Enabled = btnXoa.Enabled = btnGhi.Enabled = false;
                groupBox1.Enabled = false;
            }
            else if (Program.mGroup == "CHINHANH")
            {

                groupBox1.Enabled = btnGhi.Enabled = btnPhucHoi.Enabled = false;
                
            }
            else
            {
                groupBox1.Enabled = btnGhi.Enabled = btnPhucHoi.Enabled = btnInDSVatTu.Enabled = false;
            }

        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            bdsVATTU.AddNew();
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = gcVatTu.Enabled = false;
            btnGhi.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled = true;
            //txtSOLUONGTON.Value = 0;
            txtSOLUONGTON.Enabled = true;

            luaChon = THEM;
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            txtMAVT.Enabled = btnThem.Enabled = btnXoa.Enabled = btnReLoad.Enabled = txtSOLUONGTON.Enabled = btnSua.Enabled = false;
            btnGhi.Enabled = btnPhucHoi.Enabled = btnThoat.Enabled = true;
            luaChon = SUA;

            VatTu vattu = new VatTu(txtMAVT.Text, txtTENVT.Text, txtDVT.Text, txtSOLUONGTON.Text); //truyền các giá trị vô KHO
            ObjectUndo ob = new ObjectUndo(luaChon, vattu);
            st.Push(ob);
            updateButtonPhucHoi();
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (luaChon)
            {
                case THEM:
                    if (txtMAVT.Text.Trim() == "")
                    {
                        MessageBox.Show("Mã Vật Tư không được thiếu!", "", MessageBoxButtons.OK);
                        txtMAVT.Focus();
                        return;
                    }
                    if (txtTENVT.Text.Trim() == "")
                    {
                        MessageBox.Show("Tên Vật Tư không được thiếu!", "", MessageBoxButtons.OK);
                        txtTENVT.Focus();
                        return;
                    }
                    if (txtDVT.Text.Trim() == "")
                    {
                        MessageBox.Show("Đơn vị tính không được thiếu!", "", MessageBoxButtons.OK);
                        txtDVT.Focus();
                        return;
                    }
                    if (txtSOLUONGTON.Text.Trim() == "")
                    {
                        MessageBox.Show("SỐ LƯỢNG TỒN không được thiếu!", "", MessageBoxButtons.OK);
                        txtSOLUONGTON.Focus();
                        return;
                    }
                    if (Program.conn.State == ConnectionState.Closed)
                        Program.conn.Open();
                    String strLenh = "dbo.SP_KiemTraMAVT";
                    Program.sqlcmd = Program.conn.CreateCommand();
                    Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                    Program.sqlcmd.CommandText = strLenh;
                    Program.sqlcmd.Parameters.Add("@MAVT", SqlDbType.NChar).Value = txtMAVT.Text;
                    Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.NChar).Direction = ParameterDirection.ReturnValue;
                    Program.sqlcmd.ExecuteNonQuery();
                    Program.conn.Close();
                    String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
                    if (Ret != "0")
                    {
                        MessageBox.Show("Mã Vật Tư bị trùng!", "", MessageBoxButtons.OK);
                        txtMAVT.Focus();
                        return;
                    }
                    try
                    {
                        bdsVATTU.EndEdit();
                        bdsVATTU.ResetCurrentItem();
                        this.vATTUTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.vATTUTableAdapter.Update(this.dS.VATTU);
                        string lenh = "exec SP_UndoThemVatTu'" + txtMAVT.Text + "'";
                        ObjectUndo ob = new ObjectUndo(luaChon, lenh);
                        st.Push(ob);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ghi VẬT TƯ.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcVatTu.Enabled = true;
                    btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnThoat.Enabled = true;
                    btnGhi.Enabled = false; groupBox1.Enabled = false;
                    break;

                case SUA:
                    try
                    {
                        bdsVATTU.EndEdit();
                        bdsVATTU.ResetCurrentItem();
                        this.vATTUTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.vATTUTableAdapter.Update(this.dS.VATTU);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi ghi VẬT TƯ.\n" + ex.Message, "", MessageBoxButtons.OK);
                        return;
                    }
                    gcVatTu.Enabled = true;
                    btnThem.Enabled = btnThoat.Enabled = btnXoa.Enabled = btnReLoad.Enabled = btnSua.Enabled = true;
                    btnGhi.Enabled = groupBox1.Enabled = false;
                    break;
            }
        }


        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string mavt = "";

            if (MessageBox.Show("Bạn có thật sự muốn xóa VẬT TƯ này ?? ", "Xác nhận",
                       MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    luaChon = XOA;
                    VatTu vattu = new VatTu(txtMAVT.Text, txtTENVT.Text, txtDVT.Text, txtSOLUONGTON.Text); //truyền các giá trị vô KHO
                    ObjectUndo ob = new ObjectUndo(luaChon, vattu);
                    st.Push(ob);

                   // mavt = (((DataRowView)bdsVATTU[bdsVATTU.Position])["MAVT"].ToString()); // giữ lại để khi xóa bị lỗi thì ta sẽ quay về lại
                    bdsVATTU.RemoveCurrent();
                    this.vATTUTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.vATTUTableAdapter.Update(this.dS.VATTU);
                   // mavt = txtMAVT.Text;
                    updateButtonPhucHoi();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa Vật Tư. Bạn hãy xóa lại\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.vATTUTableAdapter.Fill(this.dS.VATTU);
                    //bdsVATTU.Position = bdsVATTU.Find("MAVT", mavt);
                    return;
                }
            }
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btnSua.Enabled == false || btnThem.Enabled == false)
            {
                this.bdsVATTU.CancelEdit();
                //if (btnThem.Enabled == false) bdsKho.Position = vitri;
                gcVatTu.Enabled = true;
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
                        this.vATTUTableAdapter.Fill(this.dS.VATTU);
                        break;

                    case SUA:
                        MessageBox.Show("Khôi phục sau khi SỬA ");
                        VatTu vattu = (VatTu)ob.obj;
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh = "dbo.SP_UndoSuaVatTu";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh;
                        Program.sqlcmd.Parameters.Add("@MAVT", SqlDbType.NChar).Value = vattu.maVT;
                        Program.sqlcmd.Parameters.Add("@TENVT", SqlDbType.NChar).Value = vattu.tenVT;
                        Program.sqlcmd.Parameters.Add("@DVT", SqlDbType.NChar).Value = vattu.DVT;
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
                        VatTu vattu1 = (VatTu)ob.obj;
                        if (Program.conn.State == ConnectionState.Closed)
                            Program.conn.Open();
                        String strLenh1 = "dbo.SP_UndoXoaVatTu";
                        Program.sqlcmd = Program.conn.CreateCommand();
                        Program.sqlcmd.CommandType = CommandType.StoredProcedure;
                        Program.sqlcmd.CommandText = strLenh1;
                        Program.sqlcmd.Parameters.Add("@MAVT", SqlDbType.NChar).Value = vattu1.maVT;
                        Program.sqlcmd.Parameters.Add("@TENVT", SqlDbType.NChar).Value = vattu1.tenVT;
                        Program.sqlcmd.Parameters.Add("@DVT", SqlDbType.NChar).Value = vattu1.DVT;
                        Program.sqlcmd.Parameters.Add("@SOLUONGTON", SqlDbType.NChar).Value = vattu1.SoLuongTon;
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
                bdsVATTU.EndEdit();
                bdsVATTU.ResetCurrentItem();
                this.vATTUTableAdapter.Connection.ConnectionString = Program.connstr;
                this.vATTUTableAdapter.Update(this.dS.VATTU);
                updateButtonPhucHoi();
                reload();
            }
            catch (Exception)
            {
                MessageBox.Show("Không có gì để UNDO", "THÔNG BÁO", MessageBoxButtons.OK);
            }
        }

        private void btnReLoad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reload();
        }

        private void updateButtonPhucHoi()
        {
            if (st.Count == 0)
                btnPhucHoi.Enabled = false;
            else
                btnPhucHoi.Enabled = true;
        }

        private void reload()
        {
            try
            {
                this.vATTUTableAdapter.Fill(this.dS.VATTU);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload :" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnInDSVatTu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form_rpDSVT fm = new Form_rpDSVT();
            fm.ShowDialog();
        }
    }

    public class VatTu
    {
        public String maVT;
        public String tenVT;
        public String DVT;
        public String SoLuongTon;

        public VatTu(String maVT, String tenVT, String DVT, String SoLuongTon)
        {
            this.maVT = maVT;
            this.tenVT = tenVT;
            this.DVT = DVT;
            this.SoLuongTon = SoLuongTon;
        }
    }
}

