using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WPFGame.UI.MainMenu.LoadSaveSubMenu
{
    /// <summary>
    /// Interaction logic for LoadSaveMenu.xaml
    /// </summary>
    public partial class LoadSaveMenu : UserControl
    {
        private readonly Uri _saveFolder;
        private readonly ProcessMenuBackButtonClick _backButtonAction;
        private readonly ProcessSaveActionButtonClick _loadAction;

        private Uri _currentFilePath;

        public LoadSaveMenu(ProcessMenuBackButtonClick backButtonAction, ProcessSaveActionButtonClick loadAction, Uri savesPath)
        {
            InitializeComponent();
            LoadBtn.IsEnabled = SaveListView.SelectedItem != null;
            _backButtonAction = backButtonAction;
            _loadAction = loadAction;
            _saveFolder = savesPath;
            UpdateSaveList();
        }

        /// <summary>
        /// Updates the save file list
        /// based on the contents of
        /// the saves folder
        /// </summary>
        public void UpdateSaveList()
        {
            string[] filePaths = Directory.GetFiles(_saveFolder.ToString(), "*.save",
                 SearchOption.TopDirectoryOnly);
            SaveListView.ItemsSource = filePaths;
        }

        private void OnLoadClick(object sender, RoutedEventArgs e)
        {
            _currentFilePath = new Uri(SaveListView.SelectedItem.ToString(), UriKind.Relative);
            _loadAction.Invoke(_currentFilePath);
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            _backButtonAction.Invoke(this);
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadBtn.IsEnabled = SaveListView.SelectedItem != null;
        }
    }
}
