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
        public string SurveyName { get; private set; }
        public string FolderLocation { get; private set; }
        public List<FitsCollection> FitsCollections { get; set; }
        public List<string> MoleculeNames { get; private set; }
        public List<string> HDUNames { get; private set; }
        public int FitsCount { get; private set; }
        public int MoleculeCount { get; private set; }
        public int HDUCount { get; private set; }
        public List<Query> Queries { get; private set; }
        public List<string> QueriesList { get; private set; }

        private const char splitChar = '_';

        public Survey(string folder)
        {
            SurveyName = Path.GetFileName(folder);
            FolderLocation = folder;
            FitsCollections = new List<FitsCollection>();
            MoleculeNames = new List<string>();
            FitsCount = 0;
            MoleculeCount = 0;
            HDUCount = 0;
            HDUNames = new List<string>();
        }

        public void ImportFolder(string[] files)
        {
            FitsCount = files.Length;

            foreach (string filePath in files)
            {
                ImportFile(filePath);
            }

            GetHDUCount();
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
                MoleculeCount++;
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

        private void GetHDUCount()
        {
            HDUNames = new List<string>();
            HDUCount = FitsCollections[0].FitsFiles[0].HDUS.Count;
            foreach (HDU hdu in FitsCollections[0].FitsFiles[0].HDUS)
            {
                HDUNames.Add(hdu.Header.DataType);
            }
        }
    }
}
