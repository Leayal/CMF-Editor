using System.Windows;

namespace CMF_Editor.WinForms.Interop
{
    internal static class Helpers
    {
        // TODO: Remove Helpers class, refactor
        internal static Window GetDefaultOwnerWindow()
        {
            Window defaultWindow = null;

            // TODO: Detect active window and change to that instead
            if (Application.Current != null && Application.Current.MainWindow != null)
            {
                defaultWindow = Application.Current.MainWindow;
            }
            return defaultWindow;
        }

    }
}
