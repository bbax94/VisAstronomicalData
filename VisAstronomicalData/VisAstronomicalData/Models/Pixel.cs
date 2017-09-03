using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisAstronomicalData.Models
{
    public class Pixel
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Weight { get; set; }

        public Pixel(double x, double y, double weight)
        {
            this.X = x;
            this.Y = y;
            this.Weight = weight;
        }
    }
}
