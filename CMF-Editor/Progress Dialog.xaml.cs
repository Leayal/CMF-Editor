using CMF_Editor.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

namespace CMF_Editor
{
    /// <summary>
    /// Interaction logic for Progress_Dialog.xaml
    /// </summary>
    public partial class Progress_Dialog : Window
    {
        private ExtractionParams myParams;
        // private TaskbarItemInfo myTaskbarItemInfo;
        public Progress_Dialog(ExtractionParams param, TaskbarItemInfo taskbarItemInfo)
        {
            this.myParams = param;
            // this.myTaskbarItemInfo = taskbarItemInfo;
            InitializeComponent();
            this.TaskbarItemInfo = taskbarItemInfo;
        }

        bool _shown;
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (_shown) return;
            _shown = true;

            // This has similiar effect for WinForm.OnShown

            if (string.IsNullOrWhiteSpace(this.myParams.Destination)) return;

            string outputFolder;
            if (this.myParams.ExtractionOptions.OptionToSubfolder)
            {
                System.IO.FileStream fs = this.myParams.Archive.Archive.BaseStream as System.IO.FileStream;
                if (fs != null)
                    outputFolder = System.IO.Path.Combine(System.IO.Path.GetFullPath(this.myParams.Destination), System.IO.Path.GetFileNameWithoutExtension(fs.Name) + "_files");
                else
                    outputFolder = System.IO.Path.Combine(System.IO.Path.GetFullPath(this.myParams.Destination), "OutputFolder_files");
            }
            else
                outputFolder = System.IO.Path.GetFullPath(this.myParams.Destination);
            System.IO.Directory.CreateDirectory(outputFolder);

