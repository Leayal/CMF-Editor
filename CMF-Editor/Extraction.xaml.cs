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
using CMF_Editor.WinForms;
using Microsoft.Win32;

namespace CMF_Editor
{
    /// <summary>
    /// Interaction logic for Extraction.xaml
    /// </summary>
    public partial class Extraction : Window
    {
        private Leayal.Closers.CMF.CMFArchive myArchive;

        public Extraction(Leayal.Closers.CMF.CMFArchive archive)
        {
            this.myArchive = archive;
            InitializeComponent();
            System.IO.FileStream fs = this.myArchive.BaseStream as System.IO.FileStream;
            if (fs != null)
                this.textBoxDestination.Text = $"{System.IO.Path.ChangeExtension(fs.Name, null)}_files";
        }

        #region "Control EventHandler"
        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            using (WpfFolderBrowserDialogEx folderBrowse = new WpfFolderBrowserDialogEx()
            {
                Title = "Select destination folder"
            })
                if (folderBrowse.ShowDialog(this) == true)
                    textBoxDestination.Text = folderBrowse.FileName;
        }
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxDestination.Text))
            {
                string outputFolder;
                if (this.checkBoxMisc1.IsChecked == true)
                {
                    System.IO.FileStream fs = this.myArchive.BaseStream as System.IO.FileStream;
                    if (fs != null)
                        outputFolder = System.IO.Path.Combine(System.IO.Path.GetFullPath(textBoxDestination.Text), System.IO.Path.GetFileNameWithoutExtension(fs.Name) + "_files");
                    else
                        outputFolder = System.IO.Path.Combine(System.IO.Path.GetFullPath(textBoxDestination.Text), "OutputFolder_files");
                }
                else
                    outputFolder = System.IO.Path.GetFullPath(textBoxDestination.Text);
                try
                {
                    System.IO.Directory.CreateDirectory(outputFolder);
                    this.myArchive.ExtractAllEntries(outputFolder);
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
