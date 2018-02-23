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

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void buttonOK_ClickOld(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxDestination.Text))
            {
                MessageBox.Show(this, "The destination path cannot be empty.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

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

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            this.OptionUpdateMode = this.GetUpdateMode();
            this.OptionOverwriteMode = this.GetOverwriteMode();
            this.OptionFilePathType = this.GetFilePathType();
            this.OptionToSubfolder = this.checkBoxMisc1.IsChecked;
            this.OptionDisplayFileAfterExtract = this.checkBoxMisc2.IsChecked;
            this.OptionContinueOnError = this.checkBoxMisc3.IsChecked;
            this.DialogResult = true;
        }
        #endregion

        public UpdateMode OptionUpdateMode { get; private set; }
        public OverwriteMode OptionOverwriteMode { get; private set; }
        public FilePathType OptionFilePathType { get; private set; }
        public bool? OptionToSubfolder { get; private set; }
        public bool? OptionDisplayFileAfterExtract { get; private set; }
        public bool? OptionContinueOnError { get; private set; }

        private UpdateMode GetUpdateMode()
        {
            if (radioUpdate2.IsChecked == true)
                return UpdateMode.ExtractExistingOnly;
            else if (radioUpdate3.IsChecked == true)
                return UpdateMode.ExtractNonExistingOnly;
            else
                return UpdateMode.ExtractAndReplace;
        }

        private OverwriteMode GetOverwriteMode()
        {
            if (radioOverwrite2.IsChecked == true)
                return OverwriteMode.Overwrite;
            else if (radioOverwrite3.IsChecked == true)
                return OverwriteMode.SkipExisting;
            else if (radioOverwrite4.IsChecked == true)
                return OverwriteMode.AutoRename;
            else
                return OverwriteMode.Prompt;
        }

        private FilePathType GetFilePathType()
        {
            if (radioFilePaths3.IsChecked == true)
                return FilePathType.FlatPath;
            else
                return FilePathType.RelativePath;
        }

        #region "Private declares"
        public enum UpdateMode : byte
        {
            ExtractAndReplace = 0,
            ExtractExistingOnly = 1 << 0,
            ExtractNonExistingOnly = 1 << 1
        }

        public enum OverwriteMode : byte
        {
            Prompt = 0,
            Overwrite = 1 << 0,
            SkipExisting = 1 << 1,
            AutoRename = 1 << 2
        }

        public enum FilePathType : byte
        {
            RelativePath = 0,
            FlatPath = 1 << 0
        }
        #endregion
    }
}
