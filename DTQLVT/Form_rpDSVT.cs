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
    public partial class Form_rpDSVT : Form
    {
        public Form_rpDSVT()
        {
            InitializeComponent();
        }

        private void FormInDSVT_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            String strLenh = "EXEC SP_RP_InDSVatTu";
            dt = Program.ExecSqlQuery(strLenh);
            rp_InDSVatTu rp = new rp_InDSVatTu();

            rp.SetDataSource(dt);
            crystalReportViewer1.ReportSource = rp;
        }
    }
}
