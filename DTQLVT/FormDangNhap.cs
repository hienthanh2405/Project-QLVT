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

namespace DTQLVT
{
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qLVTDataSet.V_DS_PHANMANG' table. You can move, or remove it, as needed.
            this.v_DS_PHANMANGTableAdapter.Fill(this.qLVTDataSet.V_DS_PHANMANG);
            tENCNComboBox.SelectedIndex = 1;
            tENCNComboBox.SelectedIndex = 0;
            
        }

        

        private void tENCNComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tENCNComboBox.SelectedValue != null)
                Program.servername = tENCNComboBox.SelectedValue.ToString();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if(txtLogin.Text.Trim() == "")
            {
                MessageBox.Show("Tài khoảng đăng nhập không được rỗng", "Báo lỗi đăng nhập", MessageBoxButtons.OK);
                txtLogin.Focus();
                return;
            }
            Program.mlogin = txtLogin.Text;
            Program.password = txtPass.Text;
            if (Program.KetNoi() == 0) return;
            //MessageBox.Show("Đăng nhập thành công.", " ", MessageBoxButtons.OK);
            Program.mChinhanh = tENCNComboBox.SelectedIndex;
            Program.bds_dspm = bdsDSPM;

            SqlDataReader myReader;
            Program.mloginDN = Program.mlogin;
            Program.passwordDN = Program.password;

            String strLenh = "EXEC  SP_DANGNHAP '" + Program.mlogin + "'";
            myReader = Program.ExecSqlDataReader(strLenh);
            if (myReader == null) return;
            myReader.Read();
            Program.username = myReader.GetString(0);     // Lay user name
            if (Convert.IsDBNull(Program.username))
            {
                MessageBox.Show("Login bạn nhập không có quyền truy cập dữ liệu\n Bạn xem lại username, password", "", MessageBoxButtons.OK);
                return;
            }
            Program.mHoten = myReader.GetString(1);
            Program.mGroup = myReader.GetString(2);
            MessageBox.Show("Đăng nhập thành công.", " ", MessageBoxButtons.OK);
            myReader.Close();
            Program.conn.Close();
            Program.frmChinh.HienThiMenu();
            Program.frmChinh.rbDanhMuc.Visible = true;
            Program.frmChinh.rbBaoCao.Visible = true;

            if (Program.mGroup == "USER")
            {
                Program.frmChinh.btnDangKy.Enabled = false;
                Program.frmChinh.rbBaoCao.Visible = false;
            }
            else {
                Program.frmChinh.btnDangKy.Enabled = true;
            }
            
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
            Program.frmChinh.rbDanhMuc.Visible = Program.frmChinh.rbBaoCao.Visible = false;
            Program.frmChinh.btnDangKy.Enabled = false;
        }
    }
}
