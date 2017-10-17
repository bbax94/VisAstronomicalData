using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using VisAstronomicalData.Models;
using VisAstronomicalData.Store;
using VisAstronomicalData.ViewModels;

namespace VisAstronomicalData
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Survey Survey { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Survey = StoreFitsData.Survey;
            this.DataContext = Survey;
        }

        public void ImportFolderButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = this.FolderSelect();
            Survey = new Survey(filePath);
            string[] files = Directory.GetFiles(filePath, "*.fits", SearchOption.AllDirectories);
            Survey.ImportFolder(files);
            StoreFitsData.Survey = Survey;
            this.DataContext = Survey;
        }

        public void GenerateGraph(String molecule, int hdu, Query query)
        {
            StoreWindows.WindowPlot.UpdatePlot(hdu, molecule, query);
        }

        private string FolderSelect()
        {
            string filePath = "";

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    filePath = fbd.SelectedPath;
                }
            }

            return filePath;
        }
    }
}
