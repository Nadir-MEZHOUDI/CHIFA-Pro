namespace CHIFA.Pro.uc
{
    partial class HomeUc
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
            var tileItemElement1 = new TileItemElement();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeUc));
            var tileItemElement2 = new TileItemElement();
            var tileItemElement3 = new TileItemElement();
            var tileItemElement4 = new TileItemElement();
            var tileItemElement5 = new TileItemElement();
            var tileItemElement6 = new TileItemElement();
            var tileItemElement7 = new TileItemElement();
            var tileItemElement8 = new TileItemElement();
            var tileItemElement9 = new TileItemElement();
            var tileItemElement10 = new TileItemElement();
            var tileItemElement11 = new TileItemElement();
            var tileItemElement12 = new TileItemElement();
            var tileItemElement13 = new TileItemElement();
            var xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            var xyDiagramPane1 = new DevExpress.XtraCharts.XYDiagramPane();
            var xyDiagramPane2 = new DevExpress.XtraCharts.XYDiagramPane();
            var secondaryAxisy1 = new DevExpress.XtraCharts.SecondaryAxisY();
            var secondaryAxisy2 = new DevExpress.XtraCharts.SecondaryAxisY();
            var series1 = new DevExpress.XtraCharts.Series();
            var sideBySideBarSeriesLabel1 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            var sideBySideBarSeriesView1 = new DevExpress.XtraCharts.SideBySideBarSeriesView();
            var series2 = new DevExpress.XtraCharts.Series();
            var sideBySideBarSeriesLabel2 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            var sideBySideBarSeriesView2 = new DevExpress.XtraCharts.SideBySideBarSeriesView();
            var series3 = new DevExpress.XtraCharts.Series();
            var pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
            var areaSeriesView1 = new DevExpress.XtraCharts.AreaSeriesView();
            weekStatBindingSource = new BindingSource(components);
            tileControl1 = new TileControl();
            tileGroup1 = new TileGroup();
            itmFactures = new TileItem();
            itmAssures = new TileItem();
            itmBordereaux = new TileItem();
            itmStatistics = new TileItem();
            itmTraitSpec = new TileItem();
            itemSpecialetes = new TileItem();
            itmMedicaments = new TileItem();
            itemFormes = new TileItem();
            itemCenters = new TileItem();
            itemListNoir = new TileItem();
            itemUsers = new TileItem();
            itemControlMedical = new TileItem();
            itemOfficine = new TileItem();
            chartControl1 = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)weekStatBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagram1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)secondaryAxisy1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)secondaryAxisy2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesLabel1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesLabel2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)series3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pointSeriesLabel1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)areaSeriesView1).BeginInit();
            SuspendLayout();
            // 
            // weekStatBindingSource
            // 
            weekStatBindingSource.DataSource = typeof(DAL.Statistics.WeekStat);
            // 
            // tileControl1
            // 
            tileControl1.AllowItemHover = true;
            tileControl1.ColumnCount = 3;
            tileControl1.Dock = DockStyle.Left;
            tileControl1.Groups.Add(tileGroup1);
            tileControl1.ItemContentAnimation = TileItemContentAnimationType.ScrollTop;
            tileControl1.Location = new Point(0, 0);
            tileControl1.MaxId = 14;
            tileControl1.Name = "tileControl1";
            tileControl1.Orientation = Orientation.Vertical;
            tileControl1.Padding = new Padding(12, 10, 12, 10);
            tileControl1.Size = new Size(395, 552);
            tileControl1.TabIndex = 1;
            // 
            // tileGroup1
            // 
            tileGroup1.Items.Add(itmFactures);
            tileGroup1.Items.Add(itmAssures);
            tileGroup1.Items.Add(itmBordereaux);
            tileGroup1.Items.Add(itmStatistics);
            tileGroup1.Items.Add(itmTraitSpec);
            tileGroup1.Items.Add(itemSpecialetes);
            tileGroup1.Items.Add(itmMedicaments);
            tileGroup1.Items.Add(itemFormes);
            tileGroup1.Items.Add(itemCenters);
            tileGroup1.Items.Add(itemListNoir);
            tileGroup1.Items.Add(itemUsers);
            tileGroup1.Items.Add(itemControlMedical);
            tileGroup1.Items.Add(itemOfficine);
            tileGroup1.Name = "tileGroup1";
            tileGroup1.Text = "TABLEAUX";
            // 
            // itmFactures
            // 
            tileItemElement1.ImageOptions.Image = (Image)resources.GetObject("resource.Image");
            tileItemElement1.ImageOptions.ImageAlignment = TileItemContentAlignment.MiddleCenter;
            tileItemElement1.ImageOptions.ImageScaleMode = TileItemImageScaleMode.NoScale;
            tileItemElement1.ImageOptions.ImageToTextAlignment = TileControlImageToTextAlignment.None;
            tileItemElement1.Text = "FACTURES";
            itmFactures.Elements.Add(tileItemElement1);
            itmFactures.Id = 1;
            itmFactures.ItemSize = TileItemSize.Medium;
            itmFactures.Name = "itmFactures";
            itmFactures.ItemClick += itmFactures_ItemClick;
            // 
            // itmAssures
            // 
            tileItemElement2.ImageOptions.Image = (Image)resources.GetObject("resource.Image1");
            tileItemElement2.ImageOptions.ImageAlignment = TileItemContentAlignment.MiddleCenter;
            tileItemElement2.ImageOptions.ImageScaleMode = TileItemImageScaleMode.NoScale;
            tileItemElement2.Text = "ASSURES";
            itmAssures.Elements.Add(tileItemElement2);
            itmAssures.Id = 2;
            itmAssures.ItemSize = TileItemSize.Medium;
            itmAssures.Name = "itmAssures";
            itmAssures.ItemClick += itmAssures_ItemClick;
            // 
            // itmBordereaux
            // 
            tileItemElement3.ImageOptions.Image = (Image)resources.GetObject("resource.Image2");
            tileItemElement3.ImageOptions.ImageAlignment = TileItemContentAlignment.MiddleCenter;
            tileItemElement3.ImageOptions.ImageScaleMode = TileItemImageScaleMode.NoScale;
            tileItemElement3.Text = "BORDEREAUX";
            itmBordereaux.Elements.Add(tileItemElement3);
            itmBordereaux.Id = 4;
            itmBordereaux.ItemSize = TileItemSize.Medium;
            itmBordereaux.Name = "itmBordereaux";
            itmBordereaux.ItemClick += itmBordereaux_ItemClick;
            // 
            // itmStatistics
            // 
            tileItemElement4.ImageOptions.Image = (Image)resources.GetObject("resource.Image3");
            tileItemElement4.ImageOptions.ImageAlignment = TileItemContentAlignment.MiddleCenter;
            tileItemElement4.ImageOptions.ImageScaleMode = TileItemImageScaleMode.NoScale;
            tileItemElement4.Text = "STATISTICS";
            itmStatistics.Elements.Add(tileItemElement4);
            itmStatistics.Id = 5;
            itmStatistics.ItemSize = TileItemSize.Medium;
            itmStatistics.Name = "itmStatistics";
            itmStatistics.ItemClick += itmStatistics_ItemClick;
            // 
            // itmTraitSpec
            // 
            tileItemElement5.ImageOptions.Image = (Image)resources.GetObject("resource.Image4");
            tileItemElement5.ImageOptions.ImageAlignment = TileItemContentAlignment.MiddleCenter;
            tileItemElement5.ImageOptions.ImageScaleMode = TileItemImageScaleMode.NoScale;
            tileItemElement5.Text = "TRAIT SPEC";
            itmTraitSpec.Elements.Add(tileItemElement5);
            itmTraitSpec.Id = 0;
            itmTraitSpec.ItemSize = TileItemSize.Medium;
            itmTraitSpec.Name = "itmTraitSpec";
            itmTraitSpec.ItemClick += itmTraitSpec_ItemClick;
            // 
            // itemSpecialetes
            // 
            tileItemElement6.ImageOptions.Image = (Image)resources.GetObject("resource.Image5");
            tileItemElement6.Text = "SPECIALETES";
            itemSpecialetes.Elements.Add(tileItemElement6);
            itemSpecialetes.Id = 8;
            itemSpecialetes.ItemSize = TileItemSize.Medium;
            itemSpecialetes.Name = "itemSpecialetes";
            itemSpecialetes.ItemClick += itemSpecialetes_ItemClick;
            // 
            // itmMedicaments
            // 
            tileItemElement7.ImageOptions.Image = (Image)resources.GetObject("resource.Image6");
            tileItemElement7.ImageOptions.ImageAlignment = TileItemContentAlignment.MiddleCenter;
            tileItemElement7.ImageOptions.ImageScaleMode = TileItemImageScaleMode.NoScale;
            tileItemElement7.Text = "MEDICAMENTS";
            itmMedicaments.Elements.Add(tileItemElement7);
            itmMedicaments.Id = 3;
            itmMedicaments.ItemSize = TileItemSize.Medium;
            itmMedicaments.Name = "itmMedicaments";
            itmMedicaments.ItemClick += itmMedicaments_ItemClick;
            // 
            // itemFormes
            // 
            tileItemElement8.ImageOptions.Image = (Image)resources.GetObject("resource.Image7");
            tileItemElement8.Text = "FORMES";
            itemFormes.Elements.Add(tileItemElement8);
            itemFormes.Id = 7;
            itemFormes.ItemSize = TileItemSize.Medium;
            itemFormes.Name = "itemFormes";
            itemFormes.ItemClick += itemFormes_ItemClick;
            // 
            // itemCenters
            // 
            tileItemElement9.ImageOptions.Image = (Image)resources.GetObject("resource.Image8");
            tileItemElement9.Text = "CENTERS";
            itemCenters.Elements.Add(tileItemElement9);
            itemCenters.Id = 12;
            itemCenters.ItemSize = TileItemSize.Medium;
            itemCenters.Name = "itemCenters";
            itemCenters.ItemClick += itemCenters_ItemClick;
            // 
            // itemListNoir
            // 
            tileItemElement10.ImageOptions.Image = (Image)resources.GetObject("resource.Image9");
            tileItemElement10.Text = "LIST NOIR";
            itemListNoir.Elements.Add(tileItemElement10);
            itemListNoir.Id = 9;
            itemListNoir.ItemSize = TileItemSize.Medium;
            itemListNoir.Name = "itemListNoir";
            itemListNoir.ItemClick += itemListNoir_ItemClick;
            // 
            // itemUsers
            // 
            tileItemElement11.ImageOptions.Image = (Image)resources.GetObject("resource.Image10");
            tileItemElement11.Text = "USERS";
            itemUsers.Elements.Add(tileItemElement11);
            itemUsers.Id = 11;
            itemUsers.ItemSize = TileItemSize.Medium;
            itemUsers.Name = "itemUsers";
            itemUsers.ItemClick += itemUsers_ItemClick;
            // 
            // itemControlMedical
            // 
            tileItemElement12.ImageOptions.Image = (Image)resources.GetObject("resource.Image11");
            tileItemElement12.Text = "CONTROL MEDICAL";
            itemControlMedical.Elements.Add(tileItemElement12);
            itemControlMedical.Id = 10;
            itemControlMedical.ItemSize = TileItemSize.Medium;
            itemControlMedical.Name = "itemControlMedical";
            itemControlMedical.ItemClick += itemControlMedical_ItemClick;
            // 
            // itemOfficine
            // 
            tileItemElement13.ImageOptions.Image = (Image)resources.GetObject("resource.Image12");
            tileItemElement13.Text = "OFFICINE";
            itemOfficine.Elements.Add(tileItemElement13);
            itemOfficine.Id = 13;
            itemOfficine.ItemSize = TileItemSize.Medium;
            itemOfficine.Name = "itemOfficine";
            itemOfficine.ItemClick += itemOfficine_ItemClick;
            // 
            // chartControl1
            // 
            chartControl1.DataSource = weekStatBindingSource;
            xyDiagram1.AxisX.Label.TextPattern = "{A:dd/MM/yyyy}";
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1;0;1";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.DefaultPane.Title.Text = "Montant par jour";
            xyDiagram1.DefaultPane.Title.Visibility = DevExpress.Utils.DefaultBoolean.Default;
            xyDiagramPane1.Name = "Pane 1";
            xyDiagramPane1.PaneID = 0;
            xyDiagramPane1.Title.Text = "Nomber des Factures par Jour";
            xyDiagramPane1.Title.Visibility = DevExpress.Utils.DefaultBoolean.Default;
            xyDiagramPane2.Name = "Pane 2";
            xyDiagramPane2.PaneID = 1;
            xyDiagramPane2.Title.Text = "Majoration par Jour";
            xyDiagramPane2.Title.Visibility = DevExpress.Utils.DefaultBoolean.Default;
            xyDiagram1.Panes.AddRange(new DevExpress.XtraCharts.XYDiagramPane[] { xyDiagramPane1, xyDiagramPane2 });
            secondaryAxisy1.Alignment = DevExpress.XtraCharts.AxisAlignment.Near;
            secondaryAxisy1.AxisID = 0;
            secondaryAxisy1.Name = "Secondary AxisY 1";
            secondaryAxisy1.VisibleInPanesSerializable = "0";
            secondaryAxisy2.Alignment = DevExpress.XtraCharts.AxisAlignment.Near;
            secondaryAxisy2.AxisID = 1;
            secondaryAxisy2.Name = "Secondary AxisY 2";
            secondaryAxisy2.VisibleInPanesSerializable = "1";
            xyDiagram1.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SecondaryAxisY[] { secondaryAxisy1, secondaryAxisy2 });
            chartControl1.Diagram = xyDiagram1;
            chartControl1.Dock = DockStyle.Fill;
            chartControl1.Legend.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
            chartControl1.Legend.TextVisible = false;
            chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            chartControl1.Location = new Point(395, 0);
            chartControl1.Name = "chartControl1";
            series1.ArgumentDataMember = "Date";
            sideBySideBarSeriesLabel1.TextPattern = "{V:N2} : {HINT}";
            series1.Label = sideBySideBarSeriesLabel1;
            series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series1.Name = "Montant";
            series1.SeriesID = 0;
            series1.ShowInLegend = false;
            series1.ToolTipHintDataMember = "Day";
            series1.ValueDataMembersSerializable = "Montant";
            sideBySideBarSeriesView1.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
            sideBySideBarSeriesView1.ColorEach = true;
            sideBySideBarSeriesView1.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Gradient;
            series1.View = sideBySideBarSeriesView1;
            series2.ArgumentDataMember = "Date";
            sideBySideBarSeriesLabel2.TextPattern = "{V:N0} : {HINT}";
            series2.Label = sideBySideBarSeriesLabel2;
            series2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series2.Name = "Factures";
            series2.SeriesID = 6;
            series2.ShowInLegend = false;
            series2.ToolTipHintDataMember = "Day";
            series2.ValueDataMembersSerializable = "Count";
            sideBySideBarSeriesView2.AxisYName = "Secondary AxisY 1";
            sideBySideBarSeriesView2.ColorEach = true;
            sideBySideBarSeriesView2.PaneName = "Pane 1";
            series2.View = sideBySideBarSeriesView2;
            series3.ArgumentDataMember = "Date";
            pointSeriesLabel1.TextPattern = "{V:N2}";
            series3.Label = pointSeriesLabel1;
            series3.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series3.Name = "Majoration";
            series3.SeriesID = 5;
            series3.ShowInLegend = false;
            series3.ToolTipHintDataMember = "Day";
            series3.ValueDataMembersSerializable = "Maj";
            areaSeriesView1.AxisYName = "Secondary AxisY 2";
            areaSeriesView1.ColorEach = true;
            areaSeriesView1.EmptyPointOptions.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Solid;
            areaSeriesView1.PaneName = "Pane 2";
            series3.View = areaSeriesView1;
            chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[]
    {
    series1,
    series2,
    series3
    };
            chartControl1.Size = new Size(822, 552);
            chartControl1.TabIndex = 2;
            // 
            // HomeUc
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(chartControl1);
            Controls.Add(tileControl1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "HomeUc";
            Size = new Size(1217, 552);
            ((System.ComponentModel.ISupportInitialize)weekStatBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane1).EndInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagramPane2).EndInit();
            ((System.ComponentModel.ISupportInitialize)secondaryAxisy1).EndInit();
            ((System.ComponentModel.ISupportInitialize)secondaryAxisy2).EndInit();
            ((System.ComponentModel.ISupportInitialize)xyDiagram1).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesLabel1).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)series1).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesLabel2).EndInit();
            ((System.ComponentModel.ISupportInitialize)sideBySideBarSeriesView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)series2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pointSeriesLabel1).EndInit();
            ((System.ComponentModel.ISupportInitialize)areaSeriesView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)series3).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartControl1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private TileControl tileControl1;
        private TileGroup tileGroup1;
        private TileItem itmTraitSpec;
        private TileItem itmFactures;
        private TileItem itmAssures;
        private TileItem itmMedicaments;
        private TileItem itmBordereaux;
        private TileItem itmStatistics;
        private TileItem itemFormes;
        private TileItem itemSpecialetes;
        private TileItem itemListNoir;
        private TileItem itemControlMedical;
        private TileItem itemUsers;
        private TileItem itemCenters;
        private TileItem itemOfficine;
        private DevExpress.XtraCharts.ChartControl chartControl1;
        private BindingSource weekStatBindingSource;
    }
}
