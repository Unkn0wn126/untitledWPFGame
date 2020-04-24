using System;
using System.Collections.Generic;
using System.IO;
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

namespace WPFGame.UI.MainMenu.SaveSaveSubMenu
{
    /// <summary>
    /// Interaction logic for SaveSaveMenu.xaml
    /// </summary>
    public partial class SaveSaveMenu : UserControl
    {
        private ProcessMenuBackButtonClick _backButtonAction;
        private Uri _saveFolder;
        ProcessSaveActionButtonClick _saveAction;

        private Uri _currentFilePath;
        public SaveSaveMenu(ProcessMenuBackButtonClick backButtonAction, ProcessSaveActionButtonClick saveAction, Uri saveFolder)
        {
            InitializeComponent();
            _backButtonAction = backButtonAction;
            _saveFolder = saveFolder;
            _saveAction = saveAction;

            UpdateSaveList();
        }

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
    }
}
