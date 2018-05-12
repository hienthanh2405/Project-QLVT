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
    public partial class Form_rpHoatDongNhanVien : Form
    {
        String macn = "";

        public Form_rpHoatDongNhanVien()
        {
            InitializeComponent();
        }

        private void Form_rpHoatDongNhanVien_Load(object sender, EventArgs e)
        {
           
            DS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.NHANVIEN1' table. You can move, or remove it, as needed.
            this.NHANVIEN1TableAdapter.Connection.ConnectionString = Program.connstr;
            this.NHANVIEN1TableAdapter.Fill(this.DS.NHANVIEN1);

            macn = ((DataRowView)bdsNHANVIEN1[0])["MACN"].ToString();
            //đưa dữ liệu từ datatable về table NHANVIEN 
            this.NHANVIEN1TableAdapter.Update(this.DS.NHANVIEN1);

            cmbChiNhanh.DataSource = Program.bds_dspm;
            cmbChiNhanh.DisplayMember = "TenCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;
            //gioi han chi co group CONG TY moi co quyen 
            if (Program.mGroup == "CONGTY")
            {
                cmbChiNhanh.Enabled = true;
            }
            else if (Program.mGroup == "CHINHANH")
            {
                cmbChiNhanh.Enabled = false;
            }
            txtHoTen.Enabled = false;

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
                    this.NHANVIEN1TableAdapter.Connection.ConnectionString = Program.connstr;
                    this.NHANVIEN1TableAdapter.Fill(this.DS.NHANVIEN1);
                    macn = ((DataRowView)bdsNHANVIEN1[0])["MACN"].ToString();
                }
            }
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            String strLenh = "dbo.[SP_RP_KiemTraHoatDongNV]";
            Program.sqlcmd = Program.conn.CreateCommand();
            Program.sqlcmd.CommandType = CommandType.StoredProcedure;
            Program.sqlcmd.CommandText = strLenh;
            Program.sqlcmd.Parameters.Add("@MANV", SqlDbType.Int).Value = cmbMANV.Text;
            Program.sqlcmd.Parameters.Add("@STARTTIME", SqlDbType.Date).Value = convertStringToDateTime(dateNgayBatDau.Text.Trim());
            Program.sqlcmd.Parameters.Add("@LASTTIME", SqlDbType.Date).Value = convertStringToDateTime(dateNgayKetThuc.Text.Trim());
            Program.sqlcmd.Parameters.Add("@Ret", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            Program.sqlcmd.ExecuteNonQuery();
            Program.conn.Close();
            String Ret = Program.sqlcmd.Parameters["@Ret"].Value.ToString();
            if (Ret.Equals("0"))
            {
                MessageBox.Show("Nhân viên chưa lập phiếu trong khoảng thời gian này!", "", MessageBoxButtons.OK);
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                String strLenh1 = "EXEC [SP_RP_InHoatDongNV] '" + cmbMANV.Text + "','"
                    + convertStringToDateTime(dateNgayBatDau.Text.Trim()).ToString("yyyy-MM-dd") + "','"
                    + convertStringToDateTime(dateNgayKetThuc.Text.Trim()).ToString("yyyy-MM-dd") + "'";
                MessageBox.Show(strLenh1);
                dt = Program.ExecSqlQuery(strLenh1);
                rp_HoatDongNhanVien rp = new rp_HoatDongNhanVien();

                rp.SetDataSource(dt);
                crystalReportViewer1.ReportSource = rp;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static DateTime convertStringToDateTime(String s)
        {
            DateTime date = DateTime.Now;
            try
            {
                date = DateTime.ParseExact(s, "dd/MM//yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                try
                {

                    date = DateTime.ParseExact(s, "d/MM/yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex1)
                {
                    try
                    {
                        date = DateTime.ParseExact(s, "d/M/yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex2)
                    {
                        try
                        {
                            date = DateTime.ParseExact(s, "dd/M/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex3)
                        {
                            MessageBox.Show("Cannot convert String to date time", "", MessageBoxButtons.OK);
                        }
                    }

                }

            }
            return date;
        }
       
    }
}
