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

namespace CMF_Editor
{
    /// <summary>
    /// Interaction logic for Image_Compressor.xaml
    /// </summary>
    public partial class Image_Compressor : Window
    {
        public Image_Compressor()
        {
            InitializeComponent();
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog imageSelect = new OpenFileDialog();
            imageSelect.Filter = "Image files (*.png)|*.png";
            if (imageSelect.ShowDialog() == true)
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
            String savePath = "";
            if (imageSave.ShowDialog() == true)
            {
                labelStatus.Content = "WORKING";
                labelStatus.Foreground = new SolidColorBrush(Colors.DarkCyan);
                try {
                    savePath = imageSave.FileName;
                    var quantizer = new WuQuantizer();
                    using (var bitmap = new Bitmap(imageLocation.Text))
                    {
                        using (var quantized = quantizer.QuantizeImage(bitmap))
                        {
                            quantized.Save(savePath, ImageFormat.Png);
                        }
                    }
                    labelStatus.Content = "FINISHED";
                    labelStatus.Foreground = new SolidColorBrush(Colors.Green);
                }
                catch (Exception ex)
                {
                    labelStatus.Content = "FAILED";
                    labelStatus.Foreground = new SolidColorBrush(Colors.Red);
                }
            }         
        }
    }
}
