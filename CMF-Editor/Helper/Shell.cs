using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace CMF_Editor.Helper
{
    static class Shell
    {
        public static void ShowPathInExplorer(string path)
        {
            ShowPathInExplorer(path, false);
        }

        public static void ShowPathInExplorer(string path, bool highlight)
        {
            if (highlight)
                Process.Start("explorer.exe", "/select, \"" + path + "\"");
            else
                Process.Start("explorer.exe", "\"" + path + "\"");
        }

        public class FileAssociation
        {
            public string Extension { get; set; }
            public string ProgId { get; set; }
            public string FileTypeDescription { get; set; }
            public string ExecutableFilePath { get; set; }
        }

        public class FileAssociations
        {
            // needed so that Explorer windows get refreshed after the registry is updated
            [System.Runtime.InteropServices.DllImport("Shell32.dll")]
            private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

            private const int SHCNE_ASSOCCHANGED = 0x8000000;
            private const int SHCNF_FLUSH = 0x1000;

            public static void EnsureAssociationsSet()
            {
                var filePath = AppInfo.ProccessPath;
                EnsureAssociationsSet(
                    new FileAssociation
                    {
                        Extension = ".ucs",
                        ProgId = "UCS_Editor_File",
                        FileTypeDescription = "UCS File",
                        ExecutableFilePath = filePath
                    });
            }
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="extension"></param>
            /// <param name="progId">No space allowed.</param>
            /// <param name="description"></param>
            public static void EnsureAssociationsSet(string extension, string progId, string description)
            {
                if (SetAssociation(extension, progId, description, AppInfo.ProccessPath))
                    SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
            }

            public static void EnsureAssociationsSet(params FileAssociation[] associations)
            {
                bool madeChanges = false;
                foreach (var association in associations)
                {
                    madeChanges |= SetAssociation(
                        association.Extension,
                        association.ProgId,
                        association.FileTypeDescription,
                        association.ExecutableFilePath);
                }

                if (madeChanges)
                {
                    SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
                }
            }

            public static bool SetAssociation(string extension, string progId, string fileTypeDescription, string applicationFilePath)
            {
                bool madeChanges = false;
                madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + extension, progId);
                madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + progId, fileTypeDescription);
                madeChanges |= SetKeyDefaultValue($@"Software\Classes\{progId}\shell\open\command", "\"" + applicationFilePath + "\" \"%1\"");
                return madeChanges;
            }

            private static bool SetKeyDefaultValue(string keyPath, string value)
            {
                using (var key = (AppInfo.IsElevated ? Registry.LocalMachine.CreateSubKey(keyPath) : Registry.CurrentUser.CreateSubKey(keyPath)))
                {
                    if (key.GetValue(null) as string != value)
                    {
                        key.SetValue(null, value);
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
