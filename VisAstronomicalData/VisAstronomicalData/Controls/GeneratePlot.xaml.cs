using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisAstronomicalData.Models;
using VisAstronomicalData.Store;

namespace VisAstronomicalData
{
    /// <summary>
    /// Interaction logic for GeneratePlot.xaml
    /// </summary>
    public partial class GeneratePlot : UserControl
    {
        public List<string> QueriesList { get; private set; }
        public GeneratePlot()
        {
            QueriesList = UpdateQueries();
            InitializeComponent();
            QueryComboBox.ItemsSource = QueriesList;
        }

        private void GeneratePlot_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);            

            if (MoleculeComboBox.SelectedValue != null && HDUComboBox.SelectedValue != null)
            {
                mainWindow.GenerateGraph(MoleculeComboBox.SelectedValue.ToString(), HDUComboBox.SelectedIndex, EvaluateQueries());
            }
        }

        private Query EvaluateQueries()
        {
            List<Query> queries = StoreFitsData.Queries;
            string op = "";
            double value = 0;

            Query query = new Query();
            query.Molecule = MoleculeComboBox.SelectedValue.ToString();
            query.HDU = HDUComboBox.SelectedIndex;
            query.HDUName = HDUComboBox.SelectedValue.ToString();

            if (!(OperatorComboBox_1.SelectedItem == null || TextField_1.Text== null))
            {
                op = ((ComboBoxItem)OperatorComboBox_1.SelectedItem).Content.ToString();

                if (Double.TryParse(TextField_1.Text, out value))
                {
                    query.Op = op;
                    query.Value = value;
                }
            }

            if (!(OperatorComboBox_2.SelectedItem == null || TextField_2.Text == null))
            {
                op = ((ComboBoxItem)OperatorComboBox_2.SelectedItem).Content.ToString();

                if (Double.TryParse(TextField_2.Text, out value))
                {
                    query.Op2 = op;
                    query.Value2 = value;
                }
            }

            if (queries == null)
            {
                queries = new List<Query>();
            }

            queries.Add(query);
            StoreFitsData.Queries = queries;
            QueriesList = UpdateQueries();
            QueryComboBox.ItemsSource = QueriesList;

            return query;
        }

        private List<string> UpdateQueries()
        {
            List<string> queries = new List<string>();

            if (StoreFitsData.Queries == null)
            {
                StoreFitsData.Queries = new List<Query>();
            }

            foreach (Query query in StoreFitsData.Queries)
            {
                string queryString = "";
                queryString += query.Molecule + ", [" + query.HDUName + "] ";

                if (query.Op != null)
                {
                    queryString += query.Op + " " + query.Value;
                }

                if (query.Op2 != null)
                {
                    queryString += ", " + query.Op2 + " " + query.Value2;
                }

                queries.Add(queryString);
            }

            return queries;
        }

        private void QueryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedQuery = QueryComboBox.SelectedValue.ToString();
            int index = QueriesList.IndexOf(selectedQuery);

            List<Query> queries = StoreFitsData.Queries;
            Query query = queries[index];

            string molecule = query.Molecule;
            int hdu = query.HDU;

            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.GenerateGraph(query.Molecule, query.HDU, query);
        }
    }
}
