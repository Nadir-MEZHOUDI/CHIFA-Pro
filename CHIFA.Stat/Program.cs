global using System;
global using System.Collections.Generic;
global using System.Linq;

global using System.Windows.Forms;

global using DataModel;


namespace CHIFA.Stat
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {

            Velopack.VelopackApp.Build().Run();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ViewerForm1());
        }
    }
}
