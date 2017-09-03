using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisAstronomicalData.Models
{
    public class HDUBinaryTable
    {
        public List<Pixel> Pixels { get; set; }
        public double Max { get; private set; }
        public double Min { get; private set; }

        public HDUBinaryTable()
        {
            Pixels = new List<Pixel>();
            Max = Double.MinValue;
            Min = Double.MaxValue;
        }

        public void AddPixel(Pixel pixel)
        {
            Pixels.Add(pixel);
        }

        public void AddPixel(double x, double y, double weight)
        {
            Pixels.Add(new Pixel(x, y, weight));

            if (weight > Max)
            {
                Max = weight;
            }

            if (weight < Min)
            {
                Min = weight;
            }
        }
    }
}
