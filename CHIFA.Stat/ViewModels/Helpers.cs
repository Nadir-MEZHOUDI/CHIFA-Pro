using DevExpress.XtraEditors;

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CHIFA.Stat.ViewModels;

public static class Helpers
{
    public static void Log(this Exception ex, [CallerMemberName] string methodName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
    {
        var result = ex.Message;
        if (Debugger.IsAttached)
        {
            result = $"Method: {methodName}\nFile:{fileName}\nLine:{lineNumber}\n{ex}";
        }
        XtraMessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}