using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisAstronomicalData.Models
{
    static class HeaderKeysList
    {
        public static readonly IList<String> HeaderKeys = new ReadOnlyCollection<string>(new List<String>
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
            "CDELT2",
            "COMMENT"
        });
    }
}
