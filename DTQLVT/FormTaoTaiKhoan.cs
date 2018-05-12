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
    public partial class FormTaoTaiKhoan : Form
    {
        String macn = "";
        public FormTaoTaiKhoan()
        {
            InitializeComponent();
        }

        private void FormTaoTaiKhoan_Load(object sender, EventArgs e)
        {
            DS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.NHANVIEN' table. You can move, or remove it, as needed.
            this.NHANVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.NHANVIENTableAdapter.Fill(this.DS.NHANVIEN);

            macn = ((DataRowView)bdsNHANVIEN[0])["MACN"].ToString();
            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập  qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;
            if (Program.mGroup == "CONGTY")
            {
                cmbQuyen.Items.Add("CONGTY");
                cmbQuyen.SelectedIndex = 0;
                cmbChiNhanh.Enabled = true;
            }
            else
            {
                if (Program.mGroup == "CHINHANH")
                {
                    cmbQuyen.Items.Add("CHINHANH");
                    cmbQuyen.Items.Add("USER");
                    cmbQuyen.SelectedIndex = 0;
                }
                cmbChiNhanh.Enabled = false;
            }

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
                    MessageBox.Show("Lỗi kết nối về chi nhánh mới", "", MessageBoxButtons.OK);
                else
                {
                    this.NHANVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.NHANVIENTableAdapter.Fill(this.DS.NHANVIEN);
                    macn = ((DataRowView)bdsNHANVIEN[0])["MACN"].ToString();
                }
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            
            if (txtTenDN.Text == "")
            {
                MessageBox.Show("Vui lòng nhập username !!!", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (txtMatKhau.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu !!!", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (cmbMANV.Text == Program.username)
            {
                MessageBox.Show("Tài khoản này đang đăng nhập !!!", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            String strLenh = "dbo.SP_RP_KiemTraTaiKhoanDaDangKy";
            Program.sqlcmd = Program.conn.CreateCommand();
            Program.sqlcmd.CommandType = CommandType.StoredProcedure;
            Program.sqlcmd.CommandText = strLenh;
            Program.sqlcmd.Parameters.Add("@TENUSER", SqlDbType.NChar).Value = cmbMANV.Text.Trim();
            Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            Program.sqlcmd.ExecuteNonQuery();
            Program.conn.Close();
            String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
            if (Ret.Equals("1"))
            {
                MessageBox.Show("Tài khoản đã đăng ký!!!", "Thông báo");
                return;
            }
            if (Ret.Equals("-1"))
            {
                MessageBox.Show("Không có mã nhân viên!!!", "Thông báo");
                return;
            }


            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            String strLenh1 = "dbo.sp_TaoTaiKhoan";
            Program.sqlcmd = Program.conn.CreateCommand();
            Program.sqlcmd.CommandType = CommandType.StoredProcedure;
            Program.sqlcmd.CommandText = strLenh1;
            Program.sqlcmd.Parameters.Add("@LGNAME", SqlDbType.VarChar).Value = txtTenDN.Text.Trim();
            Program.sqlcmd.Parameters.Add("@PASS", SqlDbType.VarChar).Value = txtMatKhau.Text.Trim();
            Program.sqlcmd.Parameters.Add("@USERNAME", SqlDbType.Int).Value = cmbMANV.Text;
            Program.sqlcmd.Parameters.Add("@ROLE ", SqlDbType.VarChar).Value = cmbQuyen.Text;
            Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            Program.sqlcmd.ExecuteNonQuery();
            Program.conn.Close();
            String Ret1 = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
            if (Ret1.Equals("1"))
            {
                MessageBox.Show("LOGINNAME bị trùng!!!", "Thông báo");
                return;
            }
            else if (Ret1.Equals("2"))
            {
                MessageBox.Show("USERNAME bị trùng !!!", "Thông báo");
                return;
            }
            else
            {
                MessageBox.Show("Thêm thành công", "thông báo");
                txtTenDN.Text = "";
                txtMatKhau.Text = "";
                return;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
