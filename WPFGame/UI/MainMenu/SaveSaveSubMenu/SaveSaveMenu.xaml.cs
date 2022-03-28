using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WPFGame.UI.MainMenu.SaveSaveSubMenu
{
    /// <summary>
    /// Interaction logic for SaveSaveMenu.xaml
    /// </summary>
    public partial class SaveSaveMenu : UserControl
    {
        private readonly ProcessMenuBackButtonClick _backButtonAction;
        private readonly Uri _saveFolder;
        readonly ProcessSaveActionButtonClick _saveAction;

        private Uri _currentFilePath;

        public SaveSaveMenu(ProcessMenuBackButtonClick backButtonAction, 
            ProcessSaveActionButtonClick saveAction, Uri saveFolder)
        {
            InitializeComponent();
                        OverWriteBtn.IsEnabled = SaveListView.SelectedItem != null;
            _backButtonAction = backButtonAction;
            _saveFolder = saveFolder;
            _saveAction = saveAction;

            UpdateSaveList();
        }

        /// <summary>
        /// Updates the save list
        /// based on the contents
        /// of the save folder
        /// </summary>
        public void UpdateSaveList()
        {
            string[] filePaths = Directory.GetFiles(_saveFolder.ToString(), "*.save",
                 SearchOption.TopDirectoryOnly);
            SaveListView.ItemsSource = filePaths;
        }

        private void OnNewSaveClick(object sender, RoutedEventArgs e)
        {
            _currentFilePath = new Uri(_saveFolder.ToString() + $"/save{SaveListView.Items.Count}.save", UriKind.Relative);
            _saveAction.Invoke(_currentFilePath);
            UpdateSaveList();
        }

        private void OnOverWriteClick(object sender, RoutedEventArgs e)
        {
            _currentFilePath = new Uri(SaveListView.SelectedItem.ToString(), UriKind.Relative);
            _saveAction.Invoke(_currentFilePath);
            UpdateSaveList();
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            _backButtonAction.Invoke(this);
        }

        private void OnItemSelect(object sender, SelectionChangedEventArgs e)
        {
            OverWriteBtn.IsEnabled = SaveListView.SelectedItem != null;
        }
    }
}
