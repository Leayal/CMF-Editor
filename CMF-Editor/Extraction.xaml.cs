using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace CMF_Editor
{
    /// <summary>
    /// Interaction logic for Extraction.xaml
    /// </summary>
    public partial class Extraction : Window
    {
        public Extraction()
        {
            InitializeComponent();
        }

        #region "Control EventHandler"
        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            using (Leayal.Forms.FolderBrowseDialogEx folderBrowse = new Leayal.Forms.FolderBrowseDialogEx())
            {
                folderBrowse.ShowNewFolderButton = true;
                folderBrowse.ShowTextBox = true;
                folderBrowse.Description = "Select destination folder";
                folderBrowse.OKButtonText = "Select";
                bool? result = folderBrowse.ShowDialog(this);
                if (result.HasValue && result.Value) textBoxDestination.Text = folderBrowse.SelectedDirectory;
            }
        }
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxDestination.Text != "")
            {
                try
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(MainWindow))
                        {
                            (window as MainWindow).extractArchive(textBoxDestination.Text);
                        }
                    }
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }               
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion


    }
}
