using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CHIFA.Server.Helpers;
public static  class LogHelper
{

    public static void Log(this Exception ex, bool showMessage = true, [CallerMemberName] string methodName = "",
        [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
    {



        Serilog.Log.Error(ex, "An error occurred in method {MethodName}, file {FileName}, line {LineNumber}", methodName, fileName, lineNumber);
        var result = ex.Message;

        if (ex is Npgsql.NpgsqlException pg && pg.Message.Contains("Failed to connect"))
        {
            //    XtraMessageBox.Show("Cannot connect to Server Run Server Or Check Settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (Debugger.IsAttached)
            result = $"Method: {methodName}\nFile:{fileName}\nLine:{lineNumber}\n{ex}";
        if (showMessage)
            MessageBox.Show(result, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
