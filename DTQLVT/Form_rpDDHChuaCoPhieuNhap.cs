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
    public partial class Form_rpDDHChuaCoPhieuNhap : Form
    {
        public Form_rpDDHChuaCoPhieuNhap()
        {
            InitializeComponent();
        }

        private void Form_rpDDHChuaCoPhieuNhap_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            String strLenh = "EXEC SP_RP_InDSDDHChuaCoPhieuNhap";
            dt = Program.ExecSqlQuery(strLenh);
            rp_DSDDHChuaCoPhieuNhap rp = new rp_DSDDHChuaCoPhieuNhap();

            rp.SetDataSource(dt);
            crystalReportViewer1.ReportSource = rp;
        }
    }
}
