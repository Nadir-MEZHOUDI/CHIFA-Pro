using Microsoft.Reporting.WinForms;

namespace CHIFA.Pro.Reports;

public partial class FacturePreviewForm : Form
{
    public ReportViewer reportViewer1;
    private Button btnClose;
    private System.ComponentModel.IContainer? components;

    public FacturePreviewForm()
    {
        InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        reportViewer1 = new ReportViewer();
        btnClose = new Button();
        SuspendLayout();
        // 
        // reportViewer1
        // 
        reportViewer1.Dock = DockStyle.Fill;
        reportViewer1.Location = new Point(0, 0);
        reportViewer1.Name = "reportViewer1";
        reportViewer1.Size = new Size(933, 519);
        reportViewer1.TabIndex = 0;
        // 
        // btnClose
        // 
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.Location = new Point(12, 12);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(75, 23);
        btnClose.TabIndex = 1;
        btnClose.Text = "Close";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Visible = false;
        // 
        // FacturePreviewForm
        // 
        AutoScaleMode = AutoScaleMode.None;
        CancelButton = btnClose;
        ClientSize = new Size(933, 519);
        Controls.Add(reportViewer1);
        Controls.Add(btnClose);
        Name = "FacturePreviewForm";
        ShowIcon = false;
        Text = "Aperçu - Report Viewer";
        WindowState = FormWindowState.Maximized;
        Load += FacturePreviewForm_Load;
        ResumeLayout(false);
    }

    private void FacturePreviewForm_Load(object? sender, EventArgs e)
    {
        reportViewer1.RefreshReport();
    }
}
