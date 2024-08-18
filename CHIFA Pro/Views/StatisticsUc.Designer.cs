using CHIFA.DAL;

using DevExpress.XtraCharts;

namespace CHIFA.Pro.Others
{
    partial class StatisticsUc
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.Columns.GridColumn colPA;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsUc));
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject9 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject10 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject11 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject12 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions4 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject13 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject14 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject15 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject16 = new DevExpress.Utils.SerializableAppearanceObject();
            colTPa = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontant1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colQt1 = new DevExpress.XtraGrid.Columns.GridColumn();
            monthlyStatisticsDtoBindingSource = new BindingSource(components);
            monthlyStatisticsDtoBindingSource1 = new BindingSource(components);
            facturesByClientBindingSource = new BindingSource(components);
            statisticsBindingSource = new BindingSource(components);
            ribbonPage3 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            btnRefresh = new DevExpress.XtraBars.BarButtonItem();
            FromDate = new DevExpress.XtraBars.BarEditItem();
            fromDateRepo = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            ToDate = new DevExpress.XtraBars.BarEditItem();
            toDateRepo = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            btnClearDates = new DevExpress.XtraBars.BarButtonItem();
            ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            tabMovments = new DevExpress.XtraTab.XtraTabPage();
            gridSplitContainer1 = new DevExpress.XtraGrid.GridSplitContainer();
            gridMouvements = new DevExpress.XtraGrid.GridControl();
            mouvementDtoBindingSource = new BindingSource(components);
            gridViewMovements = new DevExpress.XtraGrid.Views.Grid.GridView();
            colCode = new DevExpress.XtraGrid.Columns.GridColumn();
            colProduit = new DevExpress.XtraGrid.Columns.GridColumn();
            colQt = new DevExpress.XtraGrid.Columns.GridColumn();
            colPrix = new DevExpress.XtraGrid.Columns.GridColumn();
            colDate = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontant = new DevExpress.XtraGrid.Columns.GridColumn();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            tabByClient = new DevExpress.XtraTab.XtraTabPage();
            gridByClient = new DevExpress.XtraGrid.GridControl();
            gridViewByClient = new DevExpress.XtraGrid.Views.Grid.GridView();
            colNumAssure = new DevExpress.XtraGrid.Columns.GridColumn();
            colAssure = new DevExpress.XtraGrid.Columns.GridColumn();
            colFactures = new DevExpress.XtraGrid.Columns.GridColumn();
            colMaj = new DevExpress.XtraGrid.Columns.GridColumn();
            colMantFact = new DevExpress.XtraGrid.Columns.GridColumn();
            colTR = new DevExpress.XtraGrid.Columns.GridColumn();
            colMarge = new DevExpress.XtraGrid.Columns.GridColumn();
            gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            tabMonthly = new DevExpress.XtraTab.XtraTabPage();
            gridMonthly = new DevExpress.XtraGrid.GridControl();
            gridViewMonthly = new DevExpress.XtraGrid.Views.Grid.GridView();
            colDate2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMantant = new DevExpress.XtraGrid.Columns.GridColumn();
            colBorderaux = new DevExpress.XtraGrid.Columns.GridColumn();
            colFactures1 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            tabStatistics = new DevExpress.XtraTab.XtraTabPage();
            gridStatistics = new DevExpress.XtraGrid.GridControl();
            gridViewStatistics = new DevExpress.XtraGrid.Views.Grid.GridView();
            colCode1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colProduit1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colPrix1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colDate1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colGP = new DevExpress.XtraGrid.Columns.GridColumn();
            colCodeDCI = new DevExpress.XtraGrid.Columns.GridColumn();
            tabCharts = new DevExpress.XtraTab.XtraTabPage();
            chrtCntrl = new ChartControl();
            stackPanel1 = new DevExpress.Utils.Layout.StackPanel();
            btnBordereaux = new SimpleButton();
            btnMontantMonthly = new SimpleButton();
            btnFacturesMonthly = new SimpleButton();
            btnProducts = new SimpleButton();
            btnPrincepceVsGeneric = new SimpleButton();
            btnMontantDaily = new SimpleButton();
            btnFacturesDaily = new SimpleButton();
            btnMontantWeekly = new SimpleButton();
            btnFacturesWeekly = new SimpleButton();
            tabControl = new DevExpress.XtraTab.XtraTabControl();
            traitSpec2BindingSource = new BindingSource(components);
            repositoryItemRibbonSearchEdit1 = new DevExpress.XtraBars.Ribbon.Internal.RepositoryItemRibbonSearchEdit();
            colPA = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)monthlyStatisticsDtoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)monthlyStatisticsDtoBindingSource1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)facturesByClientBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)statisticsBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fromDateRepo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fromDateRepo.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toDateRepo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toDateRepo.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit1.CalendarTimeProperties).BeginInit();
            tabMovments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridSplitContainer1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridSplitContainer1.Panel1).BeginInit();
            gridSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridSplitContainer1.Panel2).BeginInit();
            gridSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridMouvements).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mouvementDtoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridViewMovements).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            tabByClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridByClient).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridViewByClient).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView2).BeginInit();
            tabMonthly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridMonthly).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridViewMonthly).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView3).BeginInit();
            tabStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridStatistics).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridViewStatistics).BeginInit();
            tabCharts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chrtCntrl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)stackPanel1).BeginInit();
            stackPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tabControl).BeginInit();
            tabControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)traitSpec2BindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemRibbonSearchEdit1).BeginInit();
            SuspendLayout();
            // 
            // colPA
            // 
            colPA.Caption = "PA";
            colPA.DisplayFormat.FormatString = "N";
            colPA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colPA.FieldName = "colPA";
            colPA.Name = "colPA";
            colPA.OptionsColumn.ReadOnly = true;
            colPA.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "colPA", "{0}") });
            colPA.UnboundExpression = "[Prix] / 1.2";
            colPA.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            colPA.Visible = true;
            colPA.VisibleIndex = 6;
            colPA.Width = 76;
            // 
            // colTPa
            // 
            colTPa.Caption = "T PA";
            colTPa.DisplayFormat.FormatString = "N";
            colTPa.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colTPa.FieldName = "colTPa";
            colTPa.Name = "colTPa";
            colTPa.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, DevExpress.Data.SummaryMode.Mixed, "colTPa", "SUM={0:n2}") });
            colTPa.UnboundExpression = "[colPA] * [Qt]";
            colTPa.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            colTPa.Visible = true;
            colTPa.VisibleIndex = 7;
            colTPa.Width = 76;
            // 
            // colMontant1
            // 
            colMontant1.AppearanceCell.BackColor = Color.FromArgb(192, 255, 192);
            colMontant1.AppearanceCell.Options.UseBackColor = true;
            colMontant1.DisplayFormat.FormatString = "n";
            colMontant1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontant1.FieldName = "Montant";
            colMontant1.Name = "colMontant1";
            colMontant1.OptionsColumn.ReadOnly = true;
            colMontant1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, DevExpress.Data.SummaryMode.Mixed, "Montant", "SUM={0:n}") });
            colMontant1.Visible = true;
            colMontant1.VisibleIndex = 5;
            colMontant1.Width = 196;
            // 
            // colQt1
            // 
            colQt1.AppearanceCell.BackColor = Color.FromArgb(255, 224, 192);
            colQt1.AppearanceCell.Options.UseBackColor = true;
            colQt1.FieldName = "Qt";
            colQt1.Name = "colQt1";
            colQt1.OptionsColumn.ReadOnly = true;
            colQt1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Qt", "{0}") });
            colQt1.Visible = true;
            colQt1.VisibleIndex = 3;
            colQt1.Width = 84;
            // 
            // monthlyStatisticsDtoBindingSource
            // 
            monthlyStatisticsDtoBindingSource.DataSource = typeof(DAL.Statistics.BordMonthlyStatDto);
            // 
            // monthlyStatisticsDtoBindingSource1
            // 
            monthlyStatisticsDtoBindingSource1.DataSource = typeof(DAL.Statistics.BordMonthlyStatDto);
            // 
            // facturesByClientBindingSource
            // 
            facturesByClientBindingSource.DataSource = typeof(DAL.Statistics.FacturesByClient);
            // 
            // statisticsBindingSource
            // 
            statisticsBindingSource.DataSource = typeof(StatisticsService);
            // 
            // ribbonPage3
            // 
            ribbonPage3.Name = "ribbonPage3";
            ribbonPage3.Text = "ribbonPage3";
            // 
            // ribbonPage1
            // 
            ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { ribbonPageGroup2 });
            ribbonPage1.Name = "ribbonPage1";
            ribbonPage1.Text = "ribbonPage1";
            // 
            // ribbonPageGroup2
            // 
            ribbonPageGroup2.ItemLinks.Add(btnRefresh);
            ribbonPageGroup2.ItemLinks.Add(FromDate);
            ribbonPageGroup2.ItemLinks.Add(ToDate);
            ribbonPageGroup2.ItemLinks.Add(btnClearDates);
            ribbonPageGroup2.Name = "ribbonPageGroup2";
            ribbonPageGroup2.Text = "Range";
            // 
            // btnRefresh
            // 
            btnRefresh.Caption = "Refresh";
            btnRefresh.Id = 6;
            btnRefresh.ImageOptions.Image = (Image)resources.GetObject("btnRefresh.ImageOptions.Image");
            btnRefresh.ImageOptions.LargeImage = (Image)resources.GetObject("btnRefresh.ImageOptions.LargeImage");
            btnRefresh.Name = "btnRefresh";
            btnRefresh.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            btnRefresh.ItemClick += BtnRefresh_ItemClick;
            // 
            // FromDate
            // 
            FromDate.Caption = "From:";
            FromDate.Edit = fromDateRepo;
            FromDate.EditWidth = 100;
            FromDate.Id = 15;
            FromDate.Name = "FromDate";
            FromDate.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            FromDate.EditValueChanged += FromDate_EditValueChanged;
            // 
            // fromDateRepo
            // 
            fromDateRepo.AutoHeight = false;
            fromDateRepo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            fromDateRepo.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            fromDateRepo.Name = "fromDateRepo";
            // 
            // ToDate
            // 
            ToDate.Caption = "To:";
            ToDate.Edit = toDateRepo;
            ToDate.EditWidth = 100;
            ToDate.Id = 16;
            ToDate.Name = "ToDate";
            ToDate.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            ToDate.EditValueChanged += ToDate_EditValueChanged;
            // 
            // toDateRepo
            // 
            toDateRepo.AutoHeight = false;
            toDateRepo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            toDateRepo.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            toDateRepo.Name = "toDateRepo";
            // 
            // btnClearDates
            // 
            btnClearDates.Caption = "Reset";
            btnClearDates.Id = 17;
            btnClearDates.ImageOptions.Image = (Image)resources.GetObject("btnClearDates.ImageOptions.Image");
            btnClearDates.ImageOptions.LargeImage = (Image)resources.GetObject("btnClearDates.ImageOptions.LargeImage");
            btnClearDates.Name = "btnClearDates";
            btnClearDates.ItemClick += BtnClearDates_ItemClick;
            // 
            // ribbonControl1
            // 
            ribbonControl1.AllowMinimizeRibbon = false;
            ribbonControl1.AutoSizeItems = true;
            ribbonControl1.DrawGroupCaptions = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.DrawGroupsBorderMode = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.EmptyAreaImageOptions.ImagePadding = new Padding(24);
            ribbonControl1.ExpandCollapseItem.Id = 0;
            ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { ribbonControl1.ExpandCollapseItem, btnRefresh, FromDate, ToDate, btnClearDates });
            ribbonControl1.Location = new Point(0, 0);
            ribbonControl1.MaxItemId = 19;
            ribbonControl1.Name = "ribbonControl1";
            ribbonControl1.OptionsMenuMinWidth = 264;
            ribbonControl1.OptionsPageCategories.ShowCaptions = false;
            ribbonControl1.OptionsTouch.ShowTouchUISelectorInSearchMenu = false;
            ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { ribbonPage1 });
            ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { fromDateRepo, toDateRepo, repositoryItemDateEdit1 });
            ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.MacOffice;
            ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.ShowPageHeadersInFormCaption = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            ribbonControl1.ShowQatLocationSelector = false;
            ribbonControl1.ShowToolbarCustomizeItem = false;
            ribbonControl1.Size = new Size(916, 91);
            ribbonControl1.Toolbar.ShowCustomizeItem = false;
            ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // repositoryItemDateEdit1
            // 
            repositoryItemDateEdit1.AutoHeight = false;
            repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // tabMovments
            // 
            tabMovments.Controls.Add(gridSplitContainer1);
            tabMovments.ImageOptions.Image = (Image)resources.GetObject("tabMovments.ImageOptions.Image");
            tabMovments.Margin = new Padding(4, 3, 4, 3);
            tabMovments.Name = "tabMovments";
            tabMovments.Size = new Size(914, 357);
            tabMovments.Text = "Movements";
            // 
            // gridSplitContainer1
            // 
            gridSplitContainer1.Dock = DockStyle.Fill;
            gridSplitContainer1.Grid = gridMouvements;
            gridSplitContainer1.Location = new Point(0, 0);
            gridSplitContainer1.Margin = new Padding(4, 3, 4, 3);
            gridSplitContainer1.Name = "gridSplitContainer1";
            // 
            // gridSplitContainer1.Panel1
            // 
            gridSplitContainer1.Panel1.Controls.Add(gridMouvements);
            gridSplitContainer1.Size = new Size(913, 365);
            gridSplitContainer1.TabIndex = 0;
            // 
            // gridMouvements
            // 
            gridMouvements.DataSource = mouvementDtoBindingSource;
            gridMouvements.Dock = DockStyle.Fill;
            gridMouvements.EmbeddedNavigator.Margin = new Padding(4, 3, 4, 3);
            gridMouvements.Location = new Point(0, 0);
            gridMouvements.MainView = gridViewMovements;
            gridMouvements.Margin = new Padding(4, 3, 4, 3);
            gridMouvements.Name = "gridMouvements";
            gridMouvements.Size = new Size(913, 365);
            gridMouvements.TabIndex = 0;
            gridMouvements.ViewCollection.AddRange(new BaseView[] { gridViewMovements, gridView1 });
            // 
            // mouvementDtoBindingSource
            // 
            mouvementDtoBindingSource.DataSource = typeof(MouvementDto);
            // 
            // gridViewMovements
            // 
            gridViewMovements.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colCode, colProduit, colQt, colPrix, colDate, colMontant });
            gridViewMovements.DetailHeight = 271;
            gridViewMovements.GridControl = gridMouvements;
            gridViewMovements.Name = "gridViewMovements";
            gridViewMovements.OptionsEditForm.PopupEditFormWidth = 581;
            gridViewMovements.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            gridViewMovements.OptionsView.ShowFooter = true;
            // 
            // colCode
            // 
            colCode.FieldName = "Code";
            colCode.Name = "colCode";
            colCode.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Code", "{0}") });
            colCode.Visible = true;
            colCode.VisibleIndex = 0;
            colCode.Width = 76;
            // 
            // colProduit
            // 
            colProduit.FieldName = "Produit";
            colProduit.Name = "colProduit";
            colProduit.Visible = true;
            colProduit.VisibleIndex = 1;
            colProduit.Width = 305;
            // 
            // colQt
            // 
            colQt.AppearanceCell.BackColor = Color.FromArgb(255, 224, 192);
            colQt.AppearanceCell.Options.UseBackColor = true;
            colQt.FieldName = "Qt";
            colQt.Name = "colQt";
            colQt.Visible = true;
            colQt.VisibleIndex = 2;
            colQt.Width = 68;
            // 
            // colPrix
            // 
            colPrix.AppearanceCell.BackColor = Color.FromArgb(192, 255, 192);
            colPrix.AppearanceCell.Options.UseBackColor = true;
            colPrix.DisplayFormat.FormatString = "n";
            colPrix.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colPrix.FieldName = "Prix";
            colPrix.Name = "colPrix";
            colPrix.Visible = true;
            colPrix.VisibleIndex = 3;
            colPrix.Width = 183;
            // 
            // colDate
            // 
            colDate.FieldName = "Date";
            colDate.Name = "colDate";
            colDate.Visible = true;
            colDate.VisibleIndex = 4;
            colDate.Width = 96;
            // 
            // colMontant
            // 
            colMontant.DisplayFormat.FormatString = "n";
            colMontant.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontant.FieldName = "Montant";
            colMontant.GroupFormat.FormatString = "n";
            colMontant.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontant.Name = "colMontant";
            colMontant.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Montant", "SUM={0:0.##}") });
            colMontant.Visible = true;
            colMontant.VisibleIndex = 5;
            colMontant.Width = 276;
            // 
            // gridView1
            // 
            gridView1.DetailHeight = 271;
            gridView1.GridControl = gridMouvements;
            gridView1.Name = "gridView1";
            gridView1.OptionsEditForm.PopupEditFormWidth = 581;
            // 
            // tabByClient
            // 
            tabByClient.Controls.Add(gridByClient);
            tabByClient.ImageOptions.Image = (Image)resources.GetObject("tabByClient.ImageOptions.Image");
            tabByClient.Margin = new Padding(4, 3, 4, 3);
            tabByClient.Name = "tabByClient";
            tabByClient.Size = new Size(914, 357);
            tabByClient.Text = "By Client";
            // 
            // gridByClient
            // 
            gridByClient.DataSource = facturesByClientBindingSource;
            gridByClient.Dock = DockStyle.Fill;
            gridByClient.EmbeddedNavigator.Margin = new Padding(4, 3, 4, 3);
            gridByClient.Location = new Point(0, 0);
            gridByClient.MainView = gridViewByClient;
            gridByClient.Margin = new Padding(4, 3, 4, 3);
            gridByClient.Name = "gridByClient";
            gridByClient.Size = new Size(913, 365);
            gridByClient.TabIndex = 0;
            gridByClient.ViewCollection.AddRange(new BaseView[] { gridViewByClient, gridView2 });
            // 
            // gridViewByClient
            // 
            gridViewByClient.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colNumAssure, colAssure, colFactures, colMaj, colMantFact, colTR, colMarge });
            gridViewByClient.DetailHeight = 271;
            gridViewByClient.GridControl = gridByClient;
            gridViewByClient.Name = "gridViewByClient";
            gridViewByClient.OptionsEditForm.PopupEditFormWidth = 581;
            gridViewByClient.OptionsView.ShowFooter = true;
            // 
            // colNumAssure
            // 
            colNumAssure.FieldName = "NumAssure";
            colNumAssure.Name = "colNumAssure";
            colNumAssure.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "NumAssure", "{0}") });
            colNumAssure.Visible = true;
            colNumAssure.VisibleIndex = 0;
            colNumAssure.Width = 76;
            // 
            // colAssure
            // 
            colAssure.AppearanceCell.BackColor = Color.FromArgb(255, 255, 192);
            colAssure.AppearanceCell.Options.UseBackColor = true;
            colAssure.AppearanceCell.Options.UseFont = true;
            colAssure.FieldName = "Malade";
            colAssure.Name = "colAssure";
            colAssure.Visible = true;
            colAssure.VisibleIndex = 1;
            colAssure.Width = 76;
            // 
            // colFactures
            // 
            colFactures.FieldName = "Factures";
            colFactures.Name = "colFactures";
            colFactures.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Factures", "SUM={0:0.##}") });
            colFactures.Visible = true;
            colFactures.VisibleIndex = 2;
            colFactures.Width = 76;
            // 
            // colMaj
            // 
            colMaj.DisplayFormat.FormatString = "n";
            colMaj.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMaj.FieldName = "Maj";
            colMaj.Name = "colMaj";
            colMaj.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Maj", "SUM={0:n}") });
            colMaj.Visible = true;
            colMaj.VisibleIndex = 3;
            colMaj.Width = 76;
            // 
            // colMantFact
            // 
            colMantFact.AppearanceCell.BackColor = Color.FromArgb(255, 224, 192);
            colMantFact.AppearanceCell.Options.UseBackColor = true;
            colMantFact.DisplayFormat.FormatString = "n";
            colMantFact.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMantFact.FieldName = "MantFact";
            colMantFact.Name = "colMantFact";
            colMantFact.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MantFact", "SUM={0:n}") });
            colMantFact.Visible = true;
            colMantFact.VisibleIndex = 4;
            colMantFact.Width = 76;
            // 
            // colTR
            // 
            colTR.DisplayFormat.FormatString = "n";
            colTR.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colTR.FieldName = "TR";
            colTR.Name = "colTR";
            colTR.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TR", "SUM={0:n}") });
            colTR.Visible = true;
            colTR.VisibleIndex = 5;
            colTR.Width = 76;
            // 
            // colMarge
            // 
            colMarge.AppearanceCell.BackColor = Color.FromArgb(192, 255, 192);
            colMarge.AppearanceCell.Options.UseBackColor = true;
            colMarge.DisplayFormat.FormatString = "n";
            colMarge.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMarge.FieldName = "Marge";
            colMarge.Name = "colMarge";
            colMarge.OptionsColumn.ReadOnly = true;
            colMarge.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Marge", "SUM={0:n}") });
            colMarge.Visible = true;
            colMarge.VisibleIndex = 6;
            colMarge.Width = 76;
            // 
            // gridView2
            // 
            gridView2.DetailHeight = 271;
            gridView2.GridControl = gridByClient;
            gridView2.Name = "gridView2";
            gridView2.OptionsEditForm.PopupEditFormWidth = 581;
            // 
            // tabMonthly
            // 
            tabMonthly.Controls.Add(gridMonthly);
            tabMonthly.ImageOptions.Image = (Image)resources.GetObject("tabMonthly.ImageOptions.Image");
            tabMonthly.Margin = new Padding(4, 3, 4, 3);
            tabMonthly.Name = "tabMonthly";
            tabMonthly.Size = new Size(910, 376);
            tabMonthly.Text = "Monthly";
            // 
            // gridMonthly
            // 
            gridMonthly.DataSource = monthlyStatisticsDtoBindingSource1;
            gridMonthly.Dock = DockStyle.Fill;
            gridMonthly.EmbeddedNavigator.Margin = new Padding(4, 3, 4, 3);
            gridMonthly.Location = new Point(0, 0);
            gridMonthly.MainView = gridViewMonthly;
            gridMonthly.Margin = new Padding(4, 3, 4, 3);
            gridMonthly.Name = "gridMonthly";
            gridMonthly.Size = new Size(910, 376);
            gridMonthly.TabIndex = 0;
            gridMonthly.ViewCollection.AddRange(new BaseView[] { gridViewMonthly, gridView3 });
            // 
            // gridViewMonthly
            // 
            gridViewMonthly.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colDate2, colMantant, colBorderaux, colFactures1 });
            gridViewMonthly.DetailHeight = 271;
            gridViewMonthly.GridControl = gridMonthly;
            gridViewMonthly.Name = "gridViewMonthly";
            gridViewMonthly.OptionsEditForm.PopupEditFormWidth = 581;
            gridViewMonthly.OptionsView.ShowFooter = true;
            // 
            // colDate2
            // 
            colDate2.FieldName = "Date";
            colDate2.Name = "colDate2";
            colDate2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Date", "{0}") });
            colDate2.Visible = true;
            colDate2.VisibleIndex = 0;
            colDate2.Width = 76;
            // 
            // colMantant
            // 
            colMantant.AppearanceCell.Options.UseFont = true;
            colMantant.DisplayFormat.FormatString = "n";
            colMantant.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMantant.FieldName = "Montant";
            colMantant.Name = "colMantant";
            colMantant.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Montant", "SUM={0:0.##}") });
            colMantant.Visible = true;
            colMantant.VisibleIndex = 1;
            colMantant.Width = 76;
            // 
            // colBorderaux
            // 
            colBorderaux.FieldName = "Borderaux";
            colBorderaux.Name = "colBorderaux";
            colBorderaux.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Borderaux", "{0}") });
            colBorderaux.Visible = true;
            colBorderaux.VisibleIndex = 2;
            colBorderaux.Width = 76;
            // 
            // colFactures1
            // 
            colFactures1.FieldName = "Factures";
            colFactures1.Name = "colFactures1";
            colFactures1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Factures", "{0}") });
            colFactures1.Visible = true;
            colFactures1.VisibleIndex = 3;
            colFactures1.Width = 76;
            // 
            // gridView3
            // 
            gridView3.DetailHeight = 271;
            gridView3.GridControl = gridMonthly;
            gridView3.Name = "gridView3";
            gridView3.OptionsEditForm.PopupEditFormWidth = 581;
            // 
            // tabStatistics
            // 
            tabStatistics.Controls.Add(gridStatistics);
            tabStatistics.ImageOptions.Image = (Image)resources.GetObject("tabStatistics.ImageOptions.Image");
            tabStatistics.Margin = new Padding(4, 3, 4, 3);
            tabStatistics.Name = "tabStatistics";
            tabStatistics.Size = new Size(914, 357);
            tabStatistics.Text = "Statistics";
            // 
            // gridStatistics
            // 
            gridStatistics.DataSource = mouvementDtoBindingSource;
            gridStatistics.Dock = DockStyle.Fill;
            gridStatistics.EmbeddedNavigator.Margin = new Padding(4, 3, 4, 3);
            gridStatistics.Location = new Point(0, 0);
            gridStatistics.MainView = gridViewStatistics;
            gridStatistics.Margin = new Padding(4, 3, 4, 3);
            gridStatistics.Name = "gridStatistics";
            gridStatistics.Size = new Size(913, 365);
            gridStatistics.TabIndex = 0;
            gridStatistics.ViewCollection.AddRange(new BaseView[] { gridViewStatistics });
            // 
            // gridViewStatistics
            // 
            gridViewStatistics.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colCode1, colProduit1, colQt1, colPrix1, colDate1, colMontant1, colPA, colTPa, colGP, colCodeDCI });
            gridViewStatistics.DetailHeight = 271;
            gridViewStatistics.GridControl = gridStatistics;
            gridViewStatistics.GroupCount = 1;
            gridViewStatistics.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "colTPa", null, "[Sum: {0:n2}]"), new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Qt", null, ""), new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "colTPa", colTPa, "{0:n2}"), new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Montant", colMontant1, "{0:n2}"), new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Qt", colQt1, "") });
            gridViewStatistics.Name = "gridViewStatistics";
            gridViewStatistics.OptionsBehavior.AutoExpandAllGroups = true;
            gridViewStatistics.OptionsEditForm.EditFormColumnCount = 1;
            gridViewStatistics.OptionsEditForm.PopupEditFormWidth = 581;
            gridViewStatistics.OptionsSelection.MultiSelect = true;
            gridViewStatistics.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            gridViewStatistics.OptionsView.ShowFooter = true;
            gridViewStatistics.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] { new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colCodeDCI, DevExpress.Data.ColumnSortOrder.Ascending) });
            // 
            // colCode1
            // 
            colCode1.FieldName = "Code";
            colCode1.Name = "colCode1";
            colCode1.OptionsColumn.ReadOnly = true;
            colCode1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Code", "{0}") });
            colCode1.Visible = true;
            colCode1.VisibleIndex = 1;
            colCode1.Width = 168;
            // 
            // colProduit1
            // 
            colProduit1.FieldName = "Produit";
            colProduit1.Name = "colProduit1";
            colProduit1.OptionsColumn.ReadOnly = true;
            colProduit1.Visible = true;
            colProduit1.VisibleIndex = 2;
            colProduit1.Width = 168;
            // 
            // colPrix1
            // 
            colPrix1.DisplayFormat.FormatString = "n";
            colPrix1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colPrix1.FieldName = "Prix";
            colPrix1.Name = "colPrix1";
            colPrix1.OptionsColumn.ReadOnly = true;
            colPrix1.Visible = true;
            colPrix1.VisibleIndex = 4;
            colPrix1.Width = 196;
            // 
            // colDate1
            // 
            colDate1.FieldName = "Date";
            colDate1.Name = "colDate1";
            colDate1.OptionsColumn.ReadOnly = true;
            colDate1.Width = 196;
            // 
            // colGP
            // 
            colGP.Caption = "G/P";
            colGP.FieldName = "Generic";
            colGP.Name = "colGP";
            colGP.Visible = true;
            colGP.VisibleIndex = 8;
            colGP.Width = 76;
            // 
            // colCodeDCI
            // 
            colCodeDCI.FieldName = "CodeDci";
            colCodeDCI.Name = "colCodeDCI";
            colCodeDCI.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "CodeDCI", "{0}"), new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "colTPa", "SUM={0:0.##}") });
            colCodeDCI.Visible = true;
            colCodeDCI.VisibleIndex = 1;
            colCodeDCI.Width = 92;
            // 
            // tabCharts
            // 
            tabCharts.Controls.Add(chrtCntrl);
            tabCharts.Controls.Add(stackPanel1);
            tabCharts.ImageOptions.Image = (Image)resources.GetObject("tabCharts.ImageOptions.Image");
            tabCharts.Name = "tabCharts";
            tabCharts.Size = new Size(910, 376);
            tabCharts.Text = "Charts";
            // 
            // chrtCntrl
            // 
            chrtCntrl.Dock = DockStyle.Fill;
            chrtCntrl.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            chrtCntrl.Legend.AlignmentVertical = LegendAlignmentVertical.TopOutside;
            chrtCntrl.Legend.LegendID = -1;
            chrtCntrl.Legend.Name = "Default Legend";
            chrtCntrl.Location = new Point(190, 0);
            chrtCntrl.Name = "chrtCntrl";
            chrtCntrl.Size = new Size(720, 376);
            chrtCntrl.TabIndex = 0;
            // 
            // stackPanel1
            // 
            stackPanel1.AutoScroll = true;
            stackPanel1.AutoSize = true;
            stackPanel1.Controls.Add(btnBordereaux);
            stackPanel1.Controls.Add(btnMontantMonthly);
            stackPanel1.Controls.Add(btnFacturesMonthly);
            stackPanel1.Controls.Add(btnProducts);
            stackPanel1.Controls.Add(btnPrincepceVsGeneric);
            stackPanel1.Controls.Add(btnMontantDaily);
            stackPanel1.Controls.Add(btnFacturesDaily);
            stackPanel1.Controls.Add(btnMontantWeekly);
            stackPanel1.Controls.Add(btnFacturesWeekly);
            stackPanel1.Dock = DockStyle.Left;
            stackPanel1.LayoutDirection = DevExpress.Utils.Layout.StackPanelLayoutDirection.TopDown;
            stackPanel1.Location = new Point(0, 0);
            stackPanel1.Name = "stackPanel1";
            stackPanel1.Size = new Size(190, 376);
            stackPanel1.TabIndex = 1;
            // 
            // btnBordereaux
            // 
            btnBordereaux.Location = new Point(3, 3);
            btnBordereaux.Name = "btnBordereaux";
            btnBordereaux.Size = new Size(184, 30);
            btnBordereaux.TabIndex = 2;
            btnBordereaux.Text = "Bordereaux";
            btnBordereaux.Click += BtnBordereaux_Click;
            // 
            // btnMontantMonthly
            // 
            btnMontantMonthly.Location = new Point(3, 39);
            btnMontantMonthly.Name = "btnMontantMonthly";
            btnMontantMonthly.Size = new Size(184, 30);
            btnMontantMonthly.TabIndex = 2;
            btnMontantMonthly.Text = "Montant Monthly";
            btnMontantMonthly.Click += BtnMontantMonthly_Click;
            // 
            // btnFacturesMonthly
            // 
            btnFacturesMonthly.Location = new Point(3, 75);
            btnFacturesMonthly.Name = "btnFacturesMonthly";
            btnFacturesMonthly.Size = new Size(184, 30);
            btnFacturesMonthly.TabIndex = 6;
            btnFacturesMonthly.Text = "Factures Monthly";
            btnFacturesMonthly.Click += BtnFacturesMonthly_Click;
            // 
            // btnProducts
            // 
            btnProducts.Location = new Point(3, 111);
            btnProducts.Name = "btnProducts";
            btnProducts.Size = new Size(184, 30);
            btnProducts.TabIndex = 2;
            btnProducts.Text = "Products";
            btnProducts.Click += BtnProducts_Click;
            // 
            // btnPrincepceVsGeneric
            // 
            btnPrincepceVsGeneric.Location = new Point(3, 147);
            btnPrincepceVsGeneric.Name = "btnPrincepceVsGeneric";
            btnPrincepceVsGeneric.Size = new Size(184, 30);
            btnPrincepceVsGeneric.TabIndex = 2;
            btnPrincepceVsGeneric.Text = "Generique / Princeps";
            btnPrincepceVsGeneric.Click += BtnPrincepceVsGeneric_Click;
            // 
            // btnMontantDaily
            // 
            btnMontantDaily.Location = new Point(3, 183);
            btnMontantDaily.Name = "btnMontantDaily";
            btnMontantDaily.Size = new Size(184, 30);
            btnMontantDaily.TabIndex = 3;
            btnMontantDaily.Text = "Montant Daily";
            btnMontantDaily.Click += BtnMontantDaily_Click;
            // 
            // btnFacturesDaily
            // 
            btnFacturesDaily.Location = new Point(3, 219);
            btnFacturesDaily.Name = "btnFacturesDaily";
            btnFacturesDaily.Size = new Size(184, 30);
            btnFacturesDaily.TabIndex = 4;
            btnFacturesDaily.Text = "Factures Daily";
            btnFacturesDaily.Click += BtnFacturesDaily_Click;
            // 
            // btnMontantWeekly
            // 
            btnMontantWeekly.Location = new Point(3, 255);
            btnMontantWeekly.Name = "btnMontantWeekly";
            btnMontantWeekly.Size = new Size(184, 30);
            btnMontantWeekly.TabIndex = 5;
            btnMontantWeekly.Text = "Montant Weekly";
            btnMontantWeekly.Click += BtnMontantWeekly_Click;
            // 
            // btnFacturesWeekly
            // 
            btnFacturesWeekly.Location = new Point(3, 291);
            btnFacturesWeekly.Name = "btnFacturesWeekly";
            btnFacturesWeekly.Size = new Size(184, 30);
            btnFacturesWeekly.TabIndex = 7;
            btnFacturesWeekly.Text = "Factures Weekly";
            btnFacturesWeekly.Click += BtnFacturesWeekly_Click;
            // 
            // tabControl
            // 
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 91);
            tabControl.Margin = new Padding(4, 3, 4, 3);
            tabControl.Name = "tabControl";
            tabControl.SelectedTabPage = tabCharts;
            tabControl.Size = new Size(916, 423);
            tabControl.TabIndex = 1;
            tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] { tabCharts, tabStatistics, tabMonthly, tabByClient, tabMovments });
            tabControl.SelectedPageChanged += tabControl_SelectedPageChanged;
            // 
            // traitSpec2BindingSource
            // 
            traitSpec2BindingSource.DataSource = typeof(TraitSpec2);
            // 
            // repositoryItemRibbonSearchEdit1
            // 
            repositoryItemRibbonSearchEdit1.AllowFocused = false;
            repositoryItemRibbonSearchEdit1.AutoHeight = false;
            repositoryItemRibbonSearchEdit1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            editorButtonImageOptions3.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            editorButtonImageOptions3.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("editorButtonImageOptions3.SvgImage");
            repositoryItemRibbonSearchEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, true, editorButtonImageOptions3, new DevExpress.Utils.KeyShortcut(Keys.None), serializableAppearanceObject9, serializableAppearanceObject10, serializableAppearanceObject11, serializableAppearanceObject12, "", null, null, DevExpress.Utils.ToolTipAnchor.Default), new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear, "", -1, true, false, false, editorButtonImageOptions4, new DevExpress.Utils.KeyShortcut(Keys.None), serializableAppearanceObject13, serializableAppearanceObject14, serializableAppearanceObject15, serializableAppearanceObject16, "", null, null, DevExpress.Utils.ToolTipAnchor.Default) });
            repositoryItemRibbonSearchEdit1.Name = "repositoryItemRibbonSearchEdit1";
            repositoryItemRibbonSearchEdit1.NullText = "Search";
            // 
            // StatisticsUc
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabControl);
            Controls.Add(ribbonControl1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "StatisticsUc";
            Size = new Size(916, 514);
            Load += MovementsUc_Load;
            ((System.ComponentModel.ISupportInitialize)monthlyStatisticsDtoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)monthlyStatisticsDtoBindingSource1).EndInit();
            ((System.ComponentModel.ISupportInitialize)facturesByClientBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)statisticsBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)fromDateRepo.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)fromDateRepo).EndInit();
            ((System.ComponentModel.ISupportInitialize)toDateRepo.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)toDateRepo).EndInit();
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit1.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit1).EndInit();
            tabMovments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridSplitContainer1.Panel1).EndInit();
            gridSplitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridSplitContainer1.Panel2).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridSplitContainer1).EndInit();
            gridSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridMouvements).EndInit();
            ((System.ComponentModel.ISupportInitialize)mouvementDtoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridViewMovements).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            tabByClient.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridByClient).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridViewByClient).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView2).EndInit();
            tabMonthly.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridMonthly).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridViewMonthly).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView3).EndInit();
            tabStatistics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridStatistics).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridViewStatistics).EndInit();
            tabCharts.ResumeLayout(false);
            tabCharts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)chrtCntrl).EndInit();
            ((System.ComponentModel.ISupportInitialize)stackPanel1).EndInit();
            stackPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)tabControl).EndInit();
            tabControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)traitSpec2BindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemRibbonSearchEdit1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private BindingSource facturesByClientBindingSource;
        private BindingSource statisticsBindingSource;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage3;
        private BindingSource monthlyStatisticsDtoBindingSource;
        private BindingSource monthlyStatisticsDtoBindingSource1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.BarButtonItem btnRefresh;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarEditItem FromDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit fromDateRepo;
        private DevExpress.XtraBars.BarEditItem ToDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit toDateRepo;
        private DevExpress.XtraBars.BarButtonItem btnClearDates;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraTab.XtraTabPage tabMovments;
        private DevExpress.XtraGrid.GridSplitContainer gridSplitContainer1;
        private DevExpress.XtraGrid.GridControl gridMouvements;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewMovements;
        private DevExpress.XtraGrid.Columns.GridColumn colCode;
        private DevExpress.XtraGrid.Columns.GridColumn colProduit;
        private DevExpress.XtraGrid.Columns.GridColumn colQt;
        private DevExpress.XtraGrid.Columns.GridColumn colPrix;
        private DevExpress.XtraGrid.Columns.GridColumn colDate;
        private DevExpress.XtraGrid.Columns.GridColumn colMontant;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraTab.XtraTabPage tabByClient;
        private DevExpress.XtraGrid.GridControl gridByClient;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewByClient;
        private DevExpress.XtraGrid.Columns.GridColumn colNumAssure;
        private DevExpress.XtraGrid.Columns.GridColumn colAssure;
        private DevExpress.XtraGrid.Columns.GridColumn colFactures;
        private DevExpress.XtraGrid.Columns.GridColumn colMaj;
        private DevExpress.XtraGrid.Columns.GridColumn colMantFact;
        private DevExpress.XtraGrid.Columns.GridColumn colTR;
        private DevExpress.XtraGrid.Columns.GridColumn colMarge;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraTab.XtraTabPage tabMonthly;
        private DevExpress.XtraGrid.GridControl gridMonthly;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewMonthly;
        private DevExpress.XtraGrid.Columns.GridColumn colDate2;
        private DevExpress.XtraGrid.Columns.GridColumn colMantant;
        private DevExpress.XtraGrid.Columns.GridColumn colBorderaux;
        private DevExpress.XtraGrid.Columns.GridColumn colFactures1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraTab.XtraTabPage tabStatistics;
        private DevExpress.XtraGrid.GridControl gridStatistics;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewStatistics;
        private DevExpress.XtraGrid.Columns.GridColumn colCode1;
        private DevExpress.XtraGrid.Columns.GridColumn colProduit1;
        private DevExpress.XtraGrid.Columns.GridColumn colQt1;
        private DevExpress.XtraGrid.Columns.GridColumn colPrix1;
        private DevExpress.XtraGrid.Columns.GridColumn colDate1;
        private DevExpress.XtraGrid.Columns.GridColumn colMontant1;
        private DevExpress.XtraGrid.Columns.GridColumn colTPa;
        private DevExpress.XtraTab.XtraTabPage tabCharts;
        private ChartControl chrtCntrl;
        private DevExpress.Utils.Layout.StackPanel stackPanel1;
        private SimpleButton btnBordereaux;
        private SimpleButton btnMontantMonthly;
        private SimpleButton btnFacturesMonthly;
        private SimpleButton btnProducts;
        private SimpleButton btnPrincepceVsGeneric;
        private SimpleButton btnMontantDaily;
        private SimpleButton btnFacturesDaily;
        private SimpleButton btnMontantWeekly;
        private SimpleButton btnFacturesWeekly;
        private DevExpress.XtraTab.XtraTabControl tabControl;
        private DevExpress.XtraGrid.Columns.GridColumn colGP;
        private BindingSource traitSpec2BindingSource;
        private BindingSource mouvementDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colCodeDCI;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.Ribbon.Internal.RepositoryItemRibbonSearchEdit repositoryItemRibbonSearchEdit1;
    }
}
