using Leayal.Closers.CMF;
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
using CMF_Editor.Classes;

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

        private CMFFile archive;

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public void ListViewColumnSortingSample()
        {
            // InitializeComponent();
            List<File> items = new List<File>();
            lvFiles.ItemsSource = items;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvFiles.ItemsSource);
            view.Filter = UserFilter;
        }

        #region "Control EventHandler"
        private void AppExit_Click(object sender, RoutedEventArgs e)
        {
            this.CloseArchive();
            this.Close();
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
                this.archive = new CMFFile(ofd.FileName);
                this.archive.Ready += this.Archive_Ready;
                this.archive.Closed += this.Archive_Closed;
            }
        }

        public void openExtraction()
        {
            Extraction extraction = new Extraction();
            extraction.Owner = App.Current.MainWindow;
            extraction.Show();
        }

        private void buttonExtract_Click(object sender, RoutedEventArgs e)
        {
            openExtraction();
            if (this.archive == null)
            {
                MessageBox.Show(this, "No CMF archive was opened. Please open a CMF archive to use this operation.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            openExtraction();
            //using (Leayal.Forms.FolderBrowseDialogEx folderBrowse = new Leayal.Forms.FolderBrowseDialogEx())
            //{
            //    folderBrowse.ShowNewFolderButton = true;
            //    folderBrowse.ShowTextBox = true;
            //    folderBrowse.Description = "Select destination folder";
            //    folderBrowse.OKButtonText = "Select";
            //    bool? result = folderBrowse.ShowDialog(this);
            //    if (result.HasValue && result.Value)
            //    {
            //        extractArchive(folderBrowse.SelectedDirectory);
            //    }
            //}
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            textBoxSearch.Clear();
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
#endregion

        #region "Methods"
        private void CloseArchive()
        {
            if (this.archive != null)
                this.archive.Dispose();
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

        public void extractArchive(string destination)
        {
            try
            {
                this.archive.ExtractAllEntries(destination);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region "Class EventHandler"
        private void Archive_Closed(object sender, EventArgs e)
        {
            lvFiles.ItemsSource = null;
            this.archive = null;
        }

        private void Archive_Ready(object sender, EventArgs e)
        {
            List<File> items = new List<File>(this.archive.FileCount);
            using (var reader = this.archive.ExtractAllEntries())
                while (reader.MoveToNextEntry())
                    items.Add(new File(reader.Entry));

            lvFiles.ItemsSource = items;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvFiles.ItemsSource);
            view.Filter = UserFilter;
        }
        #endregion
    }
}
