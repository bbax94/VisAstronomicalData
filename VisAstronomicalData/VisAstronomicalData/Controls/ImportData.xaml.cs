﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisAstronomicalData.Store;

namespace VisAstronomicalData
{
    /// <summary>
    /// Interaction logic for ImportData.xaml
    /// </summary>
    public partial class ImportData : UserControl
    {

        public ImportData()
        {
            InitializeComponent();
        }

        private void ImportFolder_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.ImportFolderButton_Click(sender, e);
        }
    }
}
