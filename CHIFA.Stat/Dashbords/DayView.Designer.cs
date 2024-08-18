using CHIFA.Stat.ViewModels;

using DevExpress.XtraEditors;

using System.Drawing;
using System.Windows.Forms;

namespace CHIFA.Pro.Dashboards;

partial class DayView
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
        groupControl1 = new GroupControl();
        labelControl4 = new LabelControl();
        labelControl3 = new LabelControl();
        bindingSource1 = new BindingSource(components);
        labelControl2 = new LabelControl();
        labelControl1 = new LabelControl();
        ((System.ComponentModel.ISupportInitialize)groupControl1).BeginInit();
        groupControl1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
        SuspendLayout();
        // 
        // groupControl1
        // 
        groupControl1.AutoSize = true;
        groupControl1.Controls.Add(labelControl4);
        groupControl1.Controls.Add(labelControl3);
        groupControl1.Controls.Add(labelControl2);
        groupControl1.Controls.Add(labelControl1);
        groupControl1.Dock = DockStyle.Fill;
        groupControl1.Location = new Point(0, 0);
        groupControl1.Name = "groupControl1";
        groupControl1.Size = new Size(186, 196);
        groupControl1.TabIndex = 0;
        // 
        // labelControl4
        // 
        labelControl4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        labelControl4.Appearance.BackColor = Color.FromArgb(255, 255, 192);
        labelControl4.Appearance.Font = new Font("Lucida Sans Unicode", 12F, FontStyle.Bold, GraphicsUnit.Point);
        labelControl4.Appearance.Options.UseBackColor = true;
        labelControl4.Appearance.Options.UseFont = true;
        labelControl4.Appearance.Options.UseTextOptions = true;
        labelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        labelControl4.AutoSizeMode = LabelAutoSizeMode.Vertical;
        labelControl4.DataBindings.Add(new Binding("Text", bindingSource1, "MontAss", true));
        labelControl4.Location = new Point(5, 153);
        labelControl4.Name = "labelControl4";
        labelControl4.Padding = new Padding(5);
        labelControl4.Size = new Size(176, 33);
        labelControl4.TabIndex = 0;
        labelControl4.Text = "01/01/2023";
        // 
        // labelControl3
        // 
        labelControl3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        labelControl3.Appearance.BackColor = Color.Transparent;
        labelControl3.Appearance.Font = new Font("Bahnschrift SemiLight", 20F, FontStyle.Bold, GraphicsUnit.Point);
        labelControl3.Appearance.ForeColor = Color.Green;
        labelControl3.Appearance.Options.UseBackColor = true;
        labelControl3.Appearance.Options.UseFont = true;
        labelControl3.Appearance.Options.UseForeColor = true;
        labelControl3.Appearance.Options.UseTextOptions = true;
        labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        labelControl3.AutoSizeMode = LabelAutoSizeMode.Vertical;
        labelControl3.DataBindings.Add(new Binding("Text", bindingSource1, "Montant", true));
        labelControl3.Location = new Point(5, 105);
        labelControl3.Name = "labelControl3";
        labelControl3.Size = new Size(176, 40);
        labelControl3.TabIndex = 0;
        labelControl3.Text = "51 411.52";
        // 
        // bindingSource1
        // 
        bindingSource1.DataSource = typeof(DayViewModel);
        // 
        // labelControl2
        // 
        labelControl2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        labelControl2.Appearance.BackColor = Color.Transparent;
        labelControl2.Appearance.Font = new Font("Bahnschrift", 30F, FontStyle.Bold, GraphicsUnit.Point);
        labelControl2.Appearance.Options.UseBackColor = true;
        labelControl2.Appearance.Options.UseFont = true;
        labelControl2.Appearance.Options.UseTextOptions = true;
        labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        labelControl2.AutoSizeMode = LabelAutoSizeMode.Vertical;
        labelControl2.DataBindings.Add(new Binding("Text", bindingSource1, "NmbrOrd", true));
        labelControl2.Location = new Point(5, 40);
        labelControl2.Name = "labelControl2";
        labelControl2.Size = new Size(176, 60);
        labelControl2.TabIndex = 0;
        labelControl2.Text = "55";
        // 
        // labelControl1
        // 
        labelControl1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        labelControl1.Appearance.BackColor = Color.FromArgb(255, 224, 192);
        labelControl1.Appearance.Font = new Font("Lucida Sans Unicode", 12F, FontStyle.Bold, GraphicsUnit.Point);
        labelControl1.Appearance.Options.UseBackColor = true;
        labelControl1.Appearance.Options.UseBorderColor = true;
        labelControl1.Appearance.Options.UseFont = true;
        labelControl1.Appearance.Options.UseForeColor = true;
        labelControl1.Appearance.Options.UseTextOptions = true;
        labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        labelControl1.AutoSizeMode = LabelAutoSizeMode.Vertical;
        labelControl1.DataBindings.Add(new Binding("Text", bindingSource1, "Date", true));
        labelControl1.Location = new Point(5, 3);
        labelControl1.Name = "labelControl1";
        labelControl1.Padding = new Padding(5);
        labelControl1.Size = new Size(176, 33);
        labelControl1.TabIndex = 0;
        labelControl1.Text = "01/01/2023";
        // 
        // DayView
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(groupControl1);
        Name = "DayView";
        Size = new Size(186, 196);
        ((System.ComponentModel.ISupportInitialize)groupControl1).EndInit();
        groupControl1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupControl groupControl1;
    private LabelControl labelControl1;
    private BindingSource bindingSource1;
    private LabelControl labelControl4;
    private LabelControl labelControl3;
    private LabelControl labelControl2;
}
