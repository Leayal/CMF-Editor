using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace CMF_Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (e.Args.Length > 0)
            {
                MainWindow window = this.MainWindow as MainWindow;
                if (window != null)
                {
                    string shelllOpen = e.Args[0];
                    if (shelllOpen.EndsWith(".cmf", StringComparison.OrdinalIgnoreCase) && System.IO.File.Exists(shelllOpen))
                        window.OpenArchive(shelllOpen);
                }
            }
            
        }

        protected override void OnExit(ExitEventArgs e)
        {
            MainWindow window = this.MainWindow as MainWindow;
            if (window != null)
            {
                window.CloseArchive();
            }
            base.OnExit(e);
        }
    }
}
