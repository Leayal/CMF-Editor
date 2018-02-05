﻿using Leayal.Closers.CMF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace CMF_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            searchType.Items.Add("Name");
            searchType.Items.Add("Size");
            searchType.Items.Add("Type");
            searchType.SelectionChanged += searchType_SelectionChanged;
        }

        private void AppExit_Click(object sender, RoutedEventArgs e)
        {
            this.CloseArchive();
            this.Close();
        }

        private CMFArchive archive;

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public void ListViewColumnSortingSample()
        {
            // InitializeComponent();
            List<File> items = new List<File>();
            items.Add(new File() { Name = "123", Size = 1, Type = "PNG File" });
            items.Add(new File() { Name = "abc", Size = 654654, Type = "BMP File" });
            items.Add(new File() { Name = "abc", Size = 2, Type = "Lua File" });
            items.Add(new File() { Name = "1234qwer", Size = 148862, Type = "3D Model" });
            items.Add(new File() { Name = "abc456", Size = 99999999, Type = "Music" });
            lvFiles.ItemsSource = items;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvFiles.ItemsSource);
            view.Filter = UserFilter;
        }

        private void lvFilesColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvFiles.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvFiles.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select a file to open";
            ofd.RestoreDirectory = true;
            ofd.DefaultExt = "cmf";
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            ofd.Filter = "Closers CMF|*.cmf";
            ofd.Multiselect = false;
            bool? result = ofd.ShowDialog(this);
            if (result.HasValue && result.Value)
            {
                this.CloseArchive();
                this.archive = CMFArchive.Read(ofd.FileName);
                List<File> items = new List<File>(this.archive.FileCount);
                using (var reader = this.archive.ExtractAllEntries())
                    while (reader.MoveToNextEntry())
                        items.Add(new File() { Name = reader.Entry.FileName, Size = reader.Entry.UnpackedSize });

                lvFiles.ItemsSource = items;
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvFiles.ItemsSource);
                view.Filter = UserFilter;
            }
        }

        private void buttonExtract_Click(object sender, RoutedEventArgs e)
        {
            if (this.archive == null) return;

            using (Leayal.Forms.FolderBrowseDialogEx folderBrowse = new Leayal.Forms.FolderBrowseDialogEx())
            {
                folderBrowse.ShowNewFolderButton = true;
                folderBrowse.ShowTextBox = true;
                folderBrowse.Description = "Select destination folder";
                folderBrowse.OKButtonText = "Select";
                bool? result = folderBrowse.ShowDialog(this);
                if (result.HasValue && result.Value)
                {
                    try
                    {
                        this.archive.ExtractAllEntries(folderBrowse.SelectedDirectory);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void CloseArchive()
        {
            if (this.archive != null)
            {
                this.archive.Dispose();
                this.archive = null;
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            textBoxSearch.Clear();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(textBoxSearch.Text))
                return true;
            else
            {
                if (searchType.SelectedIndex == 0) return ((item as File).Name.IndexOf(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                if (searchType.SelectedIndex == 1) return ((item as File).Size.ToString().IndexOf(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                if (searchType.SelectedIndex == 2) return ((item as File).Type.IndexOf(textBoxSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                return true;
            }
                
        }

        private void textBoxSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (lvFiles.ItemsSource != null) CollectionViewSource.GetDefaultView(lvFiles.ItemsSource).Refresh();
        }

        private void searchType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvFiles.ItemsSource != null) CollectionViewSource.GetDefaultView(lvFiles.ItemsSource).Refresh();
        }

        public void openImageCompressor()
        {
            Image_Compressor imageConpressor = new Image_Compressor();
            imageConpressor.Owner = App.Current.MainWindow;
            imageConpressor.Show();
        }
        private void buttonImageCompressor_Click(object sender, RoutedEventArgs e)
        {
            openImageCompressor();
        }
    }

    public class File
    {
        public string Name { get; set; }

        public long Size { get; set; }

        public string Type { get; set; }

        public int Icon { get; set; }
    }

    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry =
                Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

        private static Geometry descGeometry =
                Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir)
                : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
                    (
                            AdornedElement.RenderSize.Width - 15,
                            (AdornedElement.RenderSize.Height - 5) / 2
                    );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }

}
