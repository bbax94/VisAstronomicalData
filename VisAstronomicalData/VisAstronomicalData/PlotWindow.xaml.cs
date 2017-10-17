using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
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
using System.Windows.Shapes;
using VisAstronomicalData.Models;
using VisAstronomicalData.Store;
using VisAstronomicalData.ViewModels;

namespace VisAstronomicalData
{
    /// <summary>
    /// Interaction logic for PlotWindow.xaml
    /// </summary>
    public partial class PlotWindow : Window
    {
        private PlotWindowModel viewModel;

        public PlotWindow()
        {
            InitializeComponent();
        }

        public void UpdatePlot(int hdu, string molecule, Query query)
        {
            Survey survey = StoreFitsData.Survey;
            List<HDUBinaryTable> binaryTables;

            if (query.Tables == null)
            {
                binaryTables = new List<HDUBinaryTable>();

                foreach (FitsCollection fitsCollection in survey.FitsCollections)
                {
                    foreach (FitsFile fitsFile in fitsCollection.FitsFiles)
                    {
                        if (fitsFile.Molecule.Equals(molecule, StringComparison.InvariantCultureIgnoreCase))
                        {
                            binaryTables.Add(fitsFile.HDUS[hdu].Table);
                            break;
                        }
                    }
                }

                query.Tables = binaryTables;
            }
           
            else
            {
                binaryTables = query.Tables;
            }

            if (viewModel == null)
            {
                viewModel = new PlotWindowModel(binaryTables, query);
            }
            else
            {
                viewModel.PlotData(binaryTables, query);
            }

            DataContext = viewModel;
        }
    }
}
