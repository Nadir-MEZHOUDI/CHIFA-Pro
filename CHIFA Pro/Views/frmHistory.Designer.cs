namespace CHIFA.Pro.Views
{
    partial class FrmHistory
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHistory));
            gridHistory = new DevExpress.XtraGrid.GridControl();
            factureDtoBindingSource = new BindingSource(components);
            gridHistFactures = new DevExpress.XtraGrid.Views.Grid.GridView();
            colNumFact = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateFact = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateSoin = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontFact = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontAss = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontOff = new DevExpress.XtraGrid.Columns.GridColumn();
            colMajoration = new DevExpress.XtraGrid.Columns.GridColumn();
            colSpecialite = new DevExpress.XtraGrid.Columns.GridColumn();
            colBordereau = new DevExpress.XtraGrid.Columns.GridColumn();
            dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            NumAssureTextEdit = new TextEdit();
            BeneficiareBindingSource = new BindingSource(components);
            AssureNameTextEdit = new TextEdit();
            MaladeTextEdit = new TextEdit();
            RangTextEdit = new TextEdit();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            footerDataControl = new DevExpress.XtraLayout.LayoutControlGroup();
            ItemForNumAssure = new DevExpress.XtraLayout.LayoutControlItem();
            ItemForAssureName = new DevExpress.XtraLayout.LayoutControlItem();
            ItemForMalade = new DevExpress.XtraLayout.LayoutControlItem();
            ItemForRang = new DevExpress.XtraLayout.LayoutControlItem();
            patientDtoBindingSource = new BindingSource(components);
            gridDetails2 = new DevExpress.XtraGrid.GridControl();
            factureDetailDtoBindingSource = new BindingSource(components);
            gridHistDetails = new DevExpress.XtraGrid.Views.Grid.GridView();
            colCode = new DevExpress.XtraGrid.Columns.GridColumn();
            colMedicament = new DevExpress.XtraGrid.Columns.GridColumn();
            colQt = new DevExpress.XtraGrid.Columns.GridColumn();
            colTR = new DevExpress.XtraGrid.Columns.GridColumn();
            colDureeTrait = new DevExpress.XtraGrid.Columns.GridColumn();
            colPpa = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontAss1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontPharm = new DevExpress.XtraGrid.Columns.GridColumn();
            xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            tabConsumption = new DevExpress.XtraTab.XtraTabPage();
            gridConsumption = new DevExpress.XtraGrid.GridControl();
            consumptionDtoBindingSource = new BindingSource(components);
            gridConsomption = new DevExpress.XtraGrid.Views.Grid.GridView();
            colDuree = new DevExpress.XtraGrid.Columns.GridColumn();
            colDate = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMédecin = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            colProchain = new DevExpress.XtraGrid.Columns.GridColumn();
            colTS = new DevExpress.XtraGrid.Columns.GridColumn();
            colNEnrg = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            tabHistory = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)gridHistory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)factureDtoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridHistFactures).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataLayoutControl1).BeginInit();
            dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumAssureTextEdit.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BeneficiareBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AssureNameTextEdit.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MaladeTextEdit.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RangTextEdit.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)footerDataControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ItemForNumAssure).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ItemForAssureName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ItemForMalade).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ItemForRang).BeginInit();
            ((System.ComponentModel.ISupportInitialize)patientDtoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridDetails2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)factureDetailDtoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridHistDetails).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabControl1).BeginInit();
            xtraTabControl1.SuspendLayout();
            tabConsumption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridConsumption).BeginInit();
            ((System.ComponentModel.ISupportInitialize)consumptionDtoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridConsomption).BeginInit();
            tabHistory.SuspendLayout();
            SuspendLayout();
            // 
            // gridHistory
            // 
            gridHistory.DataSource = factureDtoBindingSource;
            gridHistory.Dock = DockStyle.Fill;
            gridHistory.Location = new Point(0, 0);
            gridHistory.MainView = gridHistFactures;
            gridHistory.Name = "gridHistory";
            gridHistory.Size = new Size(1306, 314);
            gridHistory.TabIndex = 1;
            gridHistory.ViewCollection.AddRange(new BaseView[] { gridHistFactures });
            // 
            // factureDtoBindingSource
            // 
            factureDtoBindingSource.DataSource = typeof(FactureDto);
            // 
            // gridHistFactures
            // 
            gridHistFactures.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colNumFact, colDateFact, colDateSoin, colMontFact, colMontAss, colMontOff, colMajoration, colSpecialite, colBordereau });
            gridHistFactures.DetailHeight = 382;
            gridHistFactures.GridControl = gridHistory;
            gridHistFactures.Name = "gridHistFactures";
            gridHistFactures.OptionsView.ShowFooter = true;
            gridHistFactures.CustomDrawCell += gridHistory_CustomDrawCell;
            gridHistFactures.FocusedRowChanged += gridHistory_FocusedRowChanged;
            // 
            // colNumFact
            // 
            colNumFact.FieldName = "NumFact";
            colNumFact.MinWidth = 27;
            colNumFact.Name = "colNumFact";
            colNumFact.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "NumFact", "{0}") });
            colNumFact.Visible = true;
            colNumFact.VisibleIndex = 1;
            colNumFact.Width = 104;
            // 
            // colDateFact
            // 
            colDateFact.DisplayFormat.FormatString = "dd/MM/yyyy";
            colDateFact.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            colDateFact.FieldName = "DateFact";
            colDateFact.MinWidth = 27;
            colDateFact.Name = "colDateFact";
            colDateFact.Visible = true;
            colDateFact.VisibleIndex = 2;
            colDateFact.Width = 104;
            // 
            // colDateSoin
            // 
            colDateSoin.DisplayFormat.FormatString = "dd/MM/yyyy";
            colDateSoin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            colDateSoin.FieldName = "DateSoin";
            colDateSoin.MinWidth = 27;
            colDateSoin.Name = "colDateSoin";
            colDateSoin.Visible = true;
            colDateSoin.VisibleIndex = 3;
            colDateSoin.Width = 104;
            // 
            // colMontFact
            // 
            colMontFact.FieldName = "MontFact";
            colMontFact.MinWidth = 27;
            colMontFact.Name = "colMontFact";
            colMontFact.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MontFact", "{0:N2}") });
            colMontFact.Visible = true;
            colMontFact.VisibleIndex = 4;
            colMontFact.Width = 104;
            // 
            // colMontAss
            // 
            colMontAss.FieldName = "MontAss";
            colMontAss.MinWidth = 27;
            colMontAss.Name = "colMontAss";
            colMontAss.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, DevExpress.Data.SummaryMode.Mixed, "MontAss", "{0:N2}") });
            colMontAss.Visible = true;
            colMontAss.VisibleIndex = 5;
            colMontAss.Width = 104;
            // 
            // colMontOff
            // 
            colMontOff.FieldName = "MontOff";
            colMontOff.MinWidth = 27;
            colMontOff.Name = "colMontOff";
            colMontOff.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MontOff", "{0:N2}") });
            colMontOff.Visible = true;
            colMontOff.VisibleIndex = 6;
            colMontOff.Width = 104;
            // 
            // colMajoration
            // 
            colMajoration.FieldName = "Majoration";
            colMajoration.MinWidth = 27;
            colMajoration.Name = "colMajoration";
            colMajoration.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, DevExpress.Data.SummaryMode.Mixed, "Majoration", "{0:N2}") });
            colMajoration.Visible = true;
            colMajoration.VisibleIndex = 7;
            colMajoration.Width = 104;
            // 
            // colSpecialite
            // 
            colSpecialite.FieldName = "Specialite";
            colSpecialite.MinWidth = 27;
            colSpecialite.Name = "colSpecialite";
            colSpecialite.Visible = true;
            colSpecialite.VisibleIndex = 8;
            colSpecialite.Width = 104;
            // 
            // colBordereau
            // 
            colBordereau.FieldName = "Bordereau";
            colBordereau.MinWidth = 27;
            colBordereau.Name = "colBordereau";
            colBordereau.Visible = true;
            colBordereau.VisibleIndex = 0;
            colBordereau.Width = 104;
            // 
            // dataLayoutControl1
            // 
            dataLayoutControl1.Controls.Add(NumAssureTextEdit);
            dataLayoutControl1.Controls.Add(AssureNameTextEdit);
            dataLayoutControl1.Controls.Add(MaladeTextEdit);
            dataLayoutControl1.Controls.Add(RangTextEdit);
            dataLayoutControl1.DataSource = BeneficiareBindingSource;
            dataLayoutControl1.Dock = DockStyle.Bottom;
            dataLayoutControl1.Location = new Point(0, 557);
            dataLayoutControl1.Name = "dataLayoutControl1";
            dataLayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new Rectangle(-9, -9, 1938, 1098);
            dataLayoutControl1.Root = Root;
            dataLayoutControl1.Size = new Size(1312, 82);
            dataLayoutControl1.TabIndex = 0;
            dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // NumAssureTextEdit
            // 
            NumAssureTextEdit.DataBindings.Add(new Binding("EditValue", BeneficiareBindingSource, "NumAssure", true));
            NumAssureTextEdit.Location = new Point(126, 2);
            NumAssureTextEdit.Name = "NumAssureTextEdit";
            NumAssureTextEdit.Properties.ReadOnly = true;
            NumAssureTextEdit.Properties.UseReadOnlyAppearance = false;
            NumAssureTextEdit.Size = new Size(528, 24);
            NumAssureTextEdit.StyleController = dataLayoutControl1;
            NumAssureTextEdit.TabIndex = 4;
            // 
            // BeneficiareBindingSource
            // 
            BeneficiareBindingSource.DataSource = typeof(BeneficiareDto);
            // 
            // AssureNameTextEdit
            // 
            AssureNameTextEdit.DataBindings.Add(new Binding("EditValue", BeneficiareBindingSource, "Assure", true));
            AssureNameTextEdit.Location = new Point(782, 2);
            AssureNameTextEdit.Name = "AssureNameTextEdit";
            AssureNameTextEdit.Properties.ReadOnly = true;
            AssureNameTextEdit.Properties.UseReadOnlyAppearance = false;
            AssureNameTextEdit.Size = new Size(528, 24);
            AssureNameTextEdit.StyleController = dataLayoutControl1;
            AssureNameTextEdit.TabIndex = 5;
            // 
            // MaladeTextEdit
            // 
            MaladeTextEdit.DataBindings.Add(new Binding("EditValue", BeneficiareBindingSource, "Beneficiare", true));
            MaladeTextEdit.Location = new Point(126, 30);
            MaladeTextEdit.Name = "MaladeTextEdit";
            MaladeTextEdit.Properties.ReadOnly = true;
            MaladeTextEdit.Properties.UseReadOnlyAppearance = false;
            MaladeTextEdit.Size = new Size(528, 24);
            MaladeTextEdit.StyleController = dataLayoutControl1;
            MaladeTextEdit.TabIndex = 6;
            // 
            // RangTextEdit
            // 
            RangTextEdit.DataBindings.Add(new Binding("EditValue", BeneficiareBindingSource, "Rang", true));
            RangTextEdit.Location = new Point(782, 30);
            RangTextEdit.Name = "RangTextEdit";
            RangTextEdit.Properties.ReadOnly = true;
            RangTextEdit.Properties.UseReadOnlyAppearance = false;
            RangTextEdit.Size = new Size(528, 24);
            RangTextEdit.StyleController = dataLayoutControl1;
            RangTextEdit.TabIndex = 7;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { footerDataControl });
            Root.Name = "Root";
            Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            Root.Size = new Size(1312, 82);
            Root.TextVisible = false;
            // 
            // footerDataControl
            // 
            footerDataControl.AllowDrawBackground = false;
            footerDataControl.GroupBordersVisible = false;
            footerDataControl.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { ItemForNumAssure, ItemForAssureName, ItemForMalade, ItemForRang });
            footerDataControl.Location = new Point(0, 0);
            footerDataControl.Name = "footerDataControl";
            footerDataControl.Size = new Size(1312, 82);
            // 
            // ItemForNumAssure
            // 
            ItemForNumAssure.Control = NumAssureTextEdit;
            ItemForNumAssure.Location = new Point(0, 0);
            ItemForNumAssure.Name = "ItemForNumAssure";
            ItemForNumAssure.Size = new Size(656, 28);
            ItemForNumAssure.Text = "N° Assuré";
            ItemForNumAssure.TextSize = new Size(120, 17);
            // 
            // ItemForAssureName
            // 
            ItemForAssureName.Control = AssureNameTextEdit;
            ItemForAssureName.Location = new Point(656, 0);
            ItemForAssureName.Name = "ItemForAssureName";
            ItemForAssureName.Size = new Size(656, 28);
            ItemForAssureName.Text = "Nom de l'assuré";
            ItemForAssureName.TextSize = new Size(120, 17);
            // 
            // ItemForMalade
            // 
            ItemForMalade.Control = MaladeTextEdit;
            ItemForMalade.Location = new Point(0, 28);
            ItemForMalade.Name = "ItemForMalade";
            ItemForMalade.Size = new Size(656, 54);
            ItemForMalade.Text = "Malade / Bénéficiaire";
            ItemForMalade.TextSize = new Size(120, 17);
            // 
            // ItemForRang
            // 
            ItemForRang.Control = RangTextEdit;
            ItemForRang.Location = new Point(656, 28);
            ItemForRang.Name = "ItemForRang";
            ItemForRang.Size = new Size(656, 54);
            ItemForRang.Text = "Rang";
            ItemForRang.TextSize = new Size(120, 17);
            // 
            // gridDetails2
            // 
            gridDetails2.DataSource = factureDetailDtoBindingSource;
            gridDetails2.Dock = DockStyle.Bottom;
            gridDetails2.Location = new Point(0, 314);
            gridDetails2.MainView = gridHistDetails;
            gridDetails2.Name = "gridDetails2";
            gridDetails2.Size = new Size(1306, 196);
            gridDetails2.TabIndex = 0;
            gridDetails2.ViewCollection.AddRange(new BaseView[] { gridHistDetails });
            // 
            // factureDetailDtoBindingSource
            // 
            factureDetailDtoBindingSource.DataSource = typeof(FactureDetailDto);
            // 
            // gridHistDetails
            // 
            gridHistDetails.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colCode, colMedicament, colQt, colTR, colDureeTrait, colPpa, colMontAss1, colMontPharm });
            gridHistDetails.DetailHeight = 382;
            gridHistDetails.GridControl = gridDetails2;
            gridHistDetails.Name = "gridHistDetails";
            gridHistDetails.OptionsView.ShowFooter = true;
            gridHistDetails.OptionsView.ShowGroupPanel = false;
            gridHistDetails.CustomDrawCell += gridDetails_CustomDrawCell;
            // 
            // colCode
            // 
            colCode.FieldName = "Code";
            colCode.MinWidth = 27;
            colCode.Name = "colCode";
            colCode.Visible = true;
            colCode.VisibleIndex = 0;
            colCode.Width = 104;
            // 
            // colMedicament
            // 
            colMedicament.FieldName = "Medicament";
            colMedicament.MinWidth = 27;
            colMedicament.Name = "colMedicament";
            colMedicament.Visible = true;
            colMedicament.VisibleIndex = 1;
            colMedicament.Width = 104;
            // 
            // colQt
            // 
            colQt.FieldName = "Qt";
            colQt.MinWidth = 27;
            colQt.Name = "colQt";
            colQt.Visible = true;
            colQt.VisibleIndex = 2;
            colQt.Width = 104;
            // 
            // colTR
            // 
            colTR.FieldName = "TR";
            colTR.MinWidth = 27;
            colTR.Name = "colTR";
            colTR.Visible = true;
            colTR.VisibleIndex = 5;
            colTR.Width = 104;
            // 
            // colDureeTrait
            // 
            colDureeTrait.FieldName = "DureeTrait";
            colDureeTrait.MinWidth = 27;
            colDureeTrait.Name = "colDureeTrait";
            colDureeTrait.Visible = true;
            colDureeTrait.VisibleIndex = 3;
            colDureeTrait.Width = 104;
            // 
            // colPpa
            // 
            colPpa.FieldName = "Ppa";
            colPpa.MinWidth = 27;
            colPpa.Name = "colPpa";
            colPpa.Visible = true;
            colPpa.VisibleIndex = 4;
            colPpa.Width = 104;
            // 
            // colMontAss1
            // 
            colMontAss1.FieldName = "MontAss";
            colMontAss1.MinWidth = 27;
            colMontAss1.Name = "colMontAss1";
            colMontAss1.Visible = true;
            colMontAss1.VisibleIndex = 6;
            colMontAss1.Width = 104;
            // 
            // colMontPharm
            // 
            colMontPharm.FieldName = "MontPharm";
            colMontPharm.MinWidth = 27;
            colMontPharm.Name = "colMontPharm";
            colMontPharm.Visible = true;
            colMontPharm.VisibleIndex = 7;
            colMontPharm.Width = 104;
            // 
            // xtraTabControl1
            // 
            xtraTabControl1.CustomHeaderButtons.AddRange(new DevExpress.XtraTab.Buttons.CustomHeaderButton[] { new DevExpress.XtraTab.Buttons.CustomHeaderButton() });
            xtraTabControl1.Dock = DockStyle.Fill;
            xtraTabControl1.Location = new Point(0, 0);
            xtraTabControl1.Name = "xtraTabControl1";
            xtraTabControl1.SelectedTabPage = tabConsumption;
            xtraTabControl1.Size = new Size(1312, 557);
            xtraTabControl1.TabIndex = 1;
            xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] { tabConsumption, tabHistory });
            xtraTabControl1.CustomHeaderButtonClick += xtraTabControl1_CustomHeaderButtonClick;
            // 
            // tabConsumption
            // 
            tabConsumption.Controls.Add(gridConsumption);
            tabConsumption.ImageOptions.Image = (Image)resources.GetObject("tabConsumption.ImageOptions.Image");
            tabConsumption.Name = "tabConsumption";
            tabConsumption.Size = new Size(1306, 510);
            tabConsumption.Text = "Consumption";
            // 
            // gridConsumption
            // 
            gridConsumption.DataSource = consumptionDtoBindingSource;
            gridConsumption.Dock = DockStyle.Fill;
            gridConsumption.Location = new Point(0, 0);
            gridConsumption.MainView = gridConsomption;
            gridConsumption.Name = "gridConsumption";
            gridConsumption.Size = new Size(1306, 510);
            gridConsumption.TabIndex = 2;
            gridConsumption.ViewCollection.AddRange(new BaseView[] { gridConsomption });
            // 
            // consumptionDtoBindingSource
            // 
            consumptionDtoBindingSource.DataSource = typeof(ConsumptionDto);
            // 
            // gridConsomption
            // 
            gridConsomption.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colDuree, colDate, gridColumn1, gridColumn2, colMédecin, gridColumn3, colProchain, colTS, colNEnrg, gridColumn4, gridColumn5 });
            gridConsomption.CustomizationFormBounds = new Rectangle(1064, 476, 324, 366);
            gridConsomption.DetailHeight = 364;
            gridConsomption.GridControl = gridConsumption;
            gridConsomption.Name = "gridConsomption";
            gridConsomption.OptionsView.ShowFooter = true;
            gridConsomption.CustomDrawCell += gridConsumption_CustomDrawCell;
            // 
            // colDuree
            // 
            colDuree.FieldName = "Duree";
            colDuree.MinWidth = 24;
            colDuree.Name = "colDuree";
            colDuree.Visible = true;
            colDuree.VisibleIndex = 3;
            colDuree.Width = 143;
            // 
            // colDate
            // 
            colDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            colDate.FieldName = "Date";
            colDate.MinWidth = 24;
            colDate.Name = "colDate";
            colDate.Visible = true;
            colDate.VisibleIndex = 4;
            colDate.Width = 107;
            // 
            // gridColumn1
            // 
            gridColumn1.FieldName = "Qt";
            gridColumn1.MinWidth = 24;
            gridColumn1.Name = "gridColumn1";
            gridColumn1.Visible = true;
            gridColumn1.VisibleIndex = 5;
            gridColumn1.Width = 63;
            // 
            // gridColumn2
            // 
            gridColumn2.FieldName = "Medicament";
            gridColumn2.MinWidth = 24;
            gridColumn2.Name = "gridColumn2";
            gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count) });
            gridColumn2.Visible = true;
            gridColumn2.VisibleIndex = 2;
            gridColumn2.Width = 426;
            // 
            // colMédecin
            // 
            colMédecin.FieldName = "Médecin";
            colMédecin.MinWidth = 24;
            colMédecin.Name = "colMédecin";
            colMédecin.Visible = true;
            colMédecin.VisibleIndex = 6;
            colMédecin.Width = 137;
            // 
            // gridColumn3
            // 
            gridColumn3.FieldName = "Prix";
            gridColumn3.MinWidth = 24;
            gridColumn3.Name = "gridColumn3";
            gridColumn3.Visible = true;
            gridColumn3.VisibleIndex = 7;
            gridColumn3.Width = 93;
            // 
            // colProchain
            // 
            colProchain.DisplayFormat.FormatString = "dd/MM/yyyy";
            colProchain.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            colProchain.FieldName = "Prochain";
            colProchain.MinWidth = 24;
            colProchain.Name = "colProchain";
            colProchain.OptionsColumn.ReadOnly = true;
            colProchain.Visible = true;
            colProchain.VisibleIndex = 8;
            colProchain.Width = 322;
            // 
            // colTS
            // 
            colTS.FieldName = "TS";
            colTS.MinWidth = 24;
            colTS.Name = "colTS";
            colTS.Width = 93;
            // 
            // colNEnrg
            // 
            colNEnrg.FieldName = "NEnrg";
            colNEnrg.MinWidth = 24;
            colNEnrg.Name = "colNEnrg";
            colNEnrg.Width = 93;
            // 
            // gridColumn4
            // 
            gridColumn4.FieldName = "Facture";
            gridColumn4.Name = "gridColumn4";
            gridColumn4.Visible = true;
            gridColumn4.VisibleIndex = 1;
            // 
            // gridColumn5
            // 
            gridColumn5.FieldName = "Bord";
            gridColumn5.Name = "gridColumn5";
            gridColumn5.Visible = true;
            gridColumn5.VisibleIndex = 0;
            // 
            // tabHistory
            // 
            tabHistory.Controls.Add(gridHistory);
            tabHistory.Controls.Add(gridDetails2);
            tabHistory.ImageOptions.Image = (Image)resources.GetObject("tabHistory.ImageOptions.Image");
            tabHistory.Name = "tabHistory";
            tabHistory.Size = new Size(1306, 510);
            tabHistory.Text = "Historique";
            // 
            // FrmHistory
            // 
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1312, 639);
            Controls.Add(xtraTabControl1);
            Controls.Add(dataLayoutControl1);
            Font = new Font("Tahoma", 12F);
            IconOptions.LargeImage = (Image)resources.GetObject("FrmHistory.IconOptions.LargeImage");
            Margin = new Padding(5, 4, 5, 4);
            Name = "FrmHistory";
            Text = "Patient History";
            WindowState = FormWindowState.Maximized;
            Load += FrmHistory_Load;
            ((System.ComponentModel.ISupportInitialize)gridHistory).EndInit();
            ((System.ComponentModel.ISupportInitialize)factureDtoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridHistFactures).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataLayoutControl1).EndInit();
            dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)NumAssureTextEdit.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)BeneficiareBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)AssureNameTextEdit.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)MaladeTextEdit.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)RangTextEdit.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)footerDataControl).EndInit();
            ((System.ComponentModel.ISupportInitialize)ItemForNumAssure).EndInit();
            ((System.ComponentModel.ISupportInitialize)ItemForAssureName).EndInit();
            ((System.ComponentModel.ISupportInitialize)ItemForMalade).EndInit();
            ((System.ComponentModel.ISupportInitialize)ItemForRang).EndInit();
            ((System.ComponentModel.ISupportInitialize)patientDtoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridDetails2).EndInit();
            ((System.ComponentModel.ISupportInitialize)factureDetailDtoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridHistDetails).EndInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabControl1).EndInit();
            xtraTabControl1.ResumeLayout(false);
            tabConsumption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridConsumption).EndInit();
            ((System.ComponentModel.ISupportInitialize)consumptionDtoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridConsomption).EndInit();
            tabHistory.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private DevExpress.XtraGrid.GridControl gridHistory;
        private DevExpress.XtraGrid.Views.Grid.GridView gridHistFactures;
        private DevExpress.XtraGrid.GridControl gridDetails2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridHistDetails;
        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private BindingSource BeneficiareBindingSource;
        private BindingSource factureDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colNumFact;
        private DevExpress.XtraGrid.Columns.GridColumn colDateFact;
        private DevExpress.XtraGrid.Columns.GridColumn colDateSoin;
        private DevExpress.XtraGrid.Columns.GridColumn colMontFact;
        private DevExpress.XtraGrid.Columns.GridColumn colMontAss;
        private DevExpress.XtraGrid.Columns.GridColumn colMontOff;
        private DevExpress.XtraGrid.Columns.GridColumn colMajoration;
        private DevExpress.XtraGrid.Columns.GridColumn colSpecialite;
        private DevExpress.XtraGrid.Columns.GridColumn colBordereau;
        private BindingSource factureDetailDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colCode;
        private DevExpress.XtraGrid.Columns.GridColumn colMedicament;
        private DevExpress.XtraGrid.Columns.GridColumn colQt;
        private DevExpress.XtraGrid.Columns.GridColumn colTR;
        private DevExpress.XtraGrid.Columns.GridColumn colDureeTrait;
        private DevExpress.XtraGrid.Columns.GridColumn colPpa;
        private DevExpress.XtraGrid.Columns.GridColumn colMontAss1;
        private DevExpress.XtraGrid.Columns.GridColumn colMontPharm;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage tabHistory;
        private DevExpress.XtraTab.XtraTabPage tabConsumption;
        private DevExpress.XtraGrid.GridControl gridConsumption;
        private DevExpress.XtraGrid.Views.Grid.GridView gridConsomption;
        private DevExpress.XtraGrid.Columns.GridColumn colDuree;
        private DevExpress.XtraGrid.Columns.GridColumn colDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn colMédecin;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn colProchain;
        private DevExpress.XtraGrid.Columns.GridColumn colTS;
        private DevExpress.XtraGrid.Columns.GridColumn colNEnrg;
        private DevExpress.XtraLayout.LayoutControlGroup footerDataControl;
        private BindingSource patientDtoBindingSource;
        private TextEdit NumAssureTextEdit;
        private TextEdit AssureNameTextEdit;
        private TextEdit MaladeTextEdit;
        private TextEdit RangTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem ItemForNumAssure;
        private DevExpress.XtraLayout.LayoutControlItem ItemForAssureName;
        private DevExpress.XtraLayout.LayoutControlItem ItemForMalade;
        private DevExpress.XtraLayout.LayoutControlItem ItemForRang;
        private BindingSource consumptionDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    }
}