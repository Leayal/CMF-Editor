using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using Microsoft.VisualBasic.ApplicationServices;

namespace CMF_Editor.Helper
{
    static class AppInfo
    {
        private static AssemblyInfo _entryAssemblyInfo;
        public static AssemblyInfo EntryAssemblyInfo
        {
            get
            {
                if (_entryAssemblyInfo == null)
                    _entryAssemblyInfo = new AssemblyInfo(EntryAssembly);
                return _entryAssemblyInfo;
            }
        }

        private static Assembly _entryAssembly;
        public static Assembly EntryAssembly
        {
            get
            {
                if (_entryAssembly == null)
                    _entryAssembly = Assembly.GetEntryAssembly();
                return _entryAssembly;
            }
        }

        private static int _proccessID;
        public static int ProccessID
        {
            get
            {
                if (_proccessID == 0)
                    GetProcessInfo();
                return _proccessID;
            }
        }

        private static string _proccessPath;
        public static string ProccessPath
        {
            get
            {
                if (string.IsNullOrEmpty(_proccessPath))
                    GetProcessInfo();
                return _proccessPath;
            }
        }

        private static void GetProcessInfo()
        {
            using (Process proc = Process.GetCurrentProcess())
            {
                _proccessID = proc.Id;
                _proccessPath = proc.MainModule.FileName;
            }
        }

        private static readonly bool _isElevated = CheckElevated();
        private static bool CheckElevated()
        {
            bool result = false;
            using (var asd = WindowsIdentity.GetCurrent())
                result = asd.Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
            return result;
        }

        public static bool IsElevated => _isElevated;
    }
}
