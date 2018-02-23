using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic.ApplicationServices;

namespace CMF_Editor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            AppController controller = new AppController();
            controller.Run(Environment.GetCommandLineArgs());
        }

        private static Dictionary<string, Assembly> myDict;

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (myDict == null)
                myDict = new Dictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);
            string RealName = args.Name.Split(',')[0].Trim();
            if (myDict.ContainsKey(RealName))
                return myDict[RealName];
            else
            {
                byte[] bytes;
                string resourceName = "CMF_Editor.Dlls." + RealName + ".dll";
                Assembly currentAssembly = Assembly.GetExecutingAssembly();
                using (Stream stream = currentAssembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                        return null;
                    bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                }
                Assembly result = Assembly.Load(bytes);
                myDict.Add(RealName, result);
                bytes = null;
                return result;
            }
        }
    }

    class AppController : WindowsFormsApplicationBase
    {
        public AppController() : base(AuthenticationMode.Windows)
        {
            this.IsSingleInstance = false;
            this.EnableVisualStyles = true;
            this.ShutdownStyle = ShutdownMode.AfterMainFormCloses;
        }

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            App app = new App();
            MainWindow mainwindow = new MainWindow();
            app.MainWindow = mainwindow;
#if DEBUG
            app.Run(mainwindow);
#else
            try
            {
                app.Run(mainwindow);
            }
            catch (Exception ex)
            {
                System.Environment.ExitCode = 2;
                if (app != null)
                    System.Windows.MessageBox.Show(mainwindow, ex.ToString(), "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
#endif
            return false;
            // return base.OnStartup(eventArgs);
        }
    }
}
