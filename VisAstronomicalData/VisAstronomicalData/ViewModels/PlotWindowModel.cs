﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using OxyPlot;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisAstronomicalData.Models;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace VisAstronomicalData.ViewModels
{
    public class PlotWindowModel : INotifyPropertyChanged
    {
        private PlotModel plotModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; OnPropertyChanged("PlotModel"); }
        }

        public PlotWindowModel(List<HDUBinaryTable> tables)
        {
            PlotModel = new PlotModel();
            this.SetUpModel();
            this.PlotData(tables);
        }

        private void SetUpModel()
        {
            PlotModel.LegendTitle = "Legend";
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;

            LinearAxis xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "X",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                MinorGridlineColor = OxyColor.FromRgb(211, 211, 211),
                IntervalLength = 100,
                StartPosition = 1,
                EndPosition = 0
            };

            LinearAxis yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Y",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                MinorGridlineColor = OxyColor.FromRgb(211, 211, 211),
                IntervalLength = 100
            };

            PlotModel.Axes.Add(xAxis);
            PlotModel.Axes.Add(yAxis);
        }

        private void PlotData(List<HDUBinaryTable> tables)
        {
            Random random = new Random();
            ScatterSeries scatterSeries;

            double max = Double.MinValue;
            double min = Double.MaxValue;

            foreach (HDUBinaryTable table in tables)
            {
                scatterSeries = new ScatterSeries
                {
                    MarkerType = MarkerType.Square
                };

                foreach (Pixel pixel in table.Pixels)
                {
                    scatterSeries.Points.Add(new ScatterPoint(pixel.X, pixel.Y, 3, pixel.Weight));
                }

                if (table.Max > max)
                {
                    max = table.Max;
                }

                if (table.Min < min)
                {
                    min = table.Min;
                }
                PlotModel.Series.Add(scatterSeries);
            }

            LinearColorAxis heatAxis = new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Minimum = min,
                Maximum = max,
                HighColor = OxyColors.OrangeRed,
                LowColor = OxyColors.BlueViolet
            };

            PlotModel.Axes.Add(heatAxis);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}