using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

using VisAstronomicalData.Models;
using VisAstronomicalData.ViewModels;

namespace VisAstronomicalData
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImportFolderButton_Click(object sender, RoutedEventArgs e)
        {
            string[] files = this.FolderSelect();
            FitsInterpreter.InterpretFolders(files);
        }

// -------------------------------------------------------------------------------------------------------------------------------------------------------------
        private string[] FolderSelect()
        {
            string[] files = new string[0];

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    files = Directory.GetFiles(fbd.SelectedPath, "*.fits", SearchOption.AllDirectories);
                }
            }

            return files;
        }

        private void GenerateGraphButton_Click(object sender, RoutedEventArgs e)
        {
            //some value for which HDU
            //some value for which files

            int hduSelect = 19;
            FitsInterpreter.GenerateGraph(hduSelect);
        }
    }
}
