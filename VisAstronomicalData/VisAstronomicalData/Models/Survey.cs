using nom.tam.fits;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace VisAstronomicalData.Models
{
    public class Survey
    {
        public string SurveyName { get; set; }
        public List<FitsCollection> FitsCollections { get; set; }
        public List<string> MoleculeNames { get; set; }

        private const char splitChar = '_';

        public Survey(string surveyName)
        {
            SurveyName = surveyName;
            FitsCollections = new List<FitsCollection>();
            MoleculeNames = new List<string>();
        }

        public void ImportFolder(string[] files)
        {
            foreach (string filePath in files)
            {
                ImportFile(filePath);
            }
        }

        private void ImportFile(string filePath)
        {
            Fits nomTamfits = new Fits(filePath);
            FitsCollection foundCollection = null;
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string moleculeName = fileName.Split(splitChar)[1];
            string name = fileName.Split(splitChar)[0];

            bool foundName = false;
            bool foundMolecule = false;

            foreach (FitsCollection fitsCollection in FitsCollections)
            {
                if (name.Equals(fitsCollection.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    foundName = true;
                    foundCollection = fitsCollection;

                    foreach (FitsFile fitsFile in fitsCollection.FitsFiles)
                    {
                        if (fitsFile.Molecule.Equals(moleculeName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            foundMolecule = true;
                            break;
                        }
                    }

                    break;
                }
            }

            if (!foundName)
            {
                foundCollection = new FitsCollection(name);
                FitsCollections.Add(foundCollection);
            }

            if (!foundMolecule && foundCollection != null)
            {
                foundCollection.AddFitsFile(new FitsFile(nomTamfits, filePath, fileName, moleculeName));
                foundCollection.AddMolecule(moleculeName);
            }

            foundMolecule = false;

            foreach (string molecule in MoleculeNames)
            {
                if (moleculeName.Equals(molecule, StringComparison.InvariantCultureIgnoreCase))
                {
                    foundMolecule = true;
                }
            }

            if (!foundMolecule)
            {
                AddMoleculeName(moleculeName);
            }

            nomTamfits.Close();
        }

        private void AddMoleculeName(string moleculeName)
        {
            foreach (string name in MoleculeNames)
            {
                if (name.Equals(moleculeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
            }

            MoleculeNames.Add(moleculeName);
        }
    }
}
