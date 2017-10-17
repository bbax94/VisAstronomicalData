using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisAstronomicalData.Models
{
    public class Query
    {
        public string Op { get; set; }
        public double Value { get; set; }
        public string Op2 { get; set; }
        public double Value2 { get; set; }
        public string Molecule { get; set; }
        public int HDU { get; set; }
        public string HDUName { get; set; }
        public List<HDUBinaryTable> Tables { get; set; }

        public Query()
        {

        }
    }
}
