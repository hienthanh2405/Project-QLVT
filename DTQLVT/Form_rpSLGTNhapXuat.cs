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
    public partial class Form_rpSLGTNhapXuat : Form
    {
        public Form_rpSLGTNhapXuat()
        {
            InitializeComponent();
        }

        private void Form_rpSLGTNhapXuat_Load(object sender, EventArgs e)
        {

        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            String strLenh = "EXEC SP_RP_In_SLTG_Nhap_Xuat '" + Program.mGroup + "','"
                + convertStringToDateTime(dateNgayBatDau.Text.Trim()).ToString("yyyy-MM-dd") + "','"
                + convertStringToDateTime(dateNgayKetThuc.Text.Trim()).ToString("yyyy-MM-dd") + "'";
            MessageBox.Show(strLenh);
            dt = Program.ExecSqlQuery(strLenh);
            rp_ChiTietSLGT_NhapXuat rp = new rp_ChiTietSLGT_NhapXuat();

            rp.SetDataSource(dt);
            crystalReportViewer1.ReportSource = rp;
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
