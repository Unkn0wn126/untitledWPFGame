﻿using System;
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

namespace WPFGame.UI.MainMenu.LoadSaveSubMenu
{
    /// <summary>
    /// Interaction logic for LoadSaveMenu.xaml
    /// </summary>
    public partial class LoadSaveMenu : UserControl
    {
        private Uri _saveFolder;
        private ProcessMenuBackButtonClick _backButtonAction;
        private ProcessSaveActionButtonClick _loadAction;

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
