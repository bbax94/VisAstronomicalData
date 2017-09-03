using nom.tam.fits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisAstronomicalData.Models
{
    class FitsFile
    {
        public List<HDU> HDUS { get; set; }
        public Fits NomTamFits { get; set; }
        public String Filepath { get; set; }
        public String Name { get; set; }

        public FitsFile()
        {
            HDUS = new List<HDU>();
            this.NomTamFits = null;
            this.Filepath = "";
        }
        public FitsFile(Fits NomTamFits, string filepath)
        {
            HDUS = new List<HDU>();
            this.NomTamFits = NomTamFits;
            this.Filepath = filepath;
        }

        public void AddHDU(HDU hdu)
        {
            HDUS.Add(hdu);
        }
    }
}
