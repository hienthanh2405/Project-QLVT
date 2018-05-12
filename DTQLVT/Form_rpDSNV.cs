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
    public partial class Form_rpDSNV : Form
    {
        public Form_rpDSNV()
        {
            InitializeComponent();
        }

        private void FormDSNV_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            String strLenh = "EXEC SP_RP_InDS_NhanVien";
            //MessageBox.Show(strLenh);
            dt = Program.ExecSqlQuery(strLenh);
            rp_DSNhanVien rp = new rp_DSNhanVien();

            rp.SetDataSource(dt);
            crystalReportViewer1.ReportSource = rp;
        }
    }
}
