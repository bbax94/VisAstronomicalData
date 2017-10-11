using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using VisAstronomicalData.Models;
using VisAstronomicalData.Store;
using VisAstronomicalData.ViewModels;

namespace VisAstronomicalData
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<string> HDUSelectionBoxChoices;
        ObservableCollection<string> MoleculeSelectionBoxChoices;

        public MainWindow()
        {
            InitializeComponent();

            HDUSelectionBoxChoices = new ObservableCollection<string>();
            HDUSelectionBox.ItemsSource = HDUSelectionBoxChoices;
            HDUSelectionBoxChoices.Add("Please Import Data");
            HDUSelectionBox.SelectedIndex = 0;

            MoleculeSelectionBoxChoices = new ObservableCollection<string>();
            MoleculeSelectionBox.ItemsSource = MoleculeSelectionBoxChoices;
            MoleculeSelectionBoxChoices.Add("Please Import Data");
            MoleculeSelectionBox.SelectedIndex = 0;
        }

        private void ImportFolderButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = this.FolderSelect();
            Survey survey = new Survey(Path.GetFileName(filePath));
            string[] files = Directory.GetFiles(filePath, "*.fits", SearchOption.AllDirectories);
            survey.ImportFolder(files);
            StoreFitsData.Survey = survey;
            PopulateComboBoxes();
            //PopulateTreeView();
        }

        private void GenerateGraphButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HDUSelectionBox.SelectedItem.ToString().Equals("Select A HDU") &&
                    !MoleculeSelectionBox.SelectedItem.ToString().Equals("Select a Molecule"))
            {
                StoreWindows.WindowPlot.UpdatePlot(
                    Convert.ToInt32(HDUSelectionBox.SelectedValue.ToString()) - 1,
                    MoleculeSelectionBox.SelectedValue.ToString()
                    );
            }
            else
            {
                //popup box error
            }
        }

        private void GenerateFileListButton_Click(object sender, RoutedEventArgs e)
        {
            //PopulateFileInfo(GetSelectedCheckBoxes(FileList.Items));
        }

        private void PopulateFileInfo(List<string> list)
        {
            
        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------------------
        private string FolderSelect()
        {
            string filePath = "";

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    filePath = fbd.SelectedPath;
                }
            }

            return filePath;
        }

        private void PopulateComboBoxes()
        {
            int count = 0;
            List<string> names;
            Survey survey = StoreFitsData.Survey;

            HDUSelectionBox.Focusable = true;
            HDUSelectionBox.IsHitTestVisible = true;
            HDUSelectionBoxChoices = new ObservableCollection<string>
            {
                "Select A HDU"
            };
            count = survey.FitsCollections[0].FitsFiles[0].HDUS.Count();

            for (int i = 1; i <= count; i++)
            {
                HDUSelectionBoxChoices.Add(i.ToString());
            }

            MoleculeSelectionBox.Focusable = true;
            MoleculeSelectionBox.IsHitTestVisible = true;
            MoleculeSelectionBoxChoices = new ObservableCollection<string>
            {
                "Select A Molecule"
            };

            names = survey.MoleculeNames;

            foreach (string name in names)
            {
                MoleculeSelectionBoxChoices.Add(name);
            }

            MoleculeSelectionBox.ItemsSource = MoleculeSelectionBoxChoices;
            HDUSelectionBox.ItemsSource = HDUSelectionBoxChoices;
            MoleculeSelectionBox.SelectedIndex = 0;
            HDUSelectionBox.SelectedIndex = 0;
        }

        /*private void PopulateTreeView()
        {
            List<List<string>> allNames = FitsInterpreter.GetFitsNames();
            List<string> moleculeNames = FitsInterpreter.Molecules;
            TreeViewItem treeItem = null;

            for (int i = 0; i < moleculeNames.Count; i++)
            {
                treeItem = new TreeViewItem
                {
                    Header = moleculeNames[i]
                };

                foreach (string name in allNames[i])
                {
                    TreeViewItem subTreeItem = new TreeViewItem()
                    {
                        Header = name,
                        HeaderTemplate = GetHeaderTemplate()
                    };

                    treeItem.Items.Add(subTreeItem);
                }

                FileList.Items.Add(treeItem);
            }
        }

        //---------------------------------------------------------------------------------------------
        private DataTemplate GetHeaderTemplate()
        {
            //create the data template
            DataTemplate dataTemplate = new DataTemplate();

            //create stack pane;
            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.Name = "parentStackpanel";
            stackPanel.SetValue(StackPanel.OrientationProperty, System.Windows.Controls.Orientation.Horizontal);

            // Create check box
            FrameworkElementFactory checkBox = new FrameworkElementFactory(typeof(System.Windows.Controls.CheckBox));
            checkBox.Name = "chk";
            checkBox.SetValue(NameProperty, "chk");
            checkBox.SetValue(TagProperty, new System.Windows.Data.Binding());
            checkBox.SetValue(MarginProperty, new Thickness(2));
            stackPanel.AppendChild(checkBox);

            // create text
            FrameworkElementFactory label = new FrameworkElementFactory(typeof(TextBlock));
            label.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding());
            label.SetValue(TextBlock.ToolTipProperty, new System.Windows.Data.Binding());
            stackPanel.AppendChild(label);

            //set the visual tree of the data template
            dataTemplate.VisualTree = stackPanel;

            return dataTemplate;
        }

        private List<string> GetSelectedCheckBoxes(ItemCollection items)
        {
            List<string> list = new List<string>();

            foreach (TreeViewItem subTree in items)
            {
                foreach (TreeViewItem item in subTree.Items)
                {
                    UIElement elemnt = GetChildControl(item, "chk");
                    if (elemnt != null)
                    {
                        System.Windows.Controls.CheckBox chk = (System.Windows.Controls.CheckBox)elemnt;
                        if (chk.IsChecked.HasValue && chk.IsChecked.Value)
                        {
                            list.Add(item.Header.ToString());
                        }
                    }
                }
            }

            return list;
        }

        private UIElement GetChildControl(DependencyObject parentObject, string childName)
        {

            UIElement element = null;

            if (parentObject != null)
            {
                int totalChild = VisualTreeHelper.GetChildrenCount(parentObject);
                for (int i = 0; i < totalChild; i++)
                {
                    DependencyObject childObject = VisualTreeHelper.GetChild(parentObject, i);

                    if (childObject is FrameworkElement &&
                        ((FrameworkElement)childObject).Name == childName)
                    {
                        element = childObject as UIElement;
                        break;
                    }

                    element = GetChildControl(childObject, childName);
                    if (element != null) break;
                }
            }

            return element;
        }*/
    }
}
