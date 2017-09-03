using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisAstronomicalData.Models
{
    class HDU
    {
        public HDUHeader Header { get; set; }
        public HDUBinaryTable Table { get; set; }

        public HDU()
        {
            Header = new HDUHeader();
            Table = new HDUBinaryTable();
        }

        public void AddPixel(Pixel pixel)
        {
            Table.AddPixel(pixel);
        }

        public void AddPixel(double x, double y, double weight)
        {
            Table.AddPixel(x, y, weight);
        }

        public void AddHeader(string key, double value)
        {
            Header.AddHeader(key, value);
        }

        public double? GetHeader(string key)
        {
            return Header.GetHeader(key);
        }
    }
}
