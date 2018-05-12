using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DTQLVT
{
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FormMain()
        {
            InitializeComponent();
            btnDangNhap.PerformClick();
        }

        public Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        private void btnDangNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormDangNhap));
            if (frm != null) frm.Activate();
            else
            {
                FormDangNhap f = new FormDangNhap();
                f.MdiParent = this;
                f.Show();
            }

        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        public void HienThiMenu()
        {
            MANV.Text = "Mã NV : " + Program.username;
            HOTEN.Text = "Họ tên nhân viên : " + Program.mHoten;
            NHOM.Text = "Nhóm : " + Program.mGroup;
            // Phân quyền
        }

        private void btnNhanVien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(formNhanVien));
            if (frm != null) frm.Activate();
            else
            {
                formNhanVien f = new formNhanVien();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnKho_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormKho));
            if (frm != null) frm.Activate();
            else
            {
                FormKho f = new FormKho();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnVatTu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormVatTu));
            if (frm != null) frm.Activate();
            else
            {
                FormVatTu f = new FormVatTu();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnDatHang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormDonDatHang));
            if (frm != null) frm.Activate();
            else
            {
                FormDonDatHang f = new FormDonDatHang();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPhieuNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormPhieuNhap));
            if (frm != null) frm.Activate();
            else
            {
                FormPhieuNhap f = new FormPhieuNhap();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPhieuXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormPhieuXuat));
            if (frm != null) frm.Activate();
            else
            {
                FormPhieuXuat f = new FormPhieuXuat();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnDangKy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormTaoTaiKhoan));
            if (frm != null) frm.Activate();
            else
            {
                FormTaoTaiKhoan f = new FormTaoTaiKhoan();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if(Program.mGroup == "")
            {
                btnDangKy.Enabled = false;
            }
        }

        private void btnSLGTNhapXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form_rpSLGTNhapXuat fm = new Form_rpSLGTNhapXuat();
            fm.ShowDialog();
        }

        private void btnDDH_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form_rpDDHChuaCoPhieuNhap fm = new Form_rpDDHChuaCoPhieuNhap();
            fm.ShowDialog();
        }

        private void btnHoatDongNV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form_rpHoatDongNhanVien fm = new Form_rpHoatDongNhanVien();
            fm.ShowDialog();
        }

        private void btnTongHopNX_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form_rpTongHopNhapXuat fm = new Form_rpTongHopNhapXuat();
            fm.ShowDialog();
        }

        
    }
}
