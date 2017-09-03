using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisAstronomicalData.Models
{
    class HDUHeader
    {
        Dictionary<string, double> Headers { get; set; }

        public HDUHeader()
        {
            Headers = new Dictionary<string, double>();
        }

        public void AddHeader(string key, double value)
        {
            Headers.Add(key, value);
        }

        public double? GetHeader(string key)
        {
            if (Headers.ContainsKey(key))
            {
                return Headers[key];
            }
            else
            {
                return null;
            }
        }
    }
}
