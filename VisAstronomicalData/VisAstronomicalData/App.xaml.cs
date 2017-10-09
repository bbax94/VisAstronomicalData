using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VisAstronomicalData.Models;
using VisAstronomicalData.Store;

namespace VisAstronomicalData
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        PlotWindow plotWindow;

        void App_Startup(object sender, StartupEventArgs e)
        {
            plotWindow = new PlotWindow();
            plotWindow.Show();
            StoreWindows.WindowPlot = plotWindow;
        }
    }
}
