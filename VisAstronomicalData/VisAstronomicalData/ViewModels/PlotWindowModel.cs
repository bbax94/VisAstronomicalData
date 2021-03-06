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

        public PlotWindowModel(List<HDUBinaryTable> tables, Query query)
        {
            PlotModel = new PlotModel();
            this.SetUpModel();
            this.PlotData(tables, query);
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

        public void PlotData(List<HDUBinaryTable> tables, Query query)
        {
            ResetDataAndColourBar();
            ScatterSeries scatterSeries;

            foreach (HDUBinaryTable table in tables)
            {
                scatterSeries = new ScatterSeries
                {
                    MarkerType = MarkerType.Square
                };

                foreach (Pixel pixel in table.Pixels)
                {
                    if (EvaluateQueries(pixel.Weight, query))
                    {
                        scatterSeries.Points.Add(new ScatterPoint(pixel.X, pixel.Y, 3, pixel.Weight));
                    }
                }

                PlotModel.Series.Add(scatterSeries);
            }

            LinearColorAxis heatAxis = new LinearColorAxis
            {
                Position = AxisPosition.Right,
                HighColor = OxyColors.OrangeRed,
                LowColor = OxyColors.BlueViolet
            };

            PlotModel.Axes.Add(heatAxis);
            PlotModel.InvalidatePlot(true);
        }

        private bool EvaluateQueries(double weight, Query query)
        {
            if (query.Op != null)
            {
                if (!EvaluateQuery(weight, query.Op, query.Value))
                {
                    return false;
                }
            }

            if (query.Op2 != null)
            {
                if (!EvaluateQuery(weight, query.Op2, query.Value2))
                {
                    return false;
                }
            }

            return true;
        }

        private bool EvaluateQuery(double weight, string op, double value)
        {
            if (op.Equals("="))
            {
                if (weight != value)
                {
                    return false;
                }
            }
            else if (op.Equals("!="))
            {
                if (weight == value)
                {
                    return false;
                }
            }
            else if (op.Equals("<"))
            {
                if (weight >= value)
                {
                    return false;
                }
            }
            else if (op.Equals("<="))
            {
                if (weight > value)
                {
                    return false;
                }
            }
            else if (op.Equals(">"))
            {
                if (weight <= value)
                {
                    return false;
                }
            }
            else if (op.Equals(">="))
            {
                if (weight < value)
                {
                    return false;
                }
            }

            return true;
        }

        private void ResetDataAndColourBar()
        {
            while (PlotModel.Series.Count > 0)
            {
                PlotModel.Series.RemoveAt(0);
            }

            if (PlotModel.Axes.Count == 3)
            {
                PlotModel.Axes.RemoveAt(2);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