            if (this.myParams.SelectedFiles.Count > 0)
            {
                List<string> filelist = new List<string>(this.myParams.SelectedFiles.Count);
                Classes.File filepointer;
                for (int i = 0; i < this.myParams.SelectedFiles.Count; i++)
                {
                    filepointer = (Classes.File)this.myParams.SelectedFiles[i];
                    filelist.Add(filepointer.Name);
                }

                if (this.myParams.ExtractionOptions.OptionContinueOnError)
                {
                    float currentfile, currenttotal = 0F;
                    byte[] buffer = new byte[4096];
                    string fullpath;
                    using (var reader = this.myParams.Archive.ExtractAllEntries())
                        while (reader.MoveToNextEntry())
                            if (filelist.Contains(reader.Entry.FileName, StringComparer.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    switch (this.myParams.ExtractionOptions.OptionFilePathType)
                                    {
                                        case Extraction.FilePathType.FlatPath:
                                            fullpath = Path.Combine(outputFolder, Path.GetFileName(reader.Entry.FileName));
                                            break;
                                        default:
                                            fullpath = Path.Combine(outputFolder, reader.Entry.FileName);
                                            break;
                                    }
                                    Microsoft.VisualBasic.FileIO.FileSystem.CreateDirectory(Path.GetDirectoryName(fullpath));

                                    currentfile = 0F;
                                    using (Stream contentStream = reader.OpenEntryStream())
                                    using (FileStream fs = System.IO.File.Create(fullpath))
                                    {
                                        int len = contentStream.Read(buffer, 0, buffer.Length);
                                        while (len > 0)
                                        {
                                            fs.Write(buffer, 0, len);
                                            currentfile += len;
                                            this.SetProgressBar2(currentfile / reader.Entry.CompressedSize);
                                            len = contentStream.Read(buffer, 0, buffer.Length);
                                        }
                                        fs.Flush();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                currenttotal += 1;
                                this.SetProgressBar1(currenttotal / this.myParams.Archive.Entries.Count);
                            }
                }
                else
                {
                    try
                    {
                        float currentfile, currenttotal = 0F;
                        byte[] buffer = new byte[4096];
                        string fullpath;

                        using (var reader = this.myParams.Archive.ExtractAllEntries())
                            while (reader.MoveToNextEntry())
                                if (filelist.Contains(reader.Entry.FileName, StringComparer.OrdinalIgnoreCase))
                                {
                                    switch (this.myParams.ExtractionOptions.OptionFilePathType)
                                    {
                                        case Extraction.FilePathType.FlatPath:
                                            fullpath = Path.Combine(outputFolder, Path.GetFileName(reader.Entry.FileName));
                                            break;
                                        default:
                                            fullpath = Path.Combine(outputFolder, reader.Entry.FileName);
                                            break;
                                    }
                                    Microsoft.VisualBasic.FileIO.FileSystem.CreateDirectory(Path.GetDirectoryName(fullpath));

                                    currentfile = 0F;
                                    using (Stream contentStream = reader.OpenEntryStream())
                                    using (FileStream fs = System.IO.File.Create(fullpath))
                                    {
                                        int len = contentStream.Read(buffer, 0, buffer.Length);
                                        while (len > 0)
                                        {
                                            fs.Write(buffer, 0, len);
                                            currentfile += len;
                                            this.SetProgressBar2(currentfile / reader.Entry.CompressedSize);
                                            len = contentStream.Read(buffer, 0, buffer.Length);
                                        }
                                        fs.Flush();
                                    }
                                    currenttotal += 1;
                                    this.SetProgressBar1(currenttotal / this.myParams.Archive.Entries.Count);
                                }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                if (this.myParams.ExtractionOptions.OptionContinueOnError)
                {
                    float currentfile, currenttotal = 0F;
                    byte[] buffer = new byte[4096];
                    string fullpath;
                    using (var reader = this.myParams.Archive.ExtractAllEntries())
                        while (reader.MoveToNextEntry())
                        {
                            try
                            {
                                switch (this.myParams.ExtractionOptions.OptionFilePathType)
                                {
                                    case Extraction.FilePathType.FlatPath:
                                        fullpath = Path.Combine(outputFolder, Path.GetFileName(reader.Entry.FileName));
                                        break;
                                    default:
                                        fullpath = Path.Combine(outputFolder, reader.Entry.FileName);
                                        break;
                                }
                                Microsoft.VisualBasic.FileIO.FileSystem.CreateDirectory(Path.GetDirectoryName(fullpath));

                                currentfile = 0F;
                                using (Stream contentStream = reader.OpenEntryStream())
                                using (FileStream fs = System.IO.File.Create(fullpath))
                                {
                                    int len = contentStream.Read(buffer, 0, buffer.Length);
                                    while (len > 0)
                                    {
                                        fs.Write(buffer, 0, len);
                                        currentfile += len;
                                        this.SetProgressBar2(currentfile / reader.Entry.CompressedSize);
                                        len = contentStream.Read(buffer, 0, buffer.Length);
                                    }
                                    fs.Flush();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            currenttotal += 1;
                            this.SetProgressBar1(currenttotal / this.myParams.Archive.Entries.Count);
                        }
                }
                else
                {
                    try
                    {
                        float currentfile, currenttotal = 0F;
                        byte[] buffer = new byte[4096];
                        string fullpath;
                        using (var reader = this.myParams.Archive.ExtractAllEntries())
                            while (reader.MoveToNextEntry())
                            {
                                switch (this.myParams.ExtractionOptions.OptionFilePathType)
                                {
                                    case Extraction.FilePathType.FlatPath:
                                        fullpath = Path.Combine(outputFolder, Path.GetFileName(reader.Entry.FileName));
                                        break;
                                    default:
                                        fullpath = Path.Combine(outputFolder, reader.Entry.FileName);
                                        break;
                                }
                                Microsoft.VisualBasic.FileIO.FileSystem.CreateDirectory(Path.GetDirectoryName(fullpath));

                                currentfile = 0F;
                                using (Stream contentStream = reader.OpenEntryStream())
                                using (FileStream fs = System.IO.File.Create(fullpath))
                                {
                                    int len = contentStream.Read(buffer, 0, buffer.Length);
                                    while (len > 0)
                                    {
                                        fs.Write(buffer, 0, len);
                                        currentfile += len;
                                        this.SetProgressBar2(currentfile / reader.Entry.CompressedSize);
                                        len = contentStream.Read(buffer, 0, buffer.Length);
                                    }
                                    fs.Flush();
                                }
                                currenttotal += 1;
                                this.SetProgressBar1(currenttotal / this.myParams.Archive.Entries.Count);
                            }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                if (this.myParams.ExtractionOptions.OptionDisplayFileAfterExtract)
                {
                    if (this.myParams.ExtractionOptions.OptionToSubfolder)
                        Helper.Shell.ShowPathInExplorer(this.myParams.Destination, true);
                    else
                        Helper.Shell.ShowPathInExplorer(this.myParams.Destination);
                }
            }

            this.Close();
        }

        private void SetProgressBar1(double progressVal)
        {

        }

        private void SetProgressBar2(double progressVal)
        {

        }
    }
}
