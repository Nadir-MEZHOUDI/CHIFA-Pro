using DevExpress.XtraEditors;

using Velopack;

namespace CHIFA.Stat
{
    public partial class ViewerForm1 : XtraForm
    {
        public ViewerForm1()
        {
            InitializeComponent();
        }

        private async void ViewerForm1_Load(object sender, EventArgs e)
        {
            await  UpdateAppAsync();

        }
        private async Task UpdateAppAsync()
        {
            var mgr = new UpdateManager("https://nadirsmartapp.blob.core.windows.net/chifa-stat");

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
                return; // no update available

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);

        }
    }
}
