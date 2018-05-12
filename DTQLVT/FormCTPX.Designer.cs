namespace DTQLVT
{
    partial class FormCTPX
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label mAPXLabel;
            System.Windows.Forms.Label mAVTLabel;
            System.Windows.Forms.Label sOLUONGLabel;
            System.Windows.Forms.Label dONGIALabel;
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.btnThem = new DevExpress.XtraBars.BarButtonItem();
            this.btnSua = new DevExpress.XtraBars.BarButtonItem();
            this.btnGhi = new DevExpress.XtraBars.BarButtonItem();
            this.btnXoa = new DevExpress.XtraBars.BarButtonItem();
            this.btnReload = new DevExpress.XtraBars.BarButtonItem();
            this.btnThoat = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.dS = new DTQLVT.DS();
            this.tableAdapterManager = new DTQLVT.DSTableAdapters.TableAdapterManager();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDONGIA = new DevExpress.XtraEditors.SpinEdit();
            this.txtSOLUONG = new DevExpress.XtraEditors.SpinEdit();
            this.cmbMAVT = new System.Windows.Forms.ComboBox();
            this.txtMAPX = new DevExpress.XtraEditors.TextEdit();
            this.sPPhieuXuatVatTuTrongKhoBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.sPPhieuXuatVatTuTrongKhoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sPVatTuCoSoLuongTonLonHon0BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sP_VatTuCo_SoLuongTon_LonHon_0TableAdapter = new DTQLVT.DSTableAdapters.SP_VatTuCo_SoLuongTon_LonHon_0TableAdapter();
            this.sP_PhieuXuat_VatTu_TrongKhoTableAdapter = new DTQLVT.DSTableAdapters.SP_PhieuXuat_VatTu_TrongKhoTableAdapter();
            this.sP_DanhSachCTPhieuXuatBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sP_DanhSachCTPhieuXuatTableAdapter = new DTQLVT.DSTableAdapters.SP_DanhSachCTPhieuXuatTableAdapter();
            this.fillToolStrip = new System.Windows.Forms.ToolStrip();
            this.mAPXToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.mAPXToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.fillToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.gcCTPX = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colMAPX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMAVT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSOLUONG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDONGIA = new DevExpress.XtraGrid.Columns.GridColumn();
            mAPXLabel = new System.Windows.Forms.Label();
            mAVTLabel = new System.Windows.Forms.Label();
            sOLUONGLabel = new System.Windows.Forms.Label();
            dONGIALabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dS)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDONGIA.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSOLUONG.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMAPX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPPhieuXuatVatTuTrongKhoBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPPhieuXuatVatTuTrongKhoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPVatTuCoSoLuongTonLonHon0BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sP_DanhSachCTPhieuXuatBindingSource)).BeginInit();
            this.fillToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCTPX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // mAPXLabel
            // 
            mAPXLabel.AutoSize = true;
            mAPXLabel.Location = new System.Drawing.Point(59, 31);
            mAPXLabel.Name = "mAPXLabel";
            mAPXLabel.Size = new System.Drawing.Size(40, 13);
            mAPXLabel.TabIndex = 0;
            mAPXLabel.Text = "MAPX:";
            // 
            // mAVTLabel
            // 
            mAVTLabel.AutoSize = true;
            mAVTLabel.Location = new System.Drawing.Point(59, 89);
            mAVTLabel.Name = "mAVTLabel";
            mAVTLabel.Size = new System.Drawing.Size(40, 13);
            mAVTLabel.TabIndex = 2;
            mAVTLabel.Text = "MAVT:";
            // 
            // sOLUONGLabel
            // 
            sOLUONGLabel.AutoSize = true;
            sOLUONGLabel.Location = new System.Drawing.Point(419, 31);
            sOLUONGLabel.Name = "sOLUONGLabel";
            sOLUONGLabel.Size = new System.Drawing.Size(63, 13);
            sOLUONGLabel.TabIndex = 4;
            sOLUONGLabel.Text = "SOLUONG:";
            // 
            // dONGIALabel
            // 
            dONGIALabel.AutoSize = true;
            dONGIALabel.Location = new System.Drawing.Point(419, 97);
            dONGIALabel.Name = "dONGIALabel";
            dONGIALabel.Size = new System.Drawing.Size(52, 13);
            dONGIALabel.TabIndex = 6;
            dONGIALabel.Text = "DONGIA:";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnThem,
            this.btnSua,
            this.btnGhi,
            this.btnXoa,
            this.barButtonItem5,
            this.btnReload,
            this.btnThoat});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 7;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.FloatLocation = new System.Drawing.Point(210, 173);
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnThem, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnSua, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnGhi, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnXoa, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnReload, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnThoat, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // btnThem
            // 
            this.btnThem.Caption = "Thêm";
            this.btnThem.Glyph = global::DTQLVT.Properties.Resources.Button_Add_24_icon;
            this.btnThem.Id = 0;
            this.btnThem.Name = "btnThem";
            this.btnThem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnThem_ItemClick);
            // 
            // btnSua
            // 
            this.btnSua.Caption = "Sửa";
            this.btnSua.Glyph = global::DTQLVT.Properties.Resources.Text_Edit_icon_24;
            this.btnSua.Id = 1;
            this.btnSua.Name = "btnSua";
            // 
            // btnGhi
            // 
            this.btnGhi.Caption = "Ghi";
            this.btnGhi.Glyph = global::DTQLVT.Properties.Resources.Save_icon_24;
            this.btnGhi.Id = 2;
            this.btnGhi.Name = "btnGhi";
            this.btnGhi.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnGhi_ItemClick);
            // 
            // btnXoa
            // 
            this.btnXoa.Caption = "Xóa";
            this.btnXoa.Glyph = global::DTQLVT.Properties.Resources.Delete_2_icon_241;
            this.btnXoa.Id = 3;
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnXoa_ItemClick);
            // 
            // btnReload
            // 
            this.btnReload.Caption = "Reload";
            this.btnReload.Glyph = global::DTQLVT.Properties.Resources.Button_Refresh_icon_24;
            this.btnReload.Id = 5;
            this.btnReload.Name = "btnReload";
            this.btnReload.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReload_ItemClick);
            // 
            // btnThoat
            // 
            this.btnThoat.Caption = "Thoát";
            this.btnThoat.Glyph = global::DTQLVT.Properties.Resources.Exit_24_icon;
            this.btnThoat.Id = 6;
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnThoat_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(813, 32);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 718);
            this.barDockControlBottom.Size = new System.Drawing.Size(813, 23);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 32);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 686);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(813, 32);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 686);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "Phục Hồi";
            this.barButtonItem5.Glyph = global::DTQLVT.Properties.Resources.Undo_icon_24;
            this.barButtonItem5.Id = 4;
            this.barButtonItem5.Name = "barButtonItem5";
            // 
            // dS
            // 
            this.dS.DataSetName = "DS";
            this.dS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CHINHANHTableAdapter = null;
            this.tableAdapterManager.CTDDHTableAdapter = null;
            this.tableAdapterManager.CTPNTableAdapter = null;
            this.tableAdapterManager.DATHANGTableAdapter = null;
            this.tableAdapterManager.KHOTableAdapter = null;
            this.tableAdapterManager.NHANVIEN1TableAdapter = null;
            this.tableAdapterManager.NHANVIENTableAdapter = null;
            this.tableAdapterManager.PHIEUNHAPTableAdapter = null;
            this.tableAdapterManager.PHIEUXUATTableAdapter = null;
            this.tableAdapterManager.SP_DanhSachCTDDHTableAdapter = null;
            this.tableAdapterManager.SP_DanhSachCTPhieuNhapTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = DTQLVT.DSTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.VATTUTableAdapter = null;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(dONGIALabel);
            this.groupBox1.Controls.Add(this.txtDONGIA);
            this.groupBox1.Controls.Add(sOLUONGLabel);
            this.groupBox1.Controls.Add(this.txtSOLUONG);
            this.groupBox1.Controls.Add(mAVTLabel);
            this.groupBox1.Controls.Add(this.cmbMAVT);
            this.groupBox1.Controls.Add(mAPXLabel);
            this.groupBox1.Controls.Add(this.txtMAPX);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 280);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(813, 438);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // txtDONGIA
            // 
            this.txtDONGIA.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtDONGIA.Location = new System.Drawing.Point(525, 94);
            this.txtDONGIA.MenuManager = this.barManager1;
            this.txtDONGIA.Name = "txtDONGIA";
            this.txtDONGIA.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDONGIA.Size = new System.Drawing.Size(141, 20);
            this.txtDONGIA.TabIndex = 7;
            // 
            // txtSOLUONG
            // 
            this.txtSOLUONG.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtSOLUONG.Location = new System.Drawing.Point(525, 28);
            this.txtSOLUONG.MenuManager = this.barManager1;
            this.txtSOLUONG.Name = "txtSOLUONG";
            this.txtSOLUONG.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtSOLUONG.Properties.IsFloatValue = false;
            this.txtSOLUONG.Properties.Mask.EditMask = "N00";
            this.txtSOLUONG.Size = new System.Drawing.Size(141, 20);
            this.txtSOLUONG.TabIndex = 5;
            // 
            // cmbMAVT
            // 
            this.cmbMAVT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMAVT.FormattingEnabled = true;
            this.cmbMAVT.Location = new System.Drawing.Point(105, 89);
            this.cmbMAVT.Name = "cmbMAVT";
            this.cmbMAVT.Size = new System.Drawing.Size(135, 21);
            this.cmbMAVT.TabIndex = 3;
            // 
            // txtMAPX
            // 
            this.txtMAPX.Location = new System.Drawing.Point(105, 28);
            this.txtMAPX.MenuManager = this.barManager1;
            this.txtMAPX.Name = "txtMAPX";
            this.txtMAPX.Size = new System.Drawing.Size(135, 20);
            this.txtMAPX.TabIndex = 1;
            // 
            // sPPhieuXuatVatTuTrongKhoBindingSource1
            // 
            this.sPPhieuXuatVatTuTrongKhoBindingSource1.DataMember = "SP_PhieuXuat_VatTu_TrongKho";
            this.sPPhieuXuatVatTuTrongKhoBindingSource1.DataSource = this.dS;
            // 
            // sPPhieuXuatVatTuTrongKhoBindingSource
            // 
            this.sPPhieuXuatVatTuTrongKhoBindingSource.DataMember = "SP_PhieuXuat_VatTu_TrongKho";
            this.sPPhieuXuatVatTuTrongKhoBindingSource.DataSource = this.dS;
            // 
            // sPVatTuCoSoLuongTonLonHon0BindingSource
            // 
            this.sPVatTuCoSoLuongTonLonHon0BindingSource.DataMember = "SP_VatTuCo_SoLuongTon_LonHon_0";
            this.sPVatTuCoSoLuongTonLonHon0BindingSource.DataSource = this.dS;
            // 
            // sP_VatTuCo_SoLuongTon_LonHon_0TableAdapter
            // 
            this.sP_VatTuCo_SoLuongTon_LonHon_0TableAdapter.ClearBeforeFill = true;
            // 
            // sP_PhieuXuat_VatTu_TrongKhoTableAdapter
            // 
            this.sP_PhieuXuat_VatTu_TrongKhoTableAdapter.ClearBeforeFill = true;
            // 
            // sP_DanhSachCTPhieuXuatBindingSource
            // 
            this.sP_DanhSachCTPhieuXuatBindingSource.DataMember = "SP_DanhSachCTPhieuXuat";
            this.sP_DanhSachCTPhieuXuatBindingSource.DataSource = this.dS;
            // 
            // sP_DanhSachCTPhieuXuatTableAdapter
            // 
            this.sP_DanhSachCTPhieuXuatTableAdapter.ClearBeforeFill = true;
            // 
            // fillToolStrip
            // 
            this.fillToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mAPXToolStripLabel,
            this.mAPXToolStripTextBox,
            this.fillToolStripButton});
            this.fillToolStrip.Location = new System.Drawing.Point(0, 32);
            this.fillToolStrip.Name = "fillToolStrip";
            this.fillToolStrip.Size = new System.Drawing.Size(813, 25);
            this.fillToolStrip.TabIndex = 19;
            this.fillToolStrip.Text = "fillToolStrip";
            // 
            // mAPXToolStripLabel
            // 
            this.mAPXToolStripLabel.Name = "mAPXToolStripLabel";
            this.mAPXToolStripLabel.Size = new System.Drawing.Size(43, 22);
            this.mAPXToolStripLabel.Text = "MAPX:";
            // 
            // mAPXToolStripTextBox
            // 
            this.mAPXToolStripTextBox.Name = "mAPXToolStripTextBox";
            this.mAPXToolStripTextBox.Size = new System.Drawing.Size(100, 25);
            // 
            // fillToolStripButton
            // 
            this.fillToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fillToolStripButton.Name = "fillToolStripButton";
            this.fillToolStripButton.Size = new System.Drawing.Size(26, 22);
            this.fillToolStripButton.Text = "Fill";
            this.fillToolStripButton.Click += new System.EventHandler(this.fillToolStripButton_Click);
            // 
            // gcCTPX
            // 
            this.gcCTPX.DataSource = this.sP_DanhSachCTPhieuXuatBindingSource;
            this.gcCTPX.Dock = System.Windows.Forms.DockStyle.Top;
            this.gcCTPX.Location = new System.Drawing.Point(0, 57);
            this.gcCTPX.MainView = this.gridView1;
            this.gcCTPX.MenuManager = this.barManager1;
            this.gcCTPX.Name = "gcCTPX";
            this.gcCTPX.Size = new System.Drawing.Size(813, 220);
            this.gcCTPX.TabIndex = 19;
            this.gcCTPX.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMAPX,
            this.colMAVT,
            this.colSOLUONG,
            this.colDONGIA});
            this.gridView1.GridControl = this.gcCTPX;
            this.gridView1.Name = "gridView1";
            // 
            // colMAPX
            // 
            this.colMAPX.FieldName = "MAPX";
            this.colMAPX.Name = "colMAPX";
            this.colMAPX.Visible = true;
            this.colMAPX.VisibleIndex = 0;
            // 
            // colMAVT
            // 
            this.colMAVT.FieldName = "MAVT";
            this.colMAVT.Name = "colMAVT";
            this.colMAVT.Visible = true;
            this.colMAVT.VisibleIndex = 1;
            // 
            // colSOLUONG
            // 
            this.colSOLUONG.FieldName = "SOLUONG";
            this.colSOLUONG.Name = "colSOLUONG";
            this.colSOLUONG.Visible = true;
            this.colSOLUONG.VisibleIndex = 2;
            // 
            // colDONGIA
            // 
            this.colDONGIA.FieldName = "DONGIA";
            this.colDONGIA.Name = "colDONGIA";
            this.colDONGIA.Visible = true;
            this.colDONGIA.VisibleIndex = 3;
            // 
            // FormCTPX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(813, 741);
            this.Controls.Add(this.gcCTPX);
            this.Controls.Add(this.fillToolStrip);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FormCTPX";
            this.Text = "FormCTPX";
            this.Load += new System.EventHandler(this.FormCTPX_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dS)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDONGIA.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSOLUONG.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMAPX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPPhieuXuatVatTuTrongKhoBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPPhieuXuatVatTuTrongKhoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPVatTuCoSoLuongTonLonHon0BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sP_DanhSachCTPhieuXuatBindingSource)).EndInit();
            this.fillToolStrip.ResumeLayout(false);
            this.fillToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCTPX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarButtonItem btnThem;
        private DevExpress.XtraBars.BarButtonItem btnSua;
        private DevExpress.XtraBars.BarButtonItem btnGhi;
        private DevExpress.XtraBars.BarButtonItem btnXoa;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem btnReload;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnThoat;
        private DS dS;
        private DSTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SpinEdit txtDONGIA;
        private DevExpress.XtraEditors.SpinEdit txtSOLUONG;
        private System.Windows.Forms.ComboBox cmbMAVT;
        private DevExpress.XtraEditors.TextEdit txtMAPX;
        private System.Windows.Forms.BindingSource sPVatTuCoSoLuongTonLonHon0BindingSource;
        private DSTableAdapters.SP_VatTuCo_SoLuongTon_LonHon_0TableAdapter sP_VatTuCo_SoLuongTon_LonHon_0TableAdapter;
        private System.Windows.Forms.BindingSource sPPhieuXuatVatTuTrongKhoBindingSource;
        private DSTableAdapters.SP_PhieuXuat_VatTu_TrongKhoTableAdapter sP_PhieuXuat_VatTu_TrongKhoTableAdapter;
        private System.Windows.Forms.BindingSource sPPhieuXuatVatTuTrongKhoBindingSource1;
        private System.Windows.Forms.ToolStrip fillToolStrip;
        private System.Windows.Forms.ToolStripLabel mAPXToolStripLabel;
        private System.Windows.Forms.ToolStripTextBox mAPXToolStripTextBox;
        private System.Windows.Forms.ToolStripButton fillToolStripButton;
        private System.Windows.Forms.BindingSource sP_DanhSachCTPhieuXuatBindingSource;
        private DSTableAdapters.SP_DanhSachCTPhieuXuatTableAdapter sP_DanhSachCTPhieuXuatTableAdapter;
        private DevExpress.XtraGrid.GridControl gcCTPX;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colMAPX;
        private DevExpress.XtraGrid.Columns.GridColumn colMAVT;
        private DevExpress.XtraGrid.Columns.GridColumn colSOLUONG;
        private DevExpress.XtraGrid.Columns.GridColumn colDONGIA;
    }
}