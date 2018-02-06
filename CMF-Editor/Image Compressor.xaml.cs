using Microsoft.Win32;
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
using nQuant;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.IO;

namespace CMF_Editor
{
    /// <summary>
    /// Interaction logic for Image_Compressor.xaml
    /// </summary>
    public partial class Image_Compressor : Window
    {
        private BackgroundWorker worker;
        public Image_Compressor()
        {
            InitializeComponent();
            this.worker = new BackgroundWorker();
            this.worker.DoWork += Worker_DoWork;
            this.worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            this.worker.WorkerReportsProgress = false;
            this.worker.WorkerSupportsCancellation = true;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog imageSelect = new OpenFileDialog();
            imageSelect.Filter = "Image files (*.png)|*.png";
            if (imageSelect.ShowDialog(this) == true)
            {
                imageLocation.Text = imageSelect.FileName;
                labelStatus.Content = "IDLE";
                labelStatus.Foreground = new SolidColorBrush(Colors.Orange);
            }
        }

        private void imageLocation_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            labelStatus.Content = "IDLE";
            labelStatus.Foreground = new SolidColorBrush(Colors.Orange);
        }

        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog imageSave = new SaveFileDialog();
            imageSave.FileName = "Compressed Image";
            imageSave.DefaultExt = ".png";
            imageSave.Filter = "Image files (*.png)|*.png";
            if (imageSave.ShowDialog(this) == true)
            {
                labelStatus.Content = "WORKING";
                labelStatus.Foreground = new SolidColorBrush(Colors.DarkCyan);
                try
                {
                    var quantizer = new WuQuantizer();
                    using (FileStream fs = new FileStream(imageLocation.Text, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan))
                    using (var bitmap = new Bitmap(fs))
                    using (var quantized = quantizer.QuantizeImage(bitmap))
                    using (FileStream ofs = new FileStream(imageSave.FileName, FileMode.Create, FileAccess.Write, FileShare.Read, 1024, FileOptions.WriteThrough))
                    {
                        quantized.Save(ofs, ImageFormat.Png);
                        ofs.Flush();
                    }
                    labelStatus.Content = "FINISHED";
                    labelStatus.Foreground = new SolidColorBrush(Colors.Green);
                }
                catch (Exception ex)
                {
                    labelStatus.Content = "FAILED";
                    labelStatus.Foreground = new SolidColorBrush(Colors.Red);
                    MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }         
        }
    }
}
