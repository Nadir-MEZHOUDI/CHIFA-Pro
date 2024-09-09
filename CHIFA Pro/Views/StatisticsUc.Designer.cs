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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsUc));
            var xyDiagram1 = new XYDiagram();
            var xyDiagramPane1 = new XYDiagramPane();
            var xyDiagramPane2 = new XYDiagramPane();
            var xyDiagramPane3 = new XYDiagramPane();
            var secondaryAxisy1 = new SecondaryAxisY();
            var secondaryAxisy2 = new SecondaryAxisY();
            var series1 = new Series();
            var series2 = new Series();
            var sideBySideBarSeriesView1 = new SideBySideBarSeriesView();
            var series3 = new Series();
            var sideBySideBarSeriesView2 = new SideBySideBarSeriesView();
            var series4 = new Series();
            var sideBySideBarSeriesView3 = new SideBySideBarSeriesView();
            var series5 = new Series();
            var sideBySideBarSeriesView4 = new SideBySideBarSeriesView();
            var series6 = new Series();
            var sideBySideBarSeriesView5 = new SideBySideBarSeriesView();
            var series7 = new Series();
            var sideBySideBarSeriesView6 = new SideBySideBarSeriesView();
            var series8 = new Series();
            var sideBySideBarSeriesView7 = new SideBySideBarSeriesView();
            var series9 = new Series();
            var sideBySideBarSeriesView8 = new SideBySideBarSeriesView();
            var xyDiagram2 = new XYDiagram();
            var xyDiagramPane4 = new XYDiagramPane();
            var series10 = new Series();
            var series11 = new Series();
            var sideBySideBarSeriesView9 = new SideBySideBarSeriesView();
            var xyDiagram3 = new XYDiagram();
            var xyDiagramPane5 = new XYDiagramPane();
            var series12 = new Series();
            var series13 = new Series();
            var sideBySideBarSeriesView10 = new SideBySideBarSeriesView();
            var xyDiagram4 = new XYDiagram();
            var xyDiagramPane6 = new XYDiagramPane();
            var series14 = new Series();
            var sideBySideBarSeriesView11 = new SideBySideBarSeriesView();
            var series15 = new Series();
            var sideBySideBarSeriesView12 = new SideBySideBarSeriesView();
            var editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            var serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            var serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            var serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            var serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            var editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            var serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            var serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            var serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            var serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            colTPa = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontant1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colQt1 = new DevExpress.XtraGrid.Columns.GridColumn();
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
            tabBordereauxTable = new DevExpress.XtraTab.XtraTabPage();
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            bordStatDtoBindingSource = new BindingSource(components);
            viewBord = new DevExpress.XtraGrid.Views.Grid.GridView();
            colDateDebut = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateFin = new DevExpress.XtraGrid.Columns.GridColumn();
            colDate = new DevExpress.XtraGrid.Columns.GridColumn();
            colNum = new DevExpress.XtraGrid.Columns.GridColumn();
            colFactures2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colJours = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantJour = new DevExpress.XtraGrid.Columns.GridColumn();
            colFactureJour = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantFacture = new DevExpress.XtraGrid.Columns.GridColumn();
            colCenter = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantMaj = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantFact = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantOff = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantFE = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantGlobal = new DevExpress.XtraGrid.Columns.GridColumn();
            colVirement = new DevExpress.XtraGrid.Columns.GridColumn();
            colMarge1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colBrute = new DevExpress.XtraGrid.Columns.GridColumn();
            colNet = new DevExpress.XtraGrid.Columns.GridColumn();
            colEcart = new DevExpress.XtraGrid.Columns.GridColumn();
            tabClientsTable = new DevExpress.XtraTab.XtraTabPage();
            gridByClient = new DevExpress.XtraGrid.GridControl();
            byClientStatBindingSource = new BindingSource(components);
            viewClients = new DevExpress.XtraGrid.Views.Grid.GridView();
            colNumAssure = new DevExpress.XtraGrid.Columns.GridColumn();
            colAssure = new DevExpress.XtraGrid.Columns.GridColumn();
            colFactures = new DevExpress.XtraGrid.Columns.GridColumn();
            colMaj = new DevExpress.XtraGrid.Columns.GridColumn();
            colMantFact = new DevExpress.XtraGrid.Columns.GridColumn();
            colTR = new DevExpress.XtraGrid.Columns.GridColumn();
            colMarge = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            tabMonthlyTable = new DevExpress.XtraTab.XtraTabPage();
            gridControl3 = new DevExpress.XtraGrid.GridControl();
            monthlyStatBindingSource = new BindingSource(components);
            viewMonthly = new DevExpress.XtraGrid.Views.Grid.GridView();
            colDate2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMonth = new DevExpress.XtraGrid.Columns.GridColumn();
            colYear = new DevExpress.XtraGrid.Columns.GridColumn();
            colFactures1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colJours1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantJour1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colFactureJour1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantFacture1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantMaj1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantFact1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantOff1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMarge2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colBrute1 = new DevExpress.XtraGrid.Columns.GridColumn();
            tabProductTable = new DevExpress.XtraTab.XtraTabPage();
            gridStatistics = new DevExpress.XtraGrid.GridControl();
            productStatBindingSource = new BindingSource(components);
            viewProducts = new DevExpress.XtraGrid.Views.Grid.GridView();
            colCode1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colProduit1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colPrix1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colDate1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colGP = new DevExpress.XtraGrid.Columns.GridColumn();
            colCodeDCI = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            tabClients = new DevExpress.XtraTab.XtraTabPage();
            chrtCntrl = new ChartControl();
            tabControl = new DevExpress.XtraTab.XtraTabControl();
            tabBordereaux = new DevExpress.XtraTab.XtraTabPage();
            chartBordereaux = new ChartControl();
            tabMonthly = new DevExpress.XtraTab.XtraTabPage();
            chartMonthly = new ChartControl();
            tabWeeklyTable = new DevExpress.XtraTab.XtraTabPage();
            gridControl2 = new DevExpress.XtraGrid.GridControl();
            weeklyStatBindingSource = new BindingSource(components);
            viewWeekly = new DevExpress.XtraGrid.Views.Grid.GridView();
            colDate3 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMonth1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colYear1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateDebut1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateFin1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colFactures3 = new DevExpress.XtraGrid.Columns.GridColumn();
            colJours2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantJour2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colFactureJour2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantFacture2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantMaj2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantFact2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantOff2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMarge3 = new DevExpress.XtraGrid.Columns.GridColumn();
            colBrute2 = new DevExpress.XtraGrid.Columns.GridColumn();
            tabWeekly = new DevExpress.XtraTab.XtraTabPage();
            chartWeekly = new ChartControl();
            tabDailyTable = new DevExpress.XtraTab.XtraTabPage();
            gridControl4 = new DevExpress.XtraGrid.GridControl();
            dailyStatBindingSource = new BindingSource(components);
            viewDaily = new DevExpress.XtraGrid.Views.Grid.GridView();
            colDate4 = new DevExpress.XtraGrid.Columns.GridColumn();
            colDay = new DevExpress.XtraGrid.Columns.GridColumn();
            colMonth2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colYear2 = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            colFactures4 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantFacture3 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantMaj3 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantFact3 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantOff3 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMarge4 = new DevExpress.XtraGrid.Columns.GridColumn();
            colBrute3 = new DevExpress.XtraGrid.Columns.GridColumn();
            tabDaily = new DevExpress.XtraTab.XtraTabPage();
            chartDaily = new ChartControl();
            tabProducts = new DevExpress.XtraTab.XtraTabPage();
            chartProducts = new ChartControl();
            repositoryItemRibbonSearchEdit1 = new DevExpress.XtraBars.Ribbon.Internal.RepositoryItemRibbonSearchEdit();
            colPA = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)statisticsBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fromDateRepo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fromDateRepo.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toDateRepo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toDateRepo.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit1.CalendarTimeProperties).BeginInit();
            tabBordereauxTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bordStatDtoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewBord).BeginInit();
            tabClientsTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridByClient).BeginInit();
            ((System.ComponentModel.ISupportInitialize)byClientStatBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewClients).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView2).BeginInit();
            tabMonthlyTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControl3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)monthlyStatBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewMonthly).BeginInit();
            tabProductTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridStatistics).BeginInit();
            ((System.ComponentModel.ISupportInitialize)productStatBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewProducts).BeginInit();
            tabClients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chrtCntrl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tabControl).BeginInit();
            tabControl.SuspendLayout();
            tabBordereaux.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartBordereaux).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagram1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)secondaryAxisy1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)secondaryAxisy2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView8).BeginInit();
            tabMonthly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartMonthly).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagram2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView9).BeginInit();
            tabWeeklyTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControl2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)weeklyStatBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewWeekly).BeginInit();
            tabWeekly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartWeekly).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagram3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series12).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series13).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView10).BeginInit();
            tabDailyTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControl4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dailyStatBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewDaily).BeginInit();
            tabDaily.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartDaily).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagram4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series14).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series15).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView12).BeginInit();
            tabProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartProducts).BeginInit();
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
            colPA.VisibleIndex = 7;
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
            colTPa.VisibleIndex = 8;
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
            colMontant1.VisibleIndex = 6;
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
            colQt1.VisibleIndex = 4;
            colQt1.Width = 84;
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
            FromDate.EditWidth = 150;
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
            ToDate.EditWidth = 150;
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
            ribbonControl1.Size = new Size(1285, 91);
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
            // tabBordereauxTable
            // 
            tabBordereauxTable.Controls.Add(gridControl1);
            tabBordereauxTable.ImageOptions.Image = (Image)resources.GetObject("tabBordereauxTable.ImageOptions.Image");
            tabBordereauxTable.Margin = new Padding(4, 3, 4, 3);
            tabBordereauxTable.Name = "tabBordereauxTable";
            tabBordereauxTable.Size = new Size(1120, 635);
            tabBordereauxTable.Text = "BORDEREAUX";
            // 
            // gridControl1
            // 
            gridControl1.DataSource = bordStatDtoBindingSource;
            gridControl1.Dock = DockStyle.Fill;
            gridControl1.Location = new Point(0, 0);
            gridControl1.MainView = viewBord;
            gridControl1.MenuManager = ribbonControl1;
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new Size(1120, 635);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new BaseView[] { viewBord });
            // 
            // bordStatDtoBindingSource
            // 
            bordStatDtoBindingSource.DataSource = typeof(DAL.Statistics.BordStatDto);
            // 
            // viewBord
            // 
            viewBord.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colDateDebut, colDateFin, colDate, colNum, colFactures2, colJours, colMontantJour, colFactureJour, colMontantFacture, colCenter, colMontantMaj, colMontantFact, colMontantOff, colMontantFE, colMontantGlobal, colVirement, colMarge1, colBrute, colNet, colEcart });
            viewBord.GridControl = gridControl1;
            viewBord.Name = "viewBord";
            viewBord.OptionsView.ShowFooter = true;
            // 
            // colDateDebut
            // 
            colDateDebut.FieldName = "DateDebut";
            colDateDebut.Name = "colDateDebut";
            // 
            // colDateFin
            // 
            colDateFin.FieldName = "DateFin";
            colDateFin.Name = "colDateFin";
            // 
            // colDate
            // 
            colDate.FieldName = "Date";
            colDate.Name = "colDate";
            colDate.OptionsColumn.ReadOnly = true;
            colDate.Visible = true;
            colDate.VisibleIndex = 2;
            colDate.Width = 61;
            // 
            // colNum
            // 
            colNum.FieldName = "Num";
            colNum.Name = "colNum";
            colNum.Visible = true;
            colNum.VisibleIndex = 1;
            colNum.Width = 61;
            // 
            // colFactures2
            // 
            colFactures2.DisplayFormat.FormatString = "N0";
            colFactures2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colFactures2.FieldName = "Factures";
            colFactures2.Name = "colFactures2";
            colFactures2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Factures", "{0:N}") });
            colFactures2.Visible = true;
            colFactures2.VisibleIndex = 3;
            colFactures2.Width = 61;
            // 
            // colJours
            // 
            colJours.DisplayFormat.FormatString = "N0";
            colJours.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colJours.FieldName = "Jours";
            colJours.Name = "colJours";
            colJours.OptionsColumn.ReadOnly = true;
            colJours.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Jours", "{0:N}") });
            colJours.Visible = true;
            colJours.VisibleIndex = 4;
            colJours.Width = 61;
            // 
            // colMontantJour
            // 
            colMontantJour.Caption = "Mnt J";
            colMontantJour.DisplayFormat.FormatString = "N2";
            colMontantJour.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantJour.FieldName = "MontantJour";
            colMontantJour.Name = "colMontantJour";
            colMontantJour.OptionsColumn.ReadOnly = true;
            colMontantJour.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantJour", "{0:N2}") });
            colMontantJour.Visible = true;
            colMontantJour.VisibleIndex = 5;
            colMontantJour.Width = 61;
            // 
            // colFactureJour
            // 
            colFactureJour.Caption = "Fact J";
            colFactureJour.DisplayFormat.FormatString = "n0";
            colFactureJour.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colFactureJour.FieldName = "FactureJour";
            colFactureJour.Name = "colFactureJour";
            colFactureJour.OptionsColumn.ReadOnly = true;
            colFactureJour.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "FactureJour", "{0:N2}") });
            colFactureJour.Visible = true;
            colFactureJour.VisibleIndex = 6;
            colFactureJour.Width = 61;
            // 
            // colMontantFacture
            // 
            colMontantFacture.Caption = "Moy Fac";
            colMontantFacture.DisplayFormat.FormatString = "N2";
            colMontantFacture.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantFacture.FieldName = "MontantFacture";
            colMontantFacture.Name = "colMontantFacture";
            colMontantFacture.OptionsColumn.ReadOnly = true;
            colMontantFacture.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantFacture", "{0:n2}") });
            colMontantFacture.Visible = true;
            colMontantFacture.VisibleIndex = 7;
            colMontantFacture.Width = 61;
            // 
            // colCenter
            // 
            colCenter.FieldName = "Center";
            colCenter.Name = "colCenter";
            colCenter.Visible = true;
            colCenter.VisibleIndex = 0;
            colCenter.Width = 61;
            // 
            // colMontantMaj
            // 
            colMontantMaj.Caption = "Maj";
            colMontantMaj.DisplayFormat.FormatString = "N2";
            colMontantMaj.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantMaj.FieldName = "MontantMaj";
            colMontantMaj.Name = "colMontantMaj";
            colMontantMaj.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantMaj", "{0:n2}") });
            colMontantMaj.Visible = true;
            colMontantMaj.VisibleIndex = 14;
            colMontantMaj.Width = 61;
            // 
            // colMontantFact
            // 
            colMontantFact.Caption = "Mnt Fac";
            colMontantFact.DisplayFormat.FormatString = "N2";
            colMontantFact.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantFact.FieldName = "MontantFact";
            colMontantFact.Name = "colMontantFact";
            colMontantFact.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantFact", "{0:n2}") });
            colMontantFact.Visible = true;
            colMontantFact.VisibleIndex = 8;
            colMontantFact.Width = 61;
            // 
            // colMontantOff
            // 
            colMontantOff.Caption = "Mnt Off";
            colMontantOff.DisplayFormat.FormatString = "N2";
            colMontantOff.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantOff.FieldName = "MontantOff";
            colMontantOff.Name = "colMontantOff";
            colMontantOff.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantOff", "{0:n2}") });
            colMontantOff.Visible = true;
            colMontantOff.VisibleIndex = 9;
            colMontantOff.Width = 65;
            // 
            // colMontantFE
            // 
            colMontantFE.Caption = "MFE";
            colMontantFE.DisplayFormat.FormatString = "N2";
            colMontantFE.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantFE.FieldName = "MontantFE";
            colMontantFE.Name = "colMontantFE";
            colMontantFE.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantFE", "{0:n2}") });
            colMontantFE.Visible = true;
            colMontantFE.VisibleIndex = 10;
            colMontantFE.Width = 60;
            // 
            // colMontantGlobal
            // 
            colMontantGlobal.Caption = "Global";
            colMontantGlobal.DisplayFormat.FormatString = "N2";
            colMontantGlobal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantGlobal.FieldName = "MontantGlobal";
            colMontantGlobal.Name = "colMontantGlobal";
            colMontantGlobal.OptionsColumn.ReadOnly = true;
            colMontantGlobal.Visible = true;
            colMontantGlobal.VisibleIndex = 11;
            colMontantGlobal.Width = 60;
            // 
            // colVirement
            // 
            colVirement.DisplayFormat.FormatString = "N2";
            colVirement.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colVirement.FieldName = "Virement";
            colVirement.Name = "colVirement";
            colVirement.Visible = true;
            colVirement.VisibleIndex = 12;
            colVirement.Width = 60;
            // 
            // colMarge1
            // 
            colMarge1.DisplayFormat.FormatString = "N2";
            colMarge1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMarge1.FieldName = "Marge";
            colMarge1.Name = "colMarge1";
            colMarge1.OptionsColumn.ReadOnly = true;
            colMarge1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Marge", "{0:n2}") });
            colMarge1.Visible = true;
            colMarge1.VisibleIndex = 13;
            colMarge1.Width = 60;
            // 
            // colBrute
            // 
            colBrute.DisplayFormat.FormatString = "N2";
            colBrute.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colBrute.FieldName = "Brute";
            colBrute.Name = "colBrute";
            colBrute.OptionsColumn.ReadOnly = true;
            colBrute.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Brute", "{0:n2}") });
            colBrute.Visible = true;
            colBrute.VisibleIndex = 15;
            colBrute.Width = 60;
            // 
            // colNet
            // 
            colNet.DisplayFormat.FormatString = "N2";
            colNet.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colNet.FieldName = "Net";
            colNet.Name = "colNet";
            colNet.OptionsColumn.ReadOnly = true;
            colNet.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Net", "{0:n2}") });
            colNet.Visible = true;
            colNet.VisibleIndex = 17;
            colNet.Width = 67;
            // 
            // colEcart
            // 
            colEcart.DisplayFormat.FormatString = "N2";
            colEcart.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colEcart.FieldName = "Ecart";
            colEcart.Name = "colEcart";
            colEcart.OptionsColumn.ReadOnly = true;
            colEcart.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Ecart", "{0:n2}") });
            colEcart.Visible = true;
            colEcart.VisibleIndex = 16;
            colEcart.Width = 60;
            // 
            // tabClientsTable
            // 
            tabClientsTable.Controls.Add(gridByClient);
            tabClientsTable.ImageOptions.Image = (Image)resources.GetObject("tabClientsTable.ImageOptions.Image");
            tabClientsTable.Margin = new Padding(4, 3, 4, 3);
            tabClientsTable.Name = "tabClientsTable";
            tabClientsTable.Size = new Size(1147, 618);
            tabClientsTable.Text = "CLIENTS";
            // 
            // gridByClient
            // 
            gridByClient.DataSource = byClientStatBindingSource;
            gridByClient.Dock = DockStyle.Fill;
            gridByClient.EmbeddedNavigator.Margin = new Padding(4, 3, 4, 3);
            gridByClient.Location = new Point(0, 0);
            gridByClient.MainView = viewClients;
            gridByClient.Margin = new Padding(4, 3, 4, 3);
            gridByClient.Name = "gridByClient";
            gridByClient.Size = new Size(1147, 618);
            gridByClient.TabIndex = 0;
            gridByClient.ViewCollection.AddRange(new BaseView[] { viewClients, gridView2 });
            // 
            // byClientStatBindingSource
            // 
            byClientStatBindingSource.DataSource = typeof(DAL.Statistics.ClientsStat);
            // 
            // viewClients
            // 
            viewClients.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colNumAssure, colAssure, colFactures, colMaj, colMantFact, colTR, colMarge, gridColumn1, gridColumn2 });
            viewClients.DetailHeight = 271;
            viewClients.GridControl = gridByClient;
            viewClients.Name = "viewClients";
            viewClients.OptionsEditForm.PopupEditFormWidth = 581;
            viewClients.OptionsView.ShowFooter = true;
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
            colFactures.DisplayFormat.FormatString = "n0";
            colFactures.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colFactures.FieldName = "Factures";
            colFactures.Name = "colFactures";
            colFactures.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Factures", "SUM={0:0.##}") });
            colFactures.Visible = true;
            colFactures.VisibleIndex = 2;
            colFactures.Width = 76;
            // 
            // colMaj
            // 
            colMaj.Caption = "Maj";
            colMaj.DisplayFormat.FormatString = "n";
            colMaj.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMaj.FieldName = "MontMaj";
            colMaj.Name = "colMaj";
            colMaj.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Maj", "SUM={0:n}") });
            colMaj.Visible = true;
            colMaj.VisibleIndex = 6;
            colMaj.Width = 76;
            // 
            // colMantFact
            // 
            colMantFact.AppearanceCell.BackColor = Color.FromArgb(255, 224, 192);
            colMantFact.AppearanceCell.Options.UseBackColor = true;
            colMantFact.DisplayFormat.FormatString = "n2";
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
            colTR.Caption = "TR";
            colTR.DisplayFormat.FormatString = "n";
            colTR.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colTR.FieldName = "MontAss";
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
            colMarge.VisibleIndex = 7;
            colMarge.Width = 76;
            // 
            // gridColumn1
            // 
            gridColumn1.DisplayFormat.FormatString = "n0";
            gridColumn1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridColumn1.FieldName = "Boites";
            gridColumn1.Name = "gridColumn1";
            gridColumn1.Visible = true;
            gridColumn1.VisibleIndex = 3;
            // 
            // gridColumn2
            // 
            gridColumn2.DisplayFormat.FormatString = "n2";
            gridColumn2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridColumn2.FieldName = "Brut";
            gridColumn2.Name = "gridColumn2";
            gridColumn2.Visible = true;
            gridColumn2.VisibleIndex = 8;
            // 
            // gridView2
            // 
            gridView2.DetailHeight = 271;
            gridView2.GridControl = gridByClient;
            gridView2.Name = "gridView2";
            gridView2.OptionsEditForm.PopupEditFormWidth = 581;
            // 
            // tabMonthlyTable
            // 
            tabMonthlyTable.Controls.Add(gridControl3);
            tabMonthlyTable.ImageOptions.Image = (Image)resources.GetObject("tabMonthlyTable.ImageOptions.Image");
            tabMonthlyTable.Margin = new Padding(4, 3, 4, 3);
            tabMonthlyTable.Name = "tabMonthlyTable";
            tabMonthlyTable.Size = new Size(1147, 618);
            tabMonthlyTable.Text = "MENSUELS";
            // 
            // gridControl3
            // 
            gridControl3.DataSource = monthlyStatBindingSource;
            gridControl3.Dock = DockStyle.Fill;
            gridControl3.Location = new Point(0, 0);
            gridControl3.MainView = viewMonthly;
            gridControl3.MenuManager = ribbonControl1;
            gridControl3.Name = "gridControl3";
            gridControl3.Size = new Size(1147, 618);
            gridControl3.TabIndex = 0;
            gridControl3.ViewCollection.AddRange(new BaseView[] { viewMonthly });
            // 
            // monthlyStatBindingSource
            // 
            monthlyStatBindingSource.DataSource = typeof(DAL.Statistics.MonthlyStat);
            // 
            // viewMonthly
            // 
            viewMonthly.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colDate2, colMonth, colYear, colFactures1, colJours1, colMontantJour1, colFactureJour1, colMontantFacture1, colMontantMaj1, colMontantFact1, colMontantOff1, colMarge2, colBrute1 });
            viewMonthly.GridControl = gridControl3;
            viewMonthly.Name = "viewMonthly";
            viewMonthly.OptionsView.ShowFooter = true;
            // 
            // colDate2
            // 
            colDate2.FieldName = "Date";
            colDate2.Name = "colDate2";
            colDate2.OptionsColumn.ReadOnly = true;
            colDate2.Visible = true;
            colDate2.VisibleIndex = 0;
            colDate2.Width = 104;
            // 
            // colMonth
            // 
            colMonth.FieldName = "Month";
            colMonth.Name = "colMonth";
            colMonth.OptionsColumn.ReadOnly = true;
            colMonth.Width = 73;
            // 
            // colYear
            // 
            colYear.FieldName = "Year";
            colYear.Name = "colYear";
            colYear.Width = 73;
            // 
            // colFactures1
            // 
            colFactures1.FieldName = "Factures";
            colFactures1.Name = "colFactures1";
            colFactures1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Factures", "{0:n0}") });
            colFactures1.Visible = true;
            colFactures1.VisibleIndex = 1;
            colFactures1.Width = 97;
            // 
            // colJours1
            // 
            colJours1.FieldName = "Jours";
            colJours1.Name = "colJours1";
            colJours1.OptionsColumn.ReadOnly = true;
            colJours1.Width = 97;
            // 
            // colMontantJour1
            // 
            colMontantJour1.Caption = "Mont / J";
            colMontantJour1.DisplayFormat.FormatString = "n";
            colMontantJour1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantJour1.FieldName = "MontantJour";
            colMontantJour1.Name = "colMontantJour1";
            colMontantJour1.OptionsColumn.ReadOnly = true;
            colMontantJour1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantJour", "{0:N2}") });
            colMontantJour1.Visible = true;
            colMontantJour1.VisibleIndex = 5;
            colMontantJour1.Width = 97;
            // 
            // colFactureJour1
            // 
            colFactureJour1.Caption = "Fact / J";
            colFactureJour1.FieldName = "FactureJour";
            colFactureJour1.Name = "colFactureJour1";
            colFactureJour1.OptionsColumn.ReadOnly = true;
            colFactureJour1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "FactureJour", "{0:n0}") });
            colFactureJour1.Visible = true;
            colFactureJour1.VisibleIndex = 2;
            colFactureJour1.Width = 97;
            // 
            // colMontantFacture1
            // 
            colMontantFacture1.Caption = "Mont / Fact";
            colMontantFacture1.DisplayFormat.FormatString = "n";
            colMontantFacture1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantFacture1.FieldName = "MontantFacture";
            colMontantFacture1.Name = "colMontantFacture1";
            colMontantFacture1.OptionsColumn.ReadOnly = true;
            colMontantFacture1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantFacture", "{0:N2}") });
            colMontantFacture1.Visible = true;
            colMontantFacture1.VisibleIndex = 4;
            colMontantFacture1.Width = 113;
            // 
            // colMontantMaj1
            // 
            colMontantMaj1.Caption = "Maj";
            colMontantMaj1.DisplayFormat.FormatString = "n";
            colMontantMaj1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantMaj1.FieldName = "MontantMaj";
            colMontantMaj1.Name = "colMontantMaj1";
            colMontantMaj1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantMaj", "{0:N2}") });
            colMontantMaj1.Visible = true;
            colMontantMaj1.VisibleIndex = 8;
            colMontantMaj1.Width = 93;
            // 
            // colMontantFact1
            // 
            colMontantFact1.DisplayFormat.FormatString = "n";
            colMontantFact1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantFact1.FieldName = "MontantFact";
            colMontantFact1.Name = "colMontantFact1";
            colMontantFact1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantFact", "{0:N2}") });
            colMontantFact1.Visible = true;
            colMontantFact1.VisibleIndex = 3;
            colMontantFact1.Width = 93;
            // 
            // colMontantOff1
            // 
            colMontantOff1.DisplayFormat.FormatString = "n";
            colMontantOff1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantOff1.FieldName = "MontantOff";
            colMontantOff1.Name = "colMontantOff1";
            colMontantOff1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantOff", "{0:N2}") });
            colMontantOff1.Visible = true;
            colMontantOff1.VisibleIndex = 6;
            colMontantOff1.Width = 93;
            // 
            // colMarge2
            // 
            colMarge2.DisplayFormat.FormatString = "n";
            colMarge2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMarge2.FieldName = "Marge";
            colMarge2.Name = "colMarge2";
            colMarge2.OptionsColumn.ReadOnly = true;
            colMarge2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Marge", "{0:N2}") });
            colMarge2.Visible = true;
            colMarge2.VisibleIndex = 7;
            colMarge2.Width = 93;
            // 
            // colBrute1
            // 
            colBrute1.DisplayFormat.FormatString = "n";
            colBrute1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colBrute1.FieldName = "Brute";
            colBrute1.Name = "colBrute1";
            colBrute1.OptionsColumn.ReadOnly = true;
            colBrute1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Brute", "{0:N2}") });
            colBrute1.Visible = true;
            colBrute1.VisibleIndex = 9;
            colBrute1.Width = 125;
            // 
            // tabProductTable
            // 
            tabProductTable.Controls.Add(gridStatistics);
            tabProductTable.ImageOptions.Image = (Image)resources.GetObject("tabProductTable.ImageOptions.Image");
            tabProductTable.Margin = new Padding(4, 3, 4, 3);
            tabProductTable.Name = "tabProductTable";
            tabProductTable.Size = new Size(1147, 618);
            tabProductTable.Text = "PROUITS";
            // 
            // gridStatistics
            // 
            gridStatistics.DataSource = productStatBindingSource;
            gridStatistics.Dock = DockStyle.Fill;
            gridStatistics.EmbeddedNavigator.Margin = new Padding(4, 3, 4, 3);
            gridStatistics.Location = new Point(0, 0);
            gridStatistics.MainView = viewProducts;
            gridStatistics.Margin = new Padding(4, 3, 4, 3);
            gridStatistics.Name = "gridStatistics";
            gridStatistics.Size = new Size(1147, 618);
            gridStatistics.TabIndex = 0;
            gridStatistics.ViewCollection.AddRange(new BaseView[] { viewProducts });
            // 
            // productStatBindingSource
            // 
            productStatBindingSource.DataSource = typeof(DAL.Statistics.ProductStat);
            // 
            // viewProducts
            // 
            viewProducts.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colCode1, colProduit1, colQt1, colPrix1, colDate1, colMontant1, colPA, colTPa, colGP, colCodeDCI, gridColumn3 });
            viewProducts.DetailHeight = 271;
            viewProducts.GridControl = gridStatistics;
            viewProducts.GroupCount = 1;
            viewProducts.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "colTPa", null, "[Sum: {0:n2}]"), new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Qt", null, ""), new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "colTPa", colTPa, "{0:n2}"), new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Montant", colMontant1, "{0:n2}"), new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Qt", colQt1, "") });
            viewProducts.Name = "viewProducts";
            viewProducts.OptionsBehavior.AutoExpandAllGroups = true;
            viewProducts.OptionsEditForm.EditFormColumnCount = 1;
            viewProducts.OptionsEditForm.PopupEditFormWidth = 581;
            viewProducts.OptionsSelection.MultiSelect = true;
            viewProducts.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            viewProducts.OptionsView.ShowFooter = true;
            viewProducts.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] { new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colCodeDCI, DevExpress.Data.ColumnSortOrder.Ascending) });
            // 
            // colCode1
            // 
            colCode1.FieldName = "NumEnr";
            colCode1.Name = "colCode1";
            colCode1.OptionsColumn.ReadOnly = true;
            colCode1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Code", "{0}") });
            colCode1.Visible = true;
            colCode1.VisibleIndex = 1;
            colCode1.Width = 168;
            // 
            // colProduit1
            // 
            colProduit1.FieldName = "Produits";
            colProduit1.Name = "colProduit1";
            colProduit1.OptionsColumn.ReadOnly = true;
            colProduit1.Visible = true;
            colProduit1.VisibleIndex = 3;
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
            colPrix1.VisibleIndex = 5;
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
            colGP.FieldName = "Local";
            colGP.Name = "colGP";
            colGP.Visible = true;
            colGP.VisibleIndex = 9;
            colGP.Width = 76;
            // 
            // colCodeDCI
            // 
            colCodeDCI.FieldName = "CodeDci";
            colCodeDCI.Name = "colCodeDCI";
            colCodeDCI.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "CodeDCI", "{0}"), new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "colTPa", "SUM={0:0.##}") });
            colCodeDCI.Visible = true;
            colCodeDCI.VisibleIndex = 2;
            colCodeDCI.Width = 92;
            // 
            // gridColumn3
            // 
            gridColumn3.FieldName = "Dci";
            gridColumn3.Name = "gridColumn3";
            gridColumn3.Visible = true;
            gridColumn3.VisibleIndex = 2;
            // 
            // tabClients
            // 
            tabClients.Controls.Add(chrtCntrl);
            tabClients.ImageOptions.Image = (Image)resources.GetObject("tabClients.ImageOptions.Image");
            tabClients.Name = "tabClients";
            tabClients.Size = new Size(1147, 618);
            tabClients.Text = "CLIENTS";
            // 
            // chrtCntrl
            // 
            chrtCntrl.Dock = DockStyle.Fill;
            chrtCntrl.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            chrtCntrl.Legend.AlignmentVertical = LegendAlignmentVertical.TopOutside;
            chrtCntrl.Legend.Name = "Default Legend";
            chrtCntrl.Location = new Point(0, 0);
            chrtCntrl.Name = "chrtCntrl";
            chrtCntrl.Size = new Size(1147, 618);
            chrtCntrl.TabIndex = 0;
            // 
            // tabControl
            // 
            tabControl.Dock = DockStyle.Fill;
            tabControl.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Left;
            tabControl.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
            tabControl.Location = new Point(0, 91);
            tabControl.Margin = new Padding(4, 3, 4, 3);
            tabControl.Name = "tabControl";
            tabControl.SelectedTabPage = tabBordereauxTable;
            tabControl.Size = new Size(1285, 641);
            tabControl.TabIndex = 1;
            tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] { tabBordereauxTable, tabBordereaux, tabMonthlyTable, tabMonthly, tabWeeklyTable, tabWeekly, tabDailyTable, tabDaily, tabProductTable, tabProducts, tabClientsTable, tabClients });
            tabControl.SelectedPageChanged += tabControl_SelectedPageChanged;
            // 
            // tabBordereaux
            // 
            tabBordereaux.Controls.Add(chartBordereaux);
            tabBordereaux.ImageOptions.Image = (Image)resources.GetObject("tabBordereaux.ImageOptions.Image");
            tabBordereaux.Name = "tabBordereaux";
            tabBordereaux.Size = new Size(1147, 618);
            tabBordereaux.Text = "BORDEREAUX";
            // 
            // chartBordereaux
            // 
            chartBordereaux.DataSource = bordStatDtoBindingSource;
            xyDiagram1.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1;0;2";
            xyDiagram1.AxisY.Title.Text = "Montant";
            xyDiagram1.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.Default;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagramPane1.Name = "Pane 1";
            xyDiagramPane1.PaneID = 0;
            xyDiagramPane2.Name = "Pane 2";
            xyDiagramPane2.PaneID = 1;
            xyDiagramPane2.Visibility = ChartElementVisibility.Hidden;
            xyDiagramPane3.Name = "Pane 3";
            xyDiagramPane3.PaneID = 2;
            xyDiagram1.Panes.AddRange(new XYDiagramPane[] { xyDiagramPane1, xyDiagramPane2, xyDiagramPane3 });
            secondaryAxisy1.Alignment = AxisAlignment.Near;
            secondaryAxisy1.AxisID = 0;
            secondaryAxisy1.Name = "Secondary AxisY 1";
            secondaryAxisy1.Title.Text = "Nomb Factures";
            secondaryAxisy1.Title.Visibility = DevExpress.Utils.DefaultBoolean.Default;
            secondaryAxisy1.VisibleInPanesSerializable = "0";
            secondaryAxisy2.Alignment = AxisAlignment.Near;
            secondaryAxisy2.AxisID = 2;
            secondaryAxisy2.Name = "Secondary AxisY 3";
            secondaryAxisy2.Title.Text = "Marge";
            secondaryAxisy2.Title.Visibility = DevExpress.Utils.DefaultBoolean.Default;
            secondaryAxisy2.VisibleInPanesSerializable = "2";
            xyDiagram1.SecondaryAxesY.AddRange(new SecondaryAxisY[] { secondaryAxisy1, secondaryAxisy2 });
            chartBordereaux.Diagram = xyDiagram1;
            chartBordereaux.Dock = DockStyle.Fill;
            chartBordereaux.Location = new Point(0, 0);
            chartBordereaux.Name = "chartBordereaux";
            series1.ArgumentDataMember = "Date";
            series1.Name = "Montant";
            series1.SeriesID = 0;
            series1.ToolTipHintDataMember = "Num";
            series1.ValueDataMembersSerializable = "MontantFact";
            series2.ArgumentDataMember = "Date";
            series2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series2.Name = "Factures";
            series2.SeriesID = 1;
            series2.ToolTipHintDataMember = "Num";
            series2.ValueDataMembersSerializable = "Factures";
            sideBySideBarSeriesView1.AxisYName = "Secondary AxisY 1";
            sideBySideBarSeriesView1.PaneName = "Pane 1";
            series2.View = sideBySideBarSeriesView1;
            series3.ArgumentDataMember = "Date";
            series3.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series3.Name = "Facture / J";
            series3.SeriesID = 4;
            series3.ValueDataMembersSerializable = "FactureJour";
            sideBySideBarSeriesView2.AxisYName = "Secondary AxisY 1";
            sideBySideBarSeriesView2.PaneName = "Pane 1";
            series3.View = sideBySideBarSeriesView2;
            series4.ArgumentDataMember = "Date";
            series4.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series4.Name = "Jours";
            series4.SeriesID = 2;
            series4.ToolTipHintDataMember = "Num";
            series4.ValueDataMembersSerializable = "Jours";
            sideBySideBarSeriesView3.AxisYName = "Secondary AxisY 1";
            sideBySideBarSeriesView3.PaneName = "Pane 1";
            series4.View = sideBySideBarSeriesView3;
            series5.ArgumentDataMember = "Date";
            series5.Name = "Marge";
            series5.SeriesID = 3;
            series5.ToolTipHintDataMember = "Num";
            series5.ValueDataMembersSerializable = "Marge";
            sideBySideBarSeriesView4.AxisYName = "Secondary AxisY 3";
            sideBySideBarSeriesView4.PaneName = "Pane 3";
            series5.View = sideBySideBarSeriesView4;
            series6.ArgumentDataMember = "Date";
            series6.Name = "Maj";
            series6.SeriesID = 5;
            series6.ValueDataMembersSerializable = "MontantMaj";
            sideBySideBarSeriesView5.AxisYName = "Secondary AxisY 3";
            sideBySideBarSeriesView5.PaneName = "Pane 3";
            series6.View = sideBySideBarSeriesView5;
            series7.ArgumentDataMember = "Date";
            series7.Name = "Ecart";
            series7.SeriesID = 8;
            series7.ValueDataMembersSerializable = "Ecart";
            sideBySideBarSeriesView6.AxisYName = "Secondary AxisY 3";
            sideBySideBarSeriesView6.PaneName = "Pane 3";
            series7.View = sideBySideBarSeriesView6;
            series8.ArgumentDataMember = "Date";
            series8.Name = "Brut";
            series8.SeriesID = 7;
            series8.ValueDataMembersSerializable = "Brute";
            sideBySideBarSeriesView7.AxisYName = "Secondary AxisY 3";
            sideBySideBarSeriesView7.PaneName = "Pane 3";
            series8.View = sideBySideBarSeriesView7;
            series9.ArgumentDataMember = "Date";
            series9.Name = "Net";
            series9.SeriesID = 6;
            series9.ValueDataMembersSerializable = "Net";
            sideBySideBarSeriesView8.AxisYName = "Secondary AxisY 3";
            sideBySideBarSeriesView8.PaneName = "Pane 3";
            series9.View = sideBySideBarSeriesView8;
            chartBordereaux.SeriesSerializable = new Series[]
    {
    series1,
    series2,
    series3,
    series4,
    series5,
    series6,
    series7,
    series8,
    series9
    };
            chartBordereaux.Size = new Size(1147, 618);
            chartBordereaux.TabIndex = 0;
            // 
            // tabMonthly
            // 
            tabMonthly.Controls.Add(chartMonthly);
            tabMonthly.ImageOptions.Image = (Image)resources.GetObject("tabMonthly.ImageOptions.Image");
            tabMonthly.Name = "tabMonthly";
            tabMonthly.Size = new Size(1147, 618);
            tabMonthly.Text = "MENSUELS";
            // 
            // chartMonthly
            // 
            xyDiagram2.AxisX.VisibleInPanesSerializable = "-1;0";
            xyDiagram2.AxisY.VisibleInPanesSerializable = "-1;0";
            xyDiagramPane4.Name = "Pane 1";
            xyDiagramPane4.PaneID = 0;
            xyDiagram2.Panes.AddRange(new XYDiagramPane[] { xyDiagramPane4 });
            chartMonthly.Diagram = xyDiagram2;
            chartMonthly.Dock = DockStyle.Fill;
            chartMonthly.Location = new Point(0, 0);
            chartMonthly.Name = "chartMonthly";
            series10.Name = "Series 1";
            series10.SeriesID = 0;
            series11.Name = "Series 2";
            series11.SeriesID = 1;
            sideBySideBarSeriesView9.PaneName = "Pane 1";
            series11.View = sideBySideBarSeriesView9;
            chartMonthly.SeriesSerializable = new Series[]
    {
    series10,
    series11
    };
            chartMonthly.Size = new Size(1147, 618);
            chartMonthly.TabIndex = 0;
            // 
            // tabWeeklyTable
            // 
            tabWeeklyTable.Controls.Add(gridControl2);
            tabWeeklyTable.ImageOptions.Image = (Image)resources.GetObject("tabWeeklyTable.ImageOptions.Image");
            tabWeeklyTable.Name = "tabWeeklyTable";
            tabWeeklyTable.Size = new Size(1147, 618);
            tabWeeklyTable.Text = "HEBDOMADAIRES";
            // 
            // gridControl2
            // 
            gridControl2.DataSource = weeklyStatBindingSource;
            gridControl2.Dock = DockStyle.Fill;
            gridControl2.Location = new Point(0, 0);
            gridControl2.MainView = viewWeekly;
            gridControl2.MenuManager = ribbonControl1;
            gridControl2.Name = "gridControl2";
            gridControl2.Size = new Size(1147, 618);
            gridControl2.TabIndex = 0;
            gridControl2.ViewCollection.AddRange(new BaseView[] { viewWeekly });
            // 
            // weeklyStatBindingSource
            // 
            weeklyStatBindingSource.DataSource = typeof(DAL.Statistics.WeeklyStat);
            // 
            // viewWeekly
            // 
            viewWeekly.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colDate3, colMonth1, colYear1, colDateDebut1, colDateFin1, colFactures3, colJours2, colMontantJour2, colFactureJour2, colMontantFacture2, colMontantMaj2, colMontantFact2, colMontantOff2, colMarge3, colBrute2 });
            viewWeekly.GridControl = gridControl2;
            viewWeekly.Name = "viewWeekly";
            viewWeekly.OptionsView.ShowFooter = true;
            // 
            // colDate3
            // 
            colDate3.FieldName = "Date";
            colDate3.Name = "colDate3";
            colDate3.OptionsColumn.ReadOnly = true;
            colDate3.Visible = true;
            colDate3.VisibleIndex = 0;
            // 
            // colMonth1
            // 
            colMonth1.FieldName = "Month";
            colMonth1.Name = "colMonth1";
            colMonth1.OptionsColumn.ReadOnly = true;
            colMonth1.Visible = true;
            colMonth1.VisibleIndex = 1;
            // 
            // colYear1
            // 
            colYear1.FieldName = "Year";
            colYear1.Name = "colYear1";
            colYear1.Visible = true;
            colYear1.VisibleIndex = 2;
            // 
            // colDateDebut1
            // 
            colDateDebut1.FieldName = "DateDebut";
            colDateDebut1.Name = "colDateDebut1";
            colDateDebut1.Visible = true;
            colDateDebut1.VisibleIndex = 3;
            // 
            // colDateFin1
            // 
            colDateFin1.FieldName = "DateFin";
            colDateFin1.Name = "colDateFin1";
            colDateFin1.Visible = true;
            colDateFin1.VisibleIndex = 4;
            // 
            // colFactures3
            // 
            colFactures3.DisplayFormat.FormatString = "n0";
            colFactures3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colFactures3.FieldName = "Factures";
            colFactures3.Name = "colFactures3";
            colFactures3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Factures", "{0:n0}") });
            colFactures3.Visible = true;
            colFactures3.VisibleIndex = 5;
            // 
            // colJours2
            // 
            colJours2.FieldName = "Jours";
            colJours2.Name = "colJours2";
            colJours2.OptionsColumn.ReadOnly = true;
            // 
            // colMontantJour2
            // 
            colMontantJour2.DisplayFormat.FormatString = "n2";
            colMontantJour2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantJour2.FieldName = "MontantJour";
            colMontantJour2.Name = "colMontantJour2";
            colMontantJour2.OptionsColumn.ReadOnly = true;
            colMontantJour2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantJour", "{0:n}") });
            colMontantJour2.Visible = true;
            colMontantJour2.VisibleIndex = 6;
            // 
            // colFactureJour2
            // 
            colFactureJour2.DisplayFormat.FormatString = "n0";
            colFactureJour2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colFactureJour2.FieldName = "FactureJour";
            colFactureJour2.Name = "colFactureJour2";
            colFactureJour2.OptionsColumn.ReadOnly = true;
            colFactureJour2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "FactureJour", "{0:n}") });
            colFactureJour2.Visible = true;
            colFactureJour2.VisibleIndex = 7;
            // 
            // colMontantFacture2
            // 
            colMontantFacture2.DisplayFormat.FormatString = "n2";
            colMontantFacture2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantFacture2.FieldName = "MontantFacture";
            colMontantFacture2.Name = "colMontantFacture2";
            colMontantFacture2.OptionsColumn.ReadOnly = true;
            colMontantFacture2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantFacture", "{0:n}") });
            colMontantFacture2.Visible = true;
            colMontantFacture2.VisibleIndex = 8;
            // 
            // colMontantMaj2
            // 
            colMontantMaj2.DisplayFormat.FormatString = "n2";
            colMontantMaj2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantMaj2.FieldName = "MontantMaj";
            colMontantMaj2.Name = "colMontantMaj2";
            colMontantMaj2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantMaj", "{0:n}") });
            colMontantMaj2.Visible = true;
            colMontantMaj2.VisibleIndex = 9;
            // 
            // colMontantFact2
            // 
            colMontantFact2.DisplayFormat.FormatString = "n2";
            colMontantFact2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantFact2.FieldName = "MontantFact";
            colMontantFact2.Name = "colMontantFact2";
            colMontantFact2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantFact", "{0:n}") });
            colMontantFact2.Visible = true;
            colMontantFact2.VisibleIndex = 10;
            // 
            // colMontantOff2
            // 
            colMontantOff2.DisplayFormat.FormatString = "n2";
            colMontantOff2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantOff2.FieldName = "MontantOff";
            colMontantOff2.Name = "colMontantOff2";
            colMontantOff2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantOff", "{0:n}") });
            colMontantOff2.Visible = true;
            colMontantOff2.VisibleIndex = 11;
            // 
            // colMarge3
            // 
            colMarge3.DisplayFormat.FormatString = "n2";
            colMarge3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMarge3.FieldName = "Marge";
            colMarge3.Name = "colMarge3";
            colMarge3.OptionsColumn.ReadOnly = true;
            colMarge3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Marge", "{0:n}") });
            colMarge3.Visible = true;
            colMarge3.VisibleIndex = 12;
            // 
            // colBrute2
            // 
            colBrute2.DisplayFormat.FormatString = "n2";
            colBrute2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colBrute2.FieldName = "Brute";
            colBrute2.Name = "colBrute2";
            colBrute2.OptionsColumn.ReadOnly = true;
            colBrute2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Brute", "{0:n}") });
            colBrute2.Visible = true;
            colBrute2.VisibleIndex = 13;
            // 
            // tabWeekly
            // 
            tabWeekly.Controls.Add(chartWeekly);
            tabWeekly.ImageOptions.Image = (Image)resources.GetObject("tabWeekly.ImageOptions.Image");
            tabWeekly.Name = "tabWeekly";
            tabWeekly.Size = new Size(1147, 618);
            tabWeekly.Text = "HEBDOMADAIRES";
            // 
            // chartWeekly
            // 
            xyDiagram3.AxisX.VisibleInPanesSerializable = "-1;0";
            xyDiagram3.AxisY.VisibleInPanesSerializable = "-1;0";
            xyDiagramPane5.Name = "Pane 1";
            xyDiagramPane5.PaneID = 0;
            xyDiagram3.Panes.AddRange(new XYDiagramPane[] { xyDiagramPane5 });
            chartWeekly.Diagram = xyDiagram3;
            chartWeekly.Dock = DockStyle.Fill;
            chartWeekly.Location = new Point(0, 0);
            chartWeekly.Name = "chartWeekly";
            series12.Name = "Series 1";
            series12.SeriesID = 2;
            series13.Name = "Series 2";
            series13.SeriesID = 3;
            sideBySideBarSeriesView10.PaneName = "Pane 1";
            series13.View = sideBySideBarSeriesView10;
            chartWeekly.SeriesSerializable = new Series[]
    {
    series12,
    series13
    };
            chartWeekly.Size = new Size(1147, 618);
            chartWeekly.TabIndex = 0;
            // 
            // tabDailyTable
            // 
            tabDailyTable.Controls.Add(gridControl4);
            tabDailyTable.ImageOptions.Image = (Image)resources.GetObject("tabDailyTable.ImageOptions.Image");
            tabDailyTable.Name = "tabDailyTable";
            tabDailyTable.Size = new Size(1120, 635);
            tabDailyTable.Text = "QUOTIDIENNES";
            // 
            // gridControl4
            // 
            gridControl4.DataSource = dailyStatBindingSource;
            gridControl4.Dock = DockStyle.Fill;
            gridControl4.Location = new Point(0, 0);
            gridControl4.MainView = viewDaily;
            gridControl4.MenuManager = ribbonControl1;
            gridControl4.Name = "gridControl4";
            gridControl4.Size = new Size(1120, 635);
            gridControl4.TabIndex = 0;
            gridControl4.ViewCollection.AddRange(new BaseView[] { viewDaily });
            // 
            // dailyStatBindingSource
            // 
            dailyStatBindingSource.DataSource = typeof(DAL.Statistics.DailyStat);
            // 
            // viewDaily
            // 
            viewDaily.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colDate4, colDay, colMonth2, colYear2, colDateTime, colFactures4, colMontantFacture3, colMontantMaj3, colMontantFact3, colMontantOff3, colMarge4, colBrute3 });
            viewDaily.GridControl = gridControl4;
            viewDaily.Name = "viewDaily";
            viewDaily.OptionsView.ShowFooter = true;
            // 
            // colDate4
            // 
            colDate4.FieldName = "Date";
            colDate4.Name = "colDate4";
            colDate4.OptionsColumn.ReadOnly = true;
            colDate4.Visible = true;
            colDate4.VisibleIndex = 0;
            // 
            // colDay
            // 
            colDay.FieldName = "Day";
            colDay.Name = "colDay";
            // 
            // colMonth2
            // 
            colMonth2.FieldName = "Month";
            colMonth2.Name = "colMonth2";
            // 
            // colYear2
            // 
            colYear2.FieldName = "Year";
            colYear2.Name = "colYear2";
            // 
            // colDateTime
            // 
            colDateTime.Caption = "Jour";
            colDateTime.FieldName = "Day";
            colDateTime.Name = "colDateTime";
            colDateTime.Visible = true;
            colDateTime.VisibleIndex = 1;
            // 
            // colFactures4
            // 
            colFactures4.FieldName = "Factures";
            colFactures4.Name = "colFactures4";
            colFactures4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Factures", "{0:n0}") });
            colFactures4.Visible = true;
            colFactures4.VisibleIndex = 2;
            // 
            // colMontantFacture3
            // 
            colMontantFacture3.Caption = "Mont/Fac";
            colMontantFacture3.DisplayFormat.FormatString = "n2";
            colMontantFacture3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantFacture3.FieldName = "MontantFacture";
            colMontantFacture3.Name = "colMontantFacture3";
            colMontantFacture3.OptionsColumn.ReadOnly = true;
            colMontantFacture3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantFacture", "{0:n2}") });
            colMontantFacture3.Visible = true;
            colMontantFacture3.VisibleIndex = 3;
            // 
            // colMontantMaj3
            // 
            colMontantMaj3.Caption = "Maj";
            colMontantMaj3.DisplayFormat.FormatString = "n2";
            colMontantMaj3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantMaj3.FieldName = "MontantMaj";
            colMontantMaj3.Name = "colMontantMaj3";
            colMontantMaj3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantMaj", "{0:n2}") });
            colMontantMaj3.Visible = true;
            colMontantMaj3.VisibleIndex = 7;
            // 
            // colMontantFact3
            // 
            colMontantFact3.DisplayFormat.FormatString = "n2";
            colMontantFact3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantFact3.FieldName = "MontantFact";
            colMontantFact3.Name = "colMontantFact3";
            colMontantFact3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantFact", "{0:n2}") });
            colMontantFact3.Visible = true;
            colMontantFact3.VisibleIndex = 4;
            // 
            // colMontantOff3
            // 
            colMontantOff3.DisplayFormat.FormatString = "n2";
            colMontantOff3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantOff3.FieldName = "MontantOff";
            colMontantOff3.Name = "colMontantOff3";
            colMontantOff3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "MontantOff", "{0:n2}") });
            colMontantOff3.Visible = true;
            colMontantOff3.VisibleIndex = 5;
            // 
            // colMarge4
            // 
            colMarge4.DisplayFormat.FormatString = "n2";
            colMarge4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMarge4.FieldName = "Marge";
            colMarge4.Name = "colMarge4";
            colMarge4.OptionsColumn.ReadOnly = true;
            colMarge4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Marge", "{0:n2}") });
            colMarge4.Visible = true;
            colMarge4.VisibleIndex = 6;
            // 
            // colBrute3
            // 
            colBrute3.DisplayFormat.FormatString = "n2";
            colBrute3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colBrute3.FieldName = "Brute";
            colBrute3.Name = "colBrute3";
            colBrute3.OptionsColumn.ReadOnly = true;
            colBrute3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, DevExpress.Data.SummaryMode.Mixed, "Brute", "{0:n2}") });
            colBrute3.Visible = true;
            colBrute3.VisibleIndex = 8;
            // 
            // tabDaily
            // 
            tabDaily.Controls.Add(chartDaily);
            tabDaily.ImageOptions.Image = (Image)resources.GetObject("tabDaily.ImageOptions.Image");
            tabDaily.Name = "tabDaily";
            tabDaily.Size = new Size(1147, 618);
            tabDaily.Text = "QUOTIDIENNES";
            // 
            // chartDaily
            // 
            xyDiagram4.AxisX.VisibleInPanesSerializable = "-1;0";
            xyDiagram4.AxisY.VisibleInPanesSerializable = "-1;0";
            xyDiagramPane6.Name = "Pane 1";
            xyDiagramPane6.PaneID = 0;
            xyDiagram4.Panes.AddRange(new XYDiagramPane[] { xyDiagramPane6 });
            chartDaily.Diagram = xyDiagram4;
            chartDaily.Dock = DockStyle.Fill;
            chartDaily.Location = new Point(0, 0);
            chartDaily.Name = "chartDaily";
            series14.Name = "Series 1";
            series14.SeriesID = 0;
            sideBySideBarSeriesView11.ColorEach = true;
            series14.View = sideBySideBarSeriesView11;
            series15.Name = "Series 2";
            series15.SeriesID = 1;
            sideBySideBarSeriesView12.PaneName = "Pane 1";
            series15.View = sideBySideBarSeriesView12;
            chartDaily.SeriesSerializable = new Series[]
    {
    series14,
    series15
    };
            chartDaily.Size = new Size(1147, 618);
            chartDaily.TabIndex = 0;
            // 
            // tabProducts
            // 
            tabProducts.Controls.Add(chartProducts);
            tabProducts.ImageOptions.Image = (Image)resources.GetObject("tabProducts.ImageOptions.Image");
            tabProducts.Name = "tabProducts";
            tabProducts.Size = new Size(1147, 618);
            tabProducts.Text = "PROUITS";
            // 
            // chartProducts
            // 
            chartProducts.Dock = DockStyle.Fill;
            chartProducts.Location = new Point(0, 0);
            chartProducts.Name = "chartProducts";
            chartProducts.Size = new Size(1147, 618);
            chartProducts.TabIndex = 0;
            // 
            // repositoryItemRibbonSearchEdit1
            // 
            repositoryItemRibbonSearchEdit1.AllowFocused = false;
            repositoryItemRibbonSearchEdit1.AutoHeight = false;
            repositoryItemRibbonSearchEdit1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            editorButtonImageOptions1.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            repositoryItemRibbonSearchEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, true, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default), new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear, "", -1, true, false, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default) });
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
            Size = new Size(1285, 732);
            Load += MovementsUc_Load;
            ((System.ComponentModel.ISupportInitialize)statisticsBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)fromDateRepo.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)fromDateRepo).EndInit();
            ((System.ComponentModel.ISupportInitialize)toDateRepo.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)toDateRepo).EndInit();
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit1.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit1).EndInit();
            tabBordereauxTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)bordStatDtoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewBord).EndInit();
            tabClientsTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridByClient).EndInit();
            ((System.ComponentModel.ISupportInitialize)byClientStatBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewClients).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView2).EndInit();
            tabMonthlyTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControl3).EndInit();
            ((System.ComponentModel.ISupportInitialize)monthlyStatBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewMonthly).EndInit();
            tabProductTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridStatistics).EndInit();
            ((System.ComponentModel.ISupportInitialize)productStatBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewProducts).EndInit();
            tabClients.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chrtCntrl).EndInit();
            ((System.ComponentModel.ISupportInitialize)tabControl).EndInit();
            tabControl.ResumeLayout(false);
            tabBordereaux.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane1).EndInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane2).EndInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane3).EndInit();
            ((System.ComponentModel.ISupportInitialize)secondaryAxisy1).EndInit();
            ((System.ComponentModel.ISupportInitialize)secondaryAxisy2).EndInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagram1).EndInit();
            ((System.ComponentModel.ISupportInitialize)series1).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)series2).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)series3).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView3).EndInit();
            ((System.ComponentModel.ISupportInitialize)series4).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView4).EndInit();
            ((System.ComponentModel.ISupportInitialize)series5).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView5).EndInit();
            ((System.ComponentModel.ISupportInitialize)series6).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView6).EndInit();
            ((System.ComponentModel.ISupportInitialize)series7).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView7).EndInit();
            ((System.ComponentModel.ISupportInitialize)series8).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView8).EndInit();
            ((System.ComponentModel.ISupportInitialize)series9).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartBordereaux).EndInit();
            tabMonthly.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane4).EndInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagram2).EndInit();
            ((System.ComponentModel.ISupportInitialize)series10).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView9).EndInit();
            ((System.ComponentModel.ISupportInitialize)series11).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartMonthly).EndInit();
            tabWeeklyTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControl2).EndInit();
            ((System.ComponentModel.ISupportInitialize)weeklyStatBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewWeekly).EndInit();
            tabWeekly.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane5).EndInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagram3).EndInit();
            ((System.ComponentModel.ISupportInitialize)series12).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView10).EndInit();
            ((System.ComponentModel.ISupportInitialize)series13).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartWeekly).EndInit();
            tabDailyTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControl4).EndInit();
            ((System.ComponentModel.ISupportInitialize)dailyStatBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewDaily).EndInit();
            tabDaily.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane6).EndInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagram4).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView11).EndInit();
            ((System.ComponentModel.ISupportInitialize)series14).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView12).EndInit();
            ((System.ComponentModel.ISupportInitialize)series15).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartDaily).EndInit();
            tabProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartProducts).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemRibbonSearchEdit1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private BindingSource statisticsBindingSource;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage3;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.BarButtonItem btnRefresh;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarEditItem FromDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit fromDateRepo;
        private DevExpress.XtraBars.BarEditItem ToDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit toDateRepo;
        private DevExpress.XtraBars.BarButtonItem btnClearDates;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraTab.XtraTabPage tabBordereauxTable;
        private DevExpress.XtraTab.XtraTabPage tabClientsTable;
        private DevExpress.XtraGrid.GridControl gridByClient;
        private DevExpress.XtraGrid.Views.Grid.GridView viewClients;
        private DevExpress.XtraGrid.Columns.GridColumn colNumAssure;
        private DevExpress.XtraGrid.Columns.GridColumn colAssure;
        private DevExpress.XtraGrid.Columns.GridColumn colFactures;
        private DevExpress.XtraGrid.Columns.GridColumn colMaj;
        private DevExpress.XtraGrid.Columns.GridColumn colMantFact;
        private DevExpress.XtraGrid.Columns.GridColumn colTR;
        private DevExpress.XtraGrid.Columns.GridColumn colMarge;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraTab.XtraTabPage tabMonthlyTable;
        private DevExpress.XtraGrid.GridControl gridCtrlMonthly;
        private DevExpress.XtraGrid.Views.Grid.GridView viewMonthly;
        private DevExpress.XtraGrid.Columns.GridColumn colMantant;
        private DevExpress.XtraGrid.Columns.GridColumn colBorderaux;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraTab.XtraTabPage tabProductTable;
        private DevExpress.XtraGrid.GridControl gridStatistics;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewStatistics;
        private DevExpress.XtraGrid.Columns.GridColumn colCode1;
        private DevExpress.XtraGrid.Columns.GridColumn colProduit1;
        private DevExpress.XtraGrid.Columns.GridColumn colQt1;
        private DevExpress.XtraGrid.Columns.GridColumn colPrix1;
        private DevExpress.XtraGrid.Columns.GridColumn colDate1;
        private DevExpress.XtraGrid.Columns.GridColumn colMontant1;
        private DevExpress.XtraGrid.Columns.GridColumn colTPa;
        private DevExpress.XtraTab.XtraTabPage tabClients;
        private ChartControl chrtCntrl;
        private DevExpress.XtraTab.XtraTabControl tabControl;
        private DevExpress.XtraGrid.Columns.GridColumn colGP;
        private DevExpress.XtraGrid.Columns.GridColumn colCodeDCI;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.Ribbon.Internal.RepositoryItemRibbonSearchEdit repositoryItemRibbonSearchEdit1;
        private DevExpress.XtraTab.XtraTabPage tabBordereaux;
        private DevExpress.XtraTab.XtraTabPage tabMonthly;
        private DevExpress.XtraTab.XtraTabPage tabWeekly;
        private DevExpress.XtraTab.XtraTabPage tabDaily;
        private ChartControl chartWeekly;
        private ChartControl chartBordereaux;
        private ChartControl chartMonthly;
        private ChartControl chartDaily;
        private BindingSource bordStatDtoBindingSource;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView viewBord;
        private DevExpress.XtraGrid.Columns.GridColumn colDateDebut;
        private DevExpress.XtraGrid.Columns.GridColumn colDateFin;
        private DevExpress.XtraGrid.Columns.GridColumn colDate;
        private DevExpress.XtraGrid.Columns.GridColumn colNum;
        private DevExpress.XtraGrid.Columns.GridColumn colFactures2;
        private DevExpress.XtraGrid.Columns.GridColumn colJours;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantJour;
        private DevExpress.XtraGrid.Columns.GridColumn colFactureJour;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantFacture;
        private DevExpress.XtraGrid.Columns.GridColumn colCenter;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantMaj;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantFact;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantOff;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantFE;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantGlobal;
        private DevExpress.XtraGrid.Columns.GridColumn colVirement;
        private DevExpress.XtraGrid.Columns.GridColumn colMarge1;
        private DevExpress.XtraGrid.Columns.GridColumn colBrute;
        private DevExpress.XtraGrid.Columns.GridColumn colNet;
        private DevExpress.XtraGrid.Columns.GridColumn colEcart;
        private DevExpress.XtraTab.XtraTabPage tabProducts;
        private ChartControl chartProducts;
        private DevExpress.XtraTab.XtraTabPage tabWeeklyTable;
        private DevExpress.XtraTab.XtraTabPage tabDailyTable;
        private BindingSource monthlyStatBindingSource;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView viewWeekly;
        private DevExpress.XtraGrid.GridControl gridControl3;
        private DevExpress.XtraGrid.Views.Grid.GridView viewProducts;
        private DevExpress.XtraGrid.GridControl gridControl4;
        private DevExpress.XtraGrid.Views.Grid.GridView viewDaily;
        private DevExpress.XtraGrid.Columns.GridColumn colDate2;
        private DevExpress.XtraGrid.Columns.GridColumn colMonth;
        private DevExpress.XtraGrid.Columns.GridColumn colYear;
        private DevExpress.XtraGrid.Columns.GridColumn colFactures1;
        private DevExpress.XtraGrid.Columns.GridColumn colJours1;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantJour1;
        private DevExpress.XtraGrid.Columns.GridColumn colFactureJour1;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantFacture1;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantMaj1;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantFact1;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantOff1;
        private DevExpress.XtraGrid.Columns.GridColumn colMarge2;
        private DevExpress.XtraGrid.Columns.GridColumn colBrute1;
        private BindingSource weeklyStatBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colDate3;
        private DevExpress.XtraGrid.Columns.GridColumn colMonth1;
        private DevExpress.XtraGrid.Columns.GridColumn colYear1;
        private DevExpress.XtraGrid.Columns.GridColumn colDateDebut1;
        private DevExpress.XtraGrid.Columns.GridColumn colDateFin1;
        private DevExpress.XtraGrid.Columns.GridColumn colFactures3;
        private DevExpress.XtraGrid.Columns.GridColumn colJours2;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantJour2;
        private DevExpress.XtraGrid.Columns.GridColumn colFactureJour2;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantFacture2;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantMaj2;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantFact2;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantOff2;
        private DevExpress.XtraGrid.Columns.GridColumn colMarge3;
        private DevExpress.XtraGrid.Columns.GridColumn colBrute2;
        private BindingSource dailyStatBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colDate4;
        private DevExpress.XtraGrid.Columns.GridColumn colDay;
        private DevExpress.XtraGrid.Columns.GridColumn colMonth2;
        private DevExpress.XtraGrid.Columns.GridColumn colYear2;
        private DevExpress.XtraGrid.Columns.GridColumn colDateTime;
        private DevExpress.XtraGrid.Columns.GridColumn colFactures4;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantFacture3;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantMaj3;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantFact3;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantOff3;
        private DevExpress.XtraGrid.Columns.GridColumn colMarge4;
        private DevExpress.XtraGrid.Columns.GridColumn colBrute3;
        private BindingSource byClientStatBindingSource;
        private BindingSource productStatBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    }
}
