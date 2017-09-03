using nom.tam.fits;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisAstronomicalData.Models;
using VisAstronomicalData;

namespace VisAstronomicalData.ViewModels
{
    static class FitsInterpreter
    {
        static List<FitsFile> FitsFiles { get; set; }
        static readonly IList<String> headerKeys = new ReadOnlyCollection<string>(new List<String>
        {
            "BITPIX",
            "NAXIS",
            "NAXIS1",
            "NAXIS2",
            "PCOUNT",
            "GCOUNT",
            "CRPIX1",
            "CRPIX2",
            "CRVAL1",
            "CRVAL2",
            "CDELT1",
            "CDELT2"
        });

        public static List<FitsFile> InterpretFolders(string[] files)
        {
            FitsFiles = new List<FitsFile>();

            foreach (string file in files)
            {
                Fits fits = new Fits(file);
                FitsFile cSharpFits = new FitsFile(fits, file);
                BasicHDU[] hdus = fits.Read();

                foreach (BasicHDU hdu in hdus.Skip(1))
                {
                    HDU cSharpHDU = new HDU();
                    foreach (string headerKey in headerKeys)
                    {                   
                        cSharpHDU.AddHeader(headerKey, hdu.Header.GetDoubleValue(headerKey));
                    }

                    System.Array temp = (System.Array)hdu.Data.DataArray;

                    double? refPixelX = cSharpHDU.GetHeader("CRPIX1");
                    double? refPixelY = cSharpHDU.GetHeader("CRPIX2");
                    double? offsetX = cSharpHDU.GetHeader("CDELT1");
                    double? offsetY = cSharpHDU.GetHeader("CDELT2");
                    double? xCoord = cSharpHDU.GetHeader("CRVAL1");
                    double? yCoord  = cSharpHDU.GetHeader("CRVAL2");
                    double? typeOf = cSharpHDU.GetHeader("BITPIX");

                    if (typeOf == -32)
                    {
                        int count = 0;
                        foreach (float[] data in temp)
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

                                if (_x is double && _y is double && !Double.IsNaN(weight))
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
                        foreach (short[] data in temp)
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

                                if (_x is double && _y is double)
                                {
                                    double x = _x ?? 0;
                                    double y = _y ?? 0;

                                    cSharpHDU.AddPixel(x, y, weight);
                                }
                            }
                            count++;
                        }
                    }
                    
                    cSharpFits.AddHDU(cSharpHDU);
                }

                FitsFiles.Add(cSharpFits);
                fits.Close();
            }

            return FitsFiles;
        }

        public static void GenerateGraph(int hduSelect)
        {
            //REMOVE AND MAKE DYNAMIC
            hduSelect = 19;
            List<HDUBinaryTable> tables = new List<HDUBinaryTable>();

            foreach (FitsFile fits in FitsFiles)
            {
                tables.Add(fits.HDUS[19].Table);
            }

            PlotWindow plot = new PlotWindow(tables);
            plot.Show();
        }
    }
}
