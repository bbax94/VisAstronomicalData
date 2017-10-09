using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisAstronomicalData.Models
{
    public class FitsCollection
    {
        public string Name { get; set; }

        public List<FitsFile> FitsFiles { get; set; }

        public List<String> Molecules { get; set; }

        public FitsCollection(string name)
        {
            Name = name;
            FitsFiles = new List<FitsFile>();
            Molecules = new List<string>();
        }

        public void AddFitsFile(FitsFile fitsFile)
        {
            FitsFiles.Add(fitsFile);
        }

        public void AddMolecule(string molecule)
        {
            Molecules.Add(molecule);
        }
    }
}
