using nom.tam.fits;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisAstronomicalData.Models
{
    public class FitsFile
    {

        public List<HDU> HDUS { get; set; }
        public Fits NomTamFits { get; set; }
        public string Filepath { get; set; }
        public string Name { get; set; }
        public string Molecule { get; set; }

        private const char splitChar = ' ';

        public FitsFile(Fits nomTamFits, string filepath, string name, string molecule)
        {
            NomTamFits = nomTamFits;
            Filepath = filepath;
            Name = name;
            Molecule = molecule;
            HDUS = new List<HDU>();

            EvaluateHDUS();
        }

        public void EvaluateHDUS()
        {
            BasicHDU[] hdus = NomTamFits.Read();

            foreach (BasicHDU hdu in hdus.Skip(1))
            {           
                HDU cSharpHDU = new HDU();
                foreach (string headerKey in HeaderKeysList.HeaderKeys)
                {
                    if (headerKey.Equals("COMMENT", StringComparison.InvariantCultureIgnoreCase))
                    {
                        cSharpHDU.Header.DataType = hdu.Header.GetCard(14).Split(splitChar)[2];
                    }
                    else
                    {
                        cSharpHDU.AddHeader(headerKey, hdu.Header.GetDoubleValue(headerKey));
                    }
                    
                }

                System.Array hduDataArray = (System.Array)hdu.Data.DataArray;
                HDUS.Add(ReadDataArray(cSharpHDU, hduDataArray));
            }
        }

        public HDU ReadDataArray(HDU cSharpHDU, System.Array hduDataArray)
        {
            double? refPixelX = cSharpHDU.GetHeader("CRPIX1");
            double? refPixelY = cSharpHDU.GetHeader("CRPIX2");
            double? offsetX = cSharpHDU.GetHeader("CDELT1");
            double? offsetY = cSharpHDU.GetHeader("CDELT2");
            double? xCoord = cSharpHDU.GetHeader("CRVAL1");
            double? yCoord = cSharpHDU.GetHeader("CRVAL2");
            double? typeOf = cSharpHDU.GetHeader("BITPIX");

            if (typeOf == -32)
            {
                int count = 0;
                foreach (float[] data in hduDataArray)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        double? _x;
                        double? _y;
                        double weight;

                        _x = xCoord + ((refPixelX + count - 1) * offsetX);
                        _x = ((_x + 180) % 360) - 180;
                        _y = yCoord + ((refPixelY + i - 1) * offsetY);
                        weight = data[i];

                        if (_x is double && _y is double && !Double.IsNaN(weight) && weight != 999)
                        {
                            double x = _x ?? 0;
                            double y = _y ?? 0;

                            cSharpHDU.AddPixel(x, y, weight);
                        }
                    }
                    count++;
                }
            }

            else if (typeOf == 16)
            {
                foreach (short[] data in hduDataArray)
                {
                    int count = 0;
                    for (int i = 0; i < data.Length; i++)
                    {
                        double? _x;
                        double? _y;
                        double weight;

                        _x = xCoord + ((refPixelX + count - 1) * offsetX);
                        _y = yCoord + ((refPixelY + i - 1) * offsetY);
                        weight = data[i];

                        if (_x is double && _y is double && !Double.IsNaN(weight) && weight != 999)
                        {
                            double x = _x ?? 0;
                            double y = _y ?? 0;

                            cSharpHDU.AddPixel(x, y, weight);
                        }
                    }
                    count++;
                }
            }

            return cSharpHDU;
        }
    }
}
